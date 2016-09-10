﻿using NBench;

namespace Hunspell.NetCore.Performance.Tests
{
    public class EnWordSuggestPerfSpec : EnWordPerfBase
    {
        protected Counter SuggestionQueries;

        [PerfSetup]
        public override void Setup(BenchmarkContext context)
        {
            base.Setup(context);
            SuggestionQueries = context.GetCounter(nameof(SuggestionQueries));
        }

        [PerfBenchmark(
            Description = "Ensure that words can be suggested quickly.",
            NumberOfIterations = 1,
            RunMode = RunMode.Throughput,
            TestMode = TestMode.Measurement)]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        [GcMeasurement(GcMetric.TotalCollections, GcGeneration.AllGc)]
        [TimingMeasurement]
        [CounterMeasurement(nameof(SuggestionQueries))]
        [CounterThroughputAssertion(nameof(SuggestionQueries), MustBe.GreaterThanOrEqualTo, 200000)]
        public void Benchmark(BenchmarkContext context)
        {
            foreach (var word in Words)
            {
                var result = Checker.Check(word);
                SuggestionQueries.Increment();
            }
        }
    }
}
