using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class BookController : Controller
{
    private readonly IBookRepository _bookRepository;
    private readonly ICategoryRepository _categoryRepository;


   public BookController(IBookRepository bookRepository, ICategoryRepository categoryRepository)
    {
        _bookRepository = bookRepository;
        _categoryRepository = categoryRepository;
    }

    // GET: Book
public async Task<IActionResult> Index(string? searchQuery)
{
    IEnumerable<Book> books;
    var categories = await _categoryRepository.GetAllCategoriesAsync();
    
    // Pass categories directly to the view using ViewData
    ViewData["Categories"] = categories;

    if (!string.IsNullOrEmpty(searchQuery))
    {
        books = await _bookRepository.GetBooksBySearchAsync(searchQuery);
        ViewData["CurrentSearch"] = searchQuery; // To retain the search term in the input field
    }
    else
    {
        books = await _bookRepository.GetAllBooksAsync();
    }

    return View(books);
}




    // GET: Book/Create
public async Task<IActionResult> Create()
{
    // Get categories from the repository
    var categories = await _categoryRepository.GetAllCategoriesAsync();
    
    // Pass categories directly to the view using ViewData
    ViewData["Categories"] = categories;

    return View();
}


[HttpPost]
[ValidateAntiForgeryToken]

public async Task<IActionResult> Create(Book book, IFormFile? image)
{
    if (ModelState.IsValid)
    {
        // Handle the image upload
        if (image != null && image.Length > 0)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", image.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            book.ImagePath = "/images/" + image.FileName;
        }

        // Add the book to the database
        await _bookRepository.AddBookAsync(book);

        // Redirect to the Index page
        return RedirectToAction(nameof(Index));
    }

    // If the model is invalid, return to the form with errors
    return View(book);
}




    // GET: Book/Edit/5
   public async Task<IActionResult> Edit(int id)
{
    var book = await _bookRepository.GetBookByIdAsync(id);
    if (book == null)
    {
        return NotFound();
    }

    // Fetch all categories from the database
    var categories = await _categoryRepository.GetAllCategoriesAsync();

    // Pass the categories to the view
    ViewData["Categories"] = categories;

    return View(book);
}


    // POST: Book/Edit/5
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, Book book, IFormFile? image)
{
    if (id != book.Id)
    {
        return NotFound();
    }

    // Retrieve the current book from the database to preserve existing values
    //var existingBook = await _bookRepository.GetBookByIdAsync(id);
    var existingBook = await _bookRepository.GetBookByIdAsyncAsNOTRacking(id);
    if (existingBook == null)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            // Preserve the existing image if no new image is uploaded
            if (image != null && image.Length > 0)
            {
                // Handle the image upload
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                book.ImagePath = "/images/" + image.FileName; // Update image path with the new file
            }
            else
            {
                // If no new image is provided, keep the existing ImagePath
                book.ImagePath = existingBook.ImagePath;
            }

            // Update the book in the database
            await _bookRepository.UpdateBookAsync(book);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _bookRepository.BookExists(book.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(Index));
    }

    // If the model is invalid, return to the form with errors
    return View(book);
}


    // GET: Book/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _bookRepository.GetBookByIdAsync(id.Value);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // POST: Book/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _bookRepository.DeleteBookAsync(id);
        return RedirectToAction(nameof(Index));
    }
        // GET: Book/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _bookRepository.GetBookByIdAsync(id.Value);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
}   
    [HttpPost]
public async Task<IActionResult> Rent(int id, int rentalDuration)
{
    var book = await _bookRepository.GetBookByIdAsync(id);
    if (book == null)
    {
        return NotFound();
    }

    // Check if the book is already rented
    if (book.Status == "Rented")
    {
        ModelState.AddModelError("BookStatus", "This book is already rented.");
        return View("Details", book); // Return back to the details page with an error
    }

    // Validate the rental duration
    if (rentalDuration <= 0)
    {
        ModelState.AddModelError("RentalDuration", "Rental duration must be greater than zero.");
        return View("Details", book);
    }

    // Update the book status and rental details
    book.Status = "Rented";
    book.RentalDuration = rentalDuration;
    book.RentalDate = DateTime.Now;
    book.DueDate = DateTime.Now.AddDays(rentalDuration);

    // Save changes to the database
    await _bookRepository.UpdateBookAsync(book);

    // Display a confirmation message to the user
    TempData["SuccessMessage"] = $"You have successfully rented '{book.Title}' for {rentalDuration} days. Due Date: {book.DueDate.Value.ToShortDateString()}";

    return RedirectToAction(nameof(Details), new { id = book.Id });
}
[HttpPost]
public async Task<IActionResult> Return(int id)
{
    var book = await _bookRepository.GetBookByIdAsync(id);
    if (book == null)
    {
        return NotFound();
    }

    // Check if the book is rented
    if (book.Status != "Rented")
    {
        ModelState.AddModelError("ReturnError", "This book is not currently rented.");
        return View("Details", book);
    }

    // Update the book status and clear rental details
    book.Status = "Available";
    book.RentalDuration = null;
    book.RentalDate = null;
    book.DueDate = null;

    // Save changes to the database
    await _bookRepository.UpdateBookAsync(book);

    // Display a return confirmation message
    TempData["SuccessMessage"] = $"You have successfully returned '{book.Title}'.";

    return RedirectToAction(nameof(Details), new { id = book.Id });
}
[HttpPost]
public async Task<IActionResult> Sell(int id)
{
    var book = await _bookRepository.GetBookByIdAsync(id);
    if (book == null)
    {
        return NotFound();
    }

    // Mark the book as sold
    book.Status = "Sold";
    book.RentalDuration = null; // Clear the rent days since it's no longer available for rent
    
    // Update the book in the database
    await _bookRepository.UpdateBookAsync(book);

    return RedirectToAction(nameof(Details), new { id = book.Id });
}

}
