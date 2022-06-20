using System;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;

namespace Ugpa.QuadTree.Benchmark
{
    public sealed class Benchmark
    {
        public static void Main()
        {
            var cfg = ManualConfig
                .CreateEmpty()
                .AddJob(Job.Default
#if DEBUG
                    .WithStrategy(BenchmarkDotNet.Engines.RunStrategy.ColdStart)
                    .WithToolchain(new BenchmarkDotNet.Toolchains.InProcess.Emit.InProcessEmitToolchain(TimeSpan.FromHours(1.0), logOutput: true))
#endif
                    )
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

            BenchmarkSwitcher
                .FromTypes(
                    new[]
                    {
                        typeof(SimpleBoundComparisonBenchmark),
                        typeof(QuadTreeBenchmark)
                    })
                .RunAllJoined(cfg);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("// Done");
            Console.WriteLine("// Press Enter to close");
            Console.ForegroundColor = ConsoleColor.White;

            Console.ReadLine();
        }
    }
}
