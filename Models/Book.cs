﻿namespace LibrarySystem.Models
{
    public partial class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public DateTime PublishYear { get; set; }
        public string Isbn { get; set; } = null!;
    }
}
