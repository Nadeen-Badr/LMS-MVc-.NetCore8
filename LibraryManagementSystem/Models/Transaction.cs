namespace LibraryManagementSystem.Models
{
    public class Transaction
    {
        public int Id { get; set; } // Primary Key

        // Foreign Keys
        public int BookId { get; set; }
        public Book Book { get; set; } // Navigation Property

        public int MemberId { get; set; }
        public Member Member { get; set; } // Navigation Property

        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; } // Nullable for pending returns
    }
}
