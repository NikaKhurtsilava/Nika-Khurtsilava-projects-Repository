using Microsoft.AspNetCore.Mvc;
using FirstProjectTest.Repo.IServices;
using System.Security.Claims;

[Controller]
public class WalletController : Controller
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet]
    public IActionResult GetCurrentBalance()
    {
        Console.WriteLine("GetCurrentBalance action method called.");
        // Get the user's ID (replace this with your actual way of getting the user ID)
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            // Handle the case where user ID is not available (e.g., user not authenticated)
            return Json(new { success = false, message = "User ID not available." });
        }

        // Get the current balance from the service
        decimal currentBalance = _walletService.GetCurrentBalanceByUserId(userId);

        // Return the balance as JSON
        return Json(new { currentBalance });
    }

    
}
