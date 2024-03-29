﻿using AutoFixture;

namespace DapperAutoData.Lib.Generators;

public class NegativeNumber : IDataGenerator
{
    public int Value;

    public NegativeNumber(int value)
    {
        Value = value;
    }

    public NegativeNumber()
    {
    }

    public static implicit operator NegativeNumber(int y) => new NegativeNumber(y);
    public static implicit operator int(NegativeNumber x) => x.Value;
    public static implicit operator double(NegativeNumber x) => x.Value;

    public static NegativeNumber Generate(IFixture fixture) => fixture.Create<PositiveNumber>() * -1;

    public override string? ToString()
    {
        return Value.ToString();
    }
    public void RegisterGenerators(IFixture fixture) => fixture.Register(() => Generate(fixture));

}