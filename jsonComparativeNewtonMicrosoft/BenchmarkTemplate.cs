using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;

namespace jsonComparativeNewtonMicrosoft;

[RPlotExporter]
[SimpleJob(
    RuntimeMoniker.Net80,
    1,
    3,
    5,
    -1,
    "NET 8.0",
    true
)]
[MemoryDiagnoser(false)]
[HideColumns(Column.Job, Column.StdDev, Column.Error, Column.RatioSD, Column.AllocRatio)]
internal class BenchmarkTemplate
{
    [Params(1000)] public int Count { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
    }

    [Benchmark]
    public void Benchmark()
    {
    }
}