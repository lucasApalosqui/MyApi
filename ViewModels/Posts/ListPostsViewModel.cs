﻿namespace BlogAspNet.ViewModels.Posts
{
    public class ListPostsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string LastUpdateDate { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
    }
}
