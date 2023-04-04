using Microsoft.EntityFrameworkCore;
using StaticBlogr;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BlogPostDb>(opt => opt.UseInMemoryDatabase("BlogPosts"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy",
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:3000")
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
});
var app = builder.Build();

RouteGroupBuilder blogPosts = app.MapGroup("/blogposts");

blogPosts.MapGet("/", (Func<BlogPostDb, bool?, Task<IResult>>)GetAllBlogPosts);
blogPosts.MapGet("/{id}", GetBlogPost);
blogPosts.MapPost("/", CreateBlogPost);
blogPosts.MapPut("/{id}", UpdateBlogPost);
blogPosts.MapDelete("/{id}", DeleteBlogPost);

app.UseCors("MyPolicy");

app.Run();


static async Task<IResult> GetAllBlogPosts(BlogPostDb db, bool? featured = null)
{
    IQueryable<BlogPost> query = db.BlogPosts;

    if (featured.HasValue)
    {
        query = query.Where(x => x.IsFeatured == featured.Value);
    }

    return TypedResults.Ok(await query.Select(x => new BlogPostDTO(x)).ToArrayAsync());
}


static async Task<IResult> GetBlogPost(int id, BlogPostDb db)
{
    return await db.BlogPosts.FindAsync(id)
        is BlogPost blogPost
            ? TypedResults.Ok(new BlogPostDTO(blogPost))
            : TypedResults.NotFound();
}

static async Task<IResult> CreateBlogPost(BlogPostDTO blogPostDTO, BlogPostDb db)
{
    var blogPost = new BlogPost
    {
        Title = blogPostDTO.Title,
        Content = blogPostDTO.Content,
        CreatedAt = DateTime.Now
    };

    db.BlogPosts.Add(blogPost);
    await db.SaveChangesAsync();

    blogPostDTO = new BlogPostDTO(blogPost);

    return TypedResults.Created($"/blogposts/{blogPost.Id}", blogPostDTO);
}

static async Task<IResult> UpdateBlogPost(int id, BlogPostDTO blogPostDTO, BlogPostDb db)
{
    var blogPost = await db.BlogPosts.FindAsync(id);

    if (blogPost is null) return TypedResults.NotFound();

    blogPost.Title = blogPostDTO.Title;
    blogPost.Content = blogPostDTO.Content;
    blogPost.IsFeatured = blogPostDTO.IsFeatured; // Add this line to update the IsFeatured property

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}


static async Task<IResult> DeleteBlogPost(int id, BlogPostDb db)
{
    if (await db.BlogPosts.FindAsync(id) is BlogPost blogPost)
    {
        db.BlogPosts.Remove(blogPost);
        await db.SaveChangesAsync();
        return TypedResults.Ok(new BlogPostDTO(blogPost));
    }

    return TypedResults.NotFound();
}
