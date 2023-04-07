# dotnet-ef-compiled-queries

Runs performance tests on Entity Framework compiled queries using Benchmark.NET

## Usage

```bash
dotnet run -c Release
```

## Results

Local Machine running SQL Server LocalDB:

```bash
BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1485/22H2/2022Update/SunValley2)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK=7.0.202
```

|                           Method |            Mean |         Error |        StdDev |          Median |   Gen0 | Allocated |
|--------------------------------- |----------------:|--------------:|--------------:|----------------:|-------:|----------:|
|                 First_Or_Default |   129,028.38 ns |  2,659.869 ns |  7,545.606 ns |   126,976.73 ns | 0.4883 |    7368 B |
|  First_Or_Default_Compiled_Query |   101,715.65 ns |  2,019.186 ns |  5,527.490 ns |   100,077.26 ns | 0.3662 |    4976 B |
|                Single_Or_Default |   121,310.89 ns |  2,383.251 ns |  4,868.349 ns |   120,243.33 ns | 0.4883 |    7368 B |
| Single_Or_Default_Compiled_Query |    99,554.67 ns |  1,867.989 ns |  1,747.318 ns |    99,854.26 ns | 0.3662 |    4976 B |
|                          Get_All | 1,150,583.72 ns | 24,711.148 ns | 66,808.025 ns | 1,136,052.93 ns | 9.7656 |  137306 B |
|           Get_All_Compiled_Query |        65.21 ns |      1.234 ns |      1.884 ns |        65.09 ns | 0.0114 |     144 B |