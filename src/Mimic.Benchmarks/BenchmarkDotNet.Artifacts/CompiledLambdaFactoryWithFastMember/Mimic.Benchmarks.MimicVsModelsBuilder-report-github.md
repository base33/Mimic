``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.778 (1909/November2018Update/19H2)
AMD A10-7800 Radeon R7, 12 Compute Cores 4C+8G, 1 CPU, 4 logical and 2 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4121.0), X86 LegacyJIT
  .NET 4.7.2 : .NET Framework 4.8 (4.8.4121.0), X86 LegacyJIT

Job=.NET 4.7.2  Runtime=.NET 4.7.2  

```
|          Method |     Mean |    Error |   StdDev | Ratio |
|---------------- |---------:|---------:|---------:|------:|
| MimicConversion | 20.66 ms | 0.222 ms | 0.197 ms |  1.00 |
