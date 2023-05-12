using AutoFixture;

namespace DapperAutoData;

public static class DataGeneratorInstaller
{
    public static void Run(IFixture fixture)
    {
        var list = System.Reflection.Assembly.GetCallingAssembly()
            .GetTypes()
            .Where(type => typeof(IDataGenerator).IsAssignableFrom(type) && !type.IsInterface).ToList();
        list.ForEach(type =>
            (Activator.CreateInstance(type) as IDataGenerator)?.RegisterGenerators(fixture));
    }

}