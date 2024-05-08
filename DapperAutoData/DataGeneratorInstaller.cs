using AutoFixture;
using DapperAutoData.Generators;

namespace DapperAutoData;

/// <summary>
/// This class is responsible for registering all the data generators in the assembly.
/// It uses reflection to find all the classes that implement the IDataGenerator interface and registers them.
/// It also filters out the DateTimeGenerator and DateTimeOffsetGenerator classes as they are generic and should not be registered.
/// </summary>
public static class DataGeneratorInstaller
{
    public static void Run(IFixture fixture)
    {
        var list = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
                    .Where(type => typeof(IDataGenerator).IsAssignableFrom(type)
                           && type is { IsInterface: false, IsAbstract: false, ContainsGenericParameters: false }
                           && !IsSubclassOfRawGeneric(typeof(DateTimeGenerator<>), type)
                           && !IsSubclassOfRawGeneric(typeof(DateTimeOffsetGenerator<>), type))
            .ToList();
        list.ForEach(type =>
            (Activator.CreateInstance(type) as IDataGenerator)?.RegisterGenerators(fixture));
    }

    private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
    {
        while (toCheck != null && toCheck != typeof(object))
        {
            var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (generic == cur)
            {
                return true;
            }
            toCheck = toCheck.BaseType;
        }
        return false;
    }

}