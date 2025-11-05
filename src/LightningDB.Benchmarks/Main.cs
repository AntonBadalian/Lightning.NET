using System;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Running;

namespace LightningDB.Benchmarks;

public static class Entry
{
    public static void Main(string[] args)
    {
        Console.WriteLine(RuntimeInformation.FrameworkDescription);
        //BenchmarkRunner.Run<WriteBenchmarks>();
        BenchmarkRunner.Run<ReadBenchmarks>();
    }
}