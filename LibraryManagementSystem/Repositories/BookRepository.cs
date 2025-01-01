using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BookRepository : IBookRepository
{
    private readonly LibraryContext _context;

    public BookRepository(LibraryContext context)
    {
        _context = context;
    }

 public async Task<IEnumerable<Book>> GetAllBooksAsync()
{
    // Eager load the Category with the Books
    return await _context.Books
                         .Include(b => b.Category)
                         .ToListAsync();
}
public async Task<IEnumerable<Book>> GetBooksBySearchAsync(string? searchQuery)
{
    // Eager load the Category with the Books
    var query = _context.Books.Include(b => b.Category).AsQueryable();

    if (!string.IsNullOrEmpty(searchQuery))
    {
        query = query.Where(b => b.Title.Contains(searchQuery)); // Search by Title
    }

    return await query.ToListAsync();
}

   public async Task<Book> GetBookByIdAsync(int id)
{
    return await _context.Books
                         .Include(b => b.Category)  // Eagerly load the Category
                         .FirstOrDefaultAsync(b => b.Id == id);
}
   public async Task<Book> GetBookByIdAsyncAsNOTRacking(int id){
    return await _context.Books.Include(b => b.Category)
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(b => b.Id == id);

   }

public async Task AddBookAsync(Book book)

{
     book.TotalRentProfit ??= 0m;
    await _context.Books.AddAsync(book);
    await _context.SaveChangesAsync();
}
public async Task<IEnumerable<Book>> GetPagedBooksAsync(int page, int pageSize)
{
    return await _context.Books
        .Skip((page - 1) * pageSize) // Skip books from previous pages
        .Take(pageSize)             // Take books for the current page
        .ToListAsync();
}
public async Task<int> GetAllBooksCountAsync()
{
    return await _context.Books.CountAsync();
}

public async Task UpdateBookAsync(Book book)
{
    // Ensure that the status and category are correctly updated
    _context.Books.Update(book);
    await _context.SaveChangesAsync();
    }
    public async Task<bool> BookExists(int id)
    {
        return await _context.Books.AnyAsync(b => b.Id == id);
    }

    public async Task DeleteBookAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<IEnumerable<Book>> GetFilteredBooksAsync(int? categoryId, string status)
{
    var query = _context.Books.Include(b => b.Category).AsQueryable();

    if (categoryId.HasValue)
    {
        query = query.Where(b => b.CategoryId == categoryId.Value);
    }

    if (!string.IsNullOrEmpty(status))
    {
        query = query.Where(b => b.Status == status);
    }

 
    return await query.ToListAsync();
}
public async Task<List<StatusGroup>> GetBooksGroupedByStatusAsync()
    {
        return await _context.Books
            .GroupBy(b => b.Status)
            .Select(g => new StatusGroup { Status = g.Key, Count = g.Count() })
            .ToListAsync();
    }

    // Async method to get the revenue from rented and sold books
    public async Task<List<RevenueGroup>> GetRevenueByStatusAsync()
    {
        return await _context.Books
            .Where(b => b.Status == "Rented" || b.Status == "Sold")
            .GroupBy(b => b.Status)
            .Select(g => new RevenueGroup
            {
                Status = g.Key,
                Revenue = g.Key == "Rented"
                ? g.Sum(b => b.TotalRentProfit ?? 0) // Sum up TotalRentProfit for Rented
                : g.Sum(b => b.SellPrice ?? 0) 

            })
            .ToListAsync();
    }
    public async Task<IEnumerable<Book>> GetFilteredBooksAsync(int? categoryId, string status, int page, int pageSize)
{
    var query = _context.Books.Include(b => b.Category).AsQueryable();

    if (categoryId.HasValue)
    {
        query = query.Where(b => b.CategoryId == categoryId.Value);
    }

    if (!string.IsNullOrEmpty(status))
    {
        query = query.Where(b => b.Status == status);
    }

    return await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
}

public async Task<int> GetFilteredBooksCountAsync(int? categoryId, string status)
{
    var query = _context.Books.AsQueryable();

    if (categoryId.HasValue)
    {
        query = query.Where(b => b.CategoryId == categoryId.Value);
    }

    if (!string.IsNullOrEmpty(status))
    {
        query = query.Where(b => b.Status == status);
    }

    return await query.CountAsync();
}
public async Task<IEnumerable<Book>> GetBooksBySearchAndFiltersAsync(string searchQuery, int? categoryId, string status, int page, int pageSize)
{
    var query = _context.Books.Include(b => b.Category).AsQueryable();

    // Apply filters
    if (categoryId.HasValue)
    {
        query = query.Where(b => b.CategoryId == categoryId.Value);
    }

    if (!string.IsNullOrEmpty(status))
    {
        query = query.Where(b => b.Status == status);
    }

    // Apply search query
    if (!string.IsNullOrEmpty(searchQuery))
    {
        query = query.Where(b => b.Title.Contains(searchQuery) );
    }

    // Apply pagination
    query = query.Skip((page - 1) * pageSize).Take(pageSize);

    return await query.ToListAsync();
}


}


