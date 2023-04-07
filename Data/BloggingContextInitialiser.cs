using Bogus;

using DotnetEfCompiledQueries.Data.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DotnetEfCompiledQueries.Data;

public class BloggingContextInitialiser
{
    private readonly BloggingContext _db;
    private readonly DataConfig _config = new()
    {
        Blogs = 10,
        PostsPerBlog = 50,
        EnableMigrations = true
    };

    public BloggingContextInitialiser(BloggingContext context)
    {
        _db = context;
    }

    public async Task Run()
    {
        await InitialiseAsync();
        await SeedAsync();
    }

    private async Task InitialiseAsync()
    {
        if (!_db.Database.IsSqlServer())
            return;

        var isMigrationsEnabled = _config.EnableMigrations;
        if (isMigrationsEnabled)
        {
            await _db.Database.MigrateAsync();
        }
        else
        {
            await _db.Database.EnsureDeletedAsync();
            await _db.Database.EnsureCreatedAsync();
        }
    }

    private async Task SeedAsync()
    {
        // Default data
        // Seed, if necessary
        if (!_db.Blogs.Any())
        {
            var tagNames = new string[] { "Architecture", "Powershell", "Bicep", "Blazor", "Web", "Azure", "Console", ".NET", "JavaScript" };
            var tags = tagNames.Select(x => new Tag { Name = x.ToLower() }).ToList();

            var postFaker = new Faker<Post>()
                .RuleFor(p => p.Title, f => f.Lorem.Sentence())
                .RuleFor(p => p.Content, f => f.Lorem.Paragraphs(3))
                .RuleFor(p => p.Tags, f => f.Random.ListItems(tags, 2));

            var blogFaker = new Faker<Blog>()
                .RuleFor(b => b.Url, f => f.Internet.Url())
                .RuleFor(b => b.Posts, f => postFaker.Generate(_config.PostsPerBlog));

            _db.Tags.AddRange(tags);
            _db.Blogs.AddRange(blogFaker.Generate(_config.Blogs));
            await _db.SaveChangesAsync();
        }
    }
}