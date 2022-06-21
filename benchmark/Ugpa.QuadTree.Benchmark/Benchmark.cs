using System;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using Ugpa.QuadTree.Benchmark.Configuration;

namespace Ugpa.QuadTree.Benchmark
{
    public sealed class Benchmark
    {
        public static void Main()
        {
            var maxDepths = new[] { 1, 2, 3, 4, 5 };
            var nodeSizes = new[] { 256, 512, 1024 };
            var areaMaxSizes = new[] { 128, 512, 2048 };

            var pointHitTestJob = CreateJob();
            var boundsHitTestJob = CreateJob();

            var cfg = ManualConfig
                .CreateEmpty()
                .AddJob(pointHitTestJob, boundsHitTestJob)
                .AddColumnProvider(
                    DefaultColumnProviders.Descriptor,
                    DefaultColumnProviders.Job,
                    DefaultColumnProviders.Statistics,
                    DefaultColumnProviders.Params,
                    DefaultColumnProviders.Metrics)
                .AddLogger(ConsoleLogger.Default)
#if DEBUG
                .WithOptions(ConfigOptions.DisableOptimizationsValidator)
#endif
                ;

            var immutableCfg = ImmutableConfigBuilder.Create(cfg);

            BenchmarkRunner.Run(BenchmarkRunInfoBuilder
                .Create()
                .AddCase<SimplePointInBoundsBenchmark>(
                    pointHitTestJob,
                    caseCfg => caseCfg
                        .WithWorkload(_ => _.PointHitTest())
                        .WithGlobalSetup(_ => _.GlobalSetup())
                        .WithIterationSetup(_ => _.IterationSetup()))
                .AddCases<QuadTreePickInPointBenchmark>(
                    pointHitTestJob,
                    caseCfg => caseCfg
                        .WithWorkload(_ => _.PointHitTest())
                        .WithGlobalSetup(_ => _.GlobalSetup())
                        .WithIterationSetup(_ => _.IterationSetup())
                        .WithParameterValues(_ => _.MaxDepth, maxDepths)
                        .WithParameterValues(_ => _.NodeSize, nodeSizes))
                .Build(immutableCfg));

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("// Done");
            Console.WriteLine("// Press Enter to continue");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();

            BenchmarkRunner.Run(BenchmarkRunInfoBuilder
                .Create()
                .AddCases<SimpleBoundsIntersectionBenchmark>(
                    boundsHitTestJob,
                    caseCfg => caseCfg
                        .WithWorkload(_ => _.BoundsHitTest())
                        .WithGlobalSetup(_ => _.GlobalSetup())
                        .WithIterationSetup(_ => _.IterationSetup())
                        .WithParameterValues(_ => _.AreaMaxSize, areaMaxSizes))
                .AddCases<QuadTreePickInBoundsBenchmark>(
                    boundsHitTestJob,
                    caseCfg => caseCfg
                        .WithWorkload(_ => _.BoundsHitTest())
                        .WithGlobalSetup(_ => _.GlobalSetup())
                        .WithIterationSetup(_ => _.IterationSetup())
                        .WithParameterValues(_ => _.MaxDepth, maxDepths)
                        .WithParameterValues(_ => _.NodeSize, nodeSizes)
                        .WithParameterValues(_ => _.AreaMaxSize, areaMaxSizes))
                .Build(immutableCfg));

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("// Done");
            Console.WriteLine("// Press Enter to continue");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
        }

        private static Job CreateJob()
        {
            return Job.Default
#if DEBUG
                .WithStrategy(BenchmarkDotNet.Engines.RunStrategy.ColdStart)
                .WithToolchain(new BenchmarkDotNet.Toolchains.InProcess.Emit.InProcessEmitToolchain(TimeSpan.FromHours(1.0), logOutput: true))
#endif
                ;
        }
    }
}
