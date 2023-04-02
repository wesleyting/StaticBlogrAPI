namespace StaticBlogr
{
    using Microsoft.EntityFrameworkCore;

    class BlogPostDb : DbContext
    {
        public BlogPostDb(DbContextOptions<BlogPostDb> options)
            : base(options) { }

        public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    }
}
