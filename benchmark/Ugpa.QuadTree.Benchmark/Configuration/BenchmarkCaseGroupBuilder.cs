using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Parameters;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Ugpa.QuadTree.Benchmark.Configuration
{
    internal sealed class BenchmarkCaseGroupBuilder<T> : BenchmarkCaseBuilder<T, BenchmarkCaseGroupBuilder<T>>
    {
        private readonly Dictionary<PropertyInfo, object[]> parameters = new Dictionary<PropertyInfo, object[]>();

        private BenchmarkCaseGroupBuilder(Job job)
            : base(job)
        {
        }

        public static BenchmarkCaseGroupBuilder<T> Create(Job job)
        {
            return new BenchmarkCaseGroupBuilder<T>(job);
        }

        public BenchmarkCaseGroupBuilder<T> WithParameterValues<P>(Expression<Func<T, P>> property, params P[] values)
        {
            var propertyInfo = (PropertyInfo)((MemberExpression)property.Body).Member;
            parameters[propertyInfo] = values.Cast<object>().ToArray();
            return this;
        }

        public override BenchmarkCase[] Build(ImmutableConfig config)
        {
            var caseDescription = new Descriptor(
                typeof(T),
                Workload,
                globalSetupMethod: GlobalSetup,
                globalCleanupMethod: null,
                iterationSetupMethod: IterationSetup,
                iterationCleanupMethod: null,
                description: null,
                additionalLogic: null,
                baseline: false,
                categories: null,
                operationsPerInvoke: 1,
                methodIndex: 0);


            if (parameters.Any())
            {
                var defs = parameters
                    .Select(_ => new ParameterDefinition(_.Key.Name, _.Key.GetGetMethod().IsStatic, _.Value, false, _.Key.PropertyType, 0))
                    .ToArray();

                var insts = defs
                    .SelectMany(d => d.Values.Select(v => new ParameterInstance(d, v, SummaryStyle.Default)))
                    .ToLookup(_ => _.Definition)
                    .Select(_ => (_.Key, _.ToList()))
                    .ToArray();

                return GetParameterInstances(new Stack<(ParameterDefinition, List<ParameterInstance>)>(insts))
                    .Select(_ => BenchmarkCase.Create(caseDescription, Job, new ParameterInstances(_.ToList()), config))
                    .ToArray();
            }
            else
            {
                return new[] { BenchmarkCase.Create(caseDescription, Job, new ParameterInstances(new List<ParameterInstance>()), config) };
            }
        }

        private IEnumerable<Queue<ParameterInstance>> GetParameterInstances(Stack<(ParameterDefinition, List<ParameterInstance>)> inputs)
        {
            var def = inputs.Pop();

            if (inputs.Any())
            {
                foreach (var inst in def.Item2)
                {
                    foreach (var item in GetParameterInstances(inputs))
                    {
                        var q = new Queue<ParameterInstance>();
                        foreach (var i in item)
                            q.Enqueue(i);

                        q.Enqueue(inst);
                        yield return q;
                    }
                }
            }
            else
            {
                foreach (var inst in def.Item2)
                {
                    var q = new Queue<ParameterInstance>();
                    q.Enqueue(inst);
                    yield return q;
                }
            }

            inputs.Push(def);
        }
    }
}
