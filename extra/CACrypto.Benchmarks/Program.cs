﻿using BenchmarkDotNet.Running;

namespace CACrypto.Benchmarks;
public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<MethodsBenchmarker>();
    }
}
