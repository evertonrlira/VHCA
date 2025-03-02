﻿using CACrypto.Commons;
using CACrypto.RNGValidators.Commons;
using System.Collections.Concurrent;

namespace CACrypto.RNGValidators.Avalanche;

public static class KeyAvalanche
{
    public static void Run(PermutiveCACryptoMethodBase cryptoMethod, TestOptions options, int? testRepetitions = null)
    {
        testRepetitions ??= SampleSize.TenMegaBytes / SampleSize.DefaultBlockSize;

        var experimentTitle = string.Format("METHOD {0} - KEY AVALANCHE EXPERIMENT - {1} REPETITIONS", cryptoMethod.GetMethodName(), testRepetitions);
        if (options.writeConsole)
        {
            Console.WriteLine(experimentTitle);
        }

        var plaintextSet = Util.GetSecureRandomByteArrays(SampleSize.DefaultBlockSize, testRepetitions.Value);

        var disturbanceSet = new ConcurrentBag<byte[]>();
        Parallel.ForEach(plaintextSet, plaintext =>
        {
            var key = cryptoMethod.GenerateRandomGenericKey(SampleSize.DefaultBlockSize);
            var ciphertext = cryptoMethod.EncryptAsSingleBlock(plaintext, key);
            key.ChangeRandomBit();
            var disturbedCiphertext = cryptoMethod.EncryptAsSingleBlock(plaintext, key);
            var disturbance = Util.XOR(ciphertext, disturbedCiphertext);
            disturbanceSet.Add(disturbance);
        });

        Util.DisplayMetricsForDisturbanceSet(disturbanceSet, options.writeConsole);
    }
}
