using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;
using Bogus;
using jsonComparativeNewtonMicrosoft.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
[HideColumns(Column.Job, Column.StdDev, Column.Error, Column.RatioSD)]
public class NewtonsoftVsMicrosoftSerialization
{
    [Params(100000)] public int Count { get; set; }

    private List<User>? _testUsers;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var faker = new Faker<User>().CustomInstantiator(
            f =>
                new User(
                    Guid.NewGuid(),
                    f.Name.FirstName(),
                    f.Name.LastName(),
                    f.Internet.Email(f.Name.FirstName(), f.Name.LastName())
                )
        );

        _testUsers = faker.Generate(Count);
    }

    [Benchmark]
    public void NewtonsoftSerializeBigData()
    {
        _ = JsonConvert.SerializeObject(_testUsers);
    }

    [Benchmark]
    public void MicrosoftSerializeBigData()
    {
        _ = JsonSerializer.Serialize(_testUsers);
    }

    [Benchmark]
    public void NewtonsoftSerializeIndividualData()
    {
        foreach (var user in _testUsers) _ = JsonConvert.SerializeObject(user);
    }

    [Benchmark]
    public void MicrosoftSerializeIndividualData()
    {
        foreach (var user in _testUsers) _ = JsonSerializer.Serialize(user);
    }
}