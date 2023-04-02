using System;

namespace StaticBlogr
{
    public class BlogPostDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ImageUrl { get; set; }

        public BlogPostDTO() { }

        public BlogPostDTO(BlogPost blogPost)
        {
            Id = blogPost.Id;
            Title = blogPost.Title;
            Content = blogPost.Content;
            CreatedAt = blogPost.CreatedAt;
            ImageUrl = blogPost.ImageUrl;
        }
    }
}
