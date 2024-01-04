using FirstProjectTest.Repo.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstProjectTest.Controllers
{
    public class WithdrawController : Controller
    {
        private readonly IWalletService _walletService;

        public WithdrawController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public IActionResult Withdraw()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MakeWithdrawal(decimal amount)
        {
            // Get the user ID from the currently logged-in user
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                // Handle the case where user ID is not available (e.g., user not authenticated)
                return Json(new { success = false, message = "User ID not available." });
            }

            // Make the withdrawal
            var result = _walletService.MakeWithdrawal(userId, amount);

            // Check the result and return an appropriate response
            if (result.Success)
            {
                return Json(new { success = true, message = "Withdrawal successful." });
            }
            else
            {
                return Json(new { success = false, message = result.Message });
            }
        }
    }
}
