namespace LibrarySystem.DTOs
{
    public class BorrowGetDTO
    {
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public string BookIsbn { get; set; }
        public DateTime BookPublishYear { get; set; }
        public string UserFullName { get; set; } 
        public string UserEmail { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
