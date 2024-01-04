using FirstProjectTest.Repo.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstProjectTest.Controllers
{
    public class DepositController : Controller
    {
        private readonly IWalletService _walletService;

        public DepositController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public IActionResult Deposit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MakeDeposit(decimal amount)
        {
            Console.WriteLine("START CONTROLLER DEPOSIT METHOD");
            // Get the user ID from the currently logged-in user
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                // Handle the case where user ID is not available (e.g., user not authenticated)
                return Json(new { success = false, message = "User ID not available." });
            }

            // Make the deposit
            var result = _walletService.MakeDeposit(userId, amount);
            Console.WriteLine("after deposit");
            // Check the result and return an appropriate response
            if (result.Success)
            {
                return Json(new { success = true, message = "Deposit successful." });
            }
            else
            {
                return Json(new { success = false, message = result.Message });
            }
            
        }
    }
}
