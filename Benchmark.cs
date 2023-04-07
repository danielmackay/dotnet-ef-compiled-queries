using BenchmarkDotNet.Attributes;

using DotnetEfCompiledQueries.Data;
using DotnetEfCompiledQueries.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace DotnetEfCompiledQueries;

[MemoryDiagnoser]
public class Benchmark
{
    private BloggingContext _dbContext = default!;
    private const int _postId = 1;

    private static readonly Func<BloggingContext, int, Post?> GetFirstOrDefault =
        EF.CompileQuery((BloggingContext dbContext, int id) =>
                dbContext.Set<Post>().FirstOrDefault(p => p.PostId == id));

    private static readonly Func<BloggingContext, int, Post?> GetSingleOrDefault =
        EF.CompileQuery((BloggingContext dbContext, int id) =>
                dbContext.Set<Post>().SingleOrDefault(p => p.PostId == id));

    private static readonly Func<BloggingContext, List<Post>> GetAll =
        EF.CompileQuery((BloggingContext dbContext) =>
                dbContext.Set<Post>().ToList());

    [GlobalSetup]
    public async Task Setup()
    {
        _dbContext = new BloggingContext();
        var initializer = new BloggingContextInitialiser(_dbContext);
        await initializer.Run();
    }

    [Benchmark]
    public void First_Or_Default()
    {
        _dbContext.Posts.FirstOrDefault(p => p.PostId == _postId);
    }

    [Benchmark]
    public void First_Or_Default_Compiled_Query()
    {
        GetFirstOrDefault(_dbContext, _postId);
    }

    [Benchmark]
    public void Single_Or_Default()
    {
        _dbContext.Posts.SingleOrDefault(p => p.PostId == _postId);
    }

    [Benchmark]
    public void Single_Or_Default_Compiled_Query()
    {
        GetSingleOrDefault(_dbContext, _postId);
    }

    [Benchmark]
    public void Get_All()
    {
        _dbContext.Posts.ToList();
    }

    [Benchmark]
    public void Get_All_Compiled_Query()
    {
        GetAll(_dbContext);
    }
}