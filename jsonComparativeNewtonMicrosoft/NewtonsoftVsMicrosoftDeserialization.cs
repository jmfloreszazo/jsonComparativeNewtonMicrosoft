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
public class NewtonsoftVsMicrosoftDeserialization
{
    [Params(100000)] public int Count { get; set; }

    private string serializedTestUsers;

    private readonly List<string>? _serializedTestUsersList;

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

        var testUsers = faker.Generate(Count);

        serializedTestUsers = JsonSerializer.Serialize(testUsers);

        foreach (var user in testUsers.Select(u => JsonSerializer.Serialize(u))) _serializedTestUsersList?.Add(user);
    }

    [Benchmark]
    public void NewtonsoftDeserializeBigData()
    {
        _ = JsonConvert.DeserializeObject<List<User>>(serializedTestUsers);
    }

    [Benchmark]
    public void MicrosoftDeserializeBigData()
    {
        _ = JsonSerializer.Deserialize<List<User>>(serializedTestUsers);
    }

    [Benchmark]
    public void NewtonsoftDeserializeIndividualData()
    {
        if (_serializedTestUsersList != null)
            foreach (var user in _serializedTestUsersList)
                _ = JsonConvert.DeserializeObject<User>(user);
    }

    [Benchmark]
    public void MicrosoftDeserializeIndividualData()
    {
        if (_serializedTestUsersList != null)
            foreach (var user in _serializedTestUsersList)
                _ = JsonSerializer.Deserialize<User>(user);
    }
}