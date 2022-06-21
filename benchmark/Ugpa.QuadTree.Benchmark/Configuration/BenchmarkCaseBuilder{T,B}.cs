using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BenchmarkDotNet.Jobs;

namespace Ugpa.QuadTree.Benchmark.Configuration
{
    internal abstract class BenchmarkCaseBuilder<T, B> : BenchmarkCaseBuilder
        where B : BenchmarkCaseBuilder<T, B>
    {
        protected BenchmarkCaseBuilder(Job job)
        {
            Job = job;
        }

        protected Job Job { get; }

        protected MethodInfo Workload { get; private set; }

        protected MethodInfo GlobalSetup { get; private set; }

        protected MethodInfo IterationSetup { get; private set; }

        public B WithWorkload(Expression<Action<T>> workload)
        {
            Workload = GetRealMethodInfo((MethodCallExpression)workload.Body);
            return (B)this;
        }

        public B WithWorkload<R>(Expression<Func<T, R>> workload)
        {
            Workload = GetRealMethodInfo((MethodCallExpression)workload.Body);
            return (B)this;
        }

        public B WithGlobalSetup(Expression<Action<T>> globalSetup)
        {
            GlobalSetup = GetRealMethodInfo((MethodCallExpression)globalSetup.Body);
            return (B)this;
        }

        public B WithIterationSetup(Expression<Action<T>> iterationSetup)
        {
            IterationSetup = GetRealMethodInfo((MethodCallExpression)iterationSetup.Body);
            return (B)this;
        }
        private MethodInfo GetRealMethodInfo(MethodCallExpression method)
        {
            if (method is null)
                return null;

            if (method.Method.DeclaringType == typeof(T))
                return method.Method;

            return method.Object.Type.GetMethods().SingleOrDefault(_ => _.GetBaseDefinition() == method.Method) ?? method.Method;
        }
    }
}
