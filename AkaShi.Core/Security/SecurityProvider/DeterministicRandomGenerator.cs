using System.Security.Cryptography;

namespace AkaShi.Core.Security.SecurityProvider;

internal class DeterministicRandomGenerator : RandomNumberGenerator
{
    private readonly Random _r = new(0);

    public override void GetBytes(byte[] data)
    {
        _r.NextBytes(data);
    }

    public override void GetNonZeroBytes(byte[] data)
    {
        for (var i = 0; i < data.Length; i++)
            data[i] = (byte)_r.Next(1, 256);
    }
}