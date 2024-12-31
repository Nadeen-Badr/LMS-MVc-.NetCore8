using Microsoft.AspNetCore.Mvc;

public class DashboardController : Controller
{
    private readonly IBookRepository _bookRepository;

    public DashboardController(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    // Make the action async
    public async Task<IActionResult> Index()
    {
        // Use async methods to fetch data
        var statusData = await _bookRepository.GetBooksGroupedByStatusAsync();
        var revenueData = await _bookRepository.GetRevenueByStatusAsync();

        // Pass the data to the View
        ViewData["StatusData"] = statusData;
        ViewData["RevenueData"] = revenueData;

        return View();
    }
}
