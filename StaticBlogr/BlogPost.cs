﻿namespace StaticBlogr
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public Boolean IsFeatured { get; set; }
    }
}
