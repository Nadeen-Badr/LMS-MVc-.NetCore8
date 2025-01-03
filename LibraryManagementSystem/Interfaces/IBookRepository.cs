using LibraryManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task<Book> GetBookByIdAsync(int id);
    Task AddBookAsync(Book book);
    Task UpdateBookAsync(Book book);
    Task DeleteBookAsync(int id);
    Task<bool> BookExists(int id);
     Task<Book> GetBookByIdAsyncAsNOTRacking(int id);
     Task<IEnumerable<Book>> GetBooksBySearchAsync(string? searchQuery);
     Task<IEnumerable<Book>> GetFilteredBooksAsync(int? categoryId, string status);
     Task<List<StatusGroup>> GetBooksGroupedByStatusAsync();
     Task<List<RevenueGroup>> GetRevenueByStatusAsync();
     Task<IEnumerable<Book>> GetPagedBooksAsync(int page, int pageSize);
     Task<int> GetAllBooksCountAsync();
     Task<int> GetFilteredBooksCountAsync(int? categoryId, string status);
     Task<IEnumerable<Book>> GetFilteredBooksAsync(int? categoryId, string status, int page, int pageSize);
     Task<IEnumerable<Book>> GetBooksBySearchAndFiltersAsync(string searchQuery, int? categoryId, string status, int page, int pageSize);
     
}
