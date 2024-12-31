using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        public int Id { get; set; } // Primary Key
        public string Title { get; set; }
        public string?Author { get; set; }
        public string? ISBN { get; set; }
        public int? PublishedYear { get; set; }
        public int? CopiesAvailable { get; set; }
        public string? ImagePath { get; set; } // Path to the image file

        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? Status { get; set; } // "Available", "Rented", or "Sold"

        [Column(TypeName = "decimal(18,2)")]
        public decimal? RentPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SellPrice { get; set; }
         public int? RentalDuration { get; set; } // Duration in days
    public DateTime? RentalDate { get; set; } // Date when the book was rented
    public DateTime? DueDate { get; set; } // Due date for return

     [DefaultValue(0)]
     public decimal? TotalRentProfit { get; set; } 
    }
}
