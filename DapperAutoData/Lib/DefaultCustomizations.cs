using AutoFixture;
using DapperAutoData.Content;

namespace DapperAutoData;

public class DefaultCustomizations : ICustomization
{
    public void Customize(IFixture fixture)
    {
        DataGeneratorInstaller.Run(fixture);
    }
}