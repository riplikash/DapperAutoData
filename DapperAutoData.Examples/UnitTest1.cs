using AutoFixture;

namespace DapperAutoData.Examples;

public class UnitTest1
{

    [Theory]
    [DapperAutoData]
    public void Test1(TestClass a)
    {
        var b = a;
    }
}

public class TestClass
{
    public string Name { get; set; }
}

public class TestClassGenerator : IDataGenerator
{
    public void RegisterGenerators(IFixture fixture)
    {
        // Customize generator so that the name is always Steve
        fixture.Customize<TestClass>(c => c.With(x => x.Name, "Steve"));
    }
}

