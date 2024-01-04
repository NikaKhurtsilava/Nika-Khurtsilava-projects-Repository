using FirstProjectTest.Enums;
using FirstProjectTest.IRepository;
using FirstProjectTest.Models;
using FirstProjectTest.Repo.IRepository;
using FirstProjectTest.Repo.IServices;

namespace FirstProjectTest.Repo.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        public WalletService(IWalletRepository walletRepository, ITransactionRepository transactionRepository)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }

        public decimal GetCurrentBalanceByUserId(string userId)
        {
            var wallet = _walletRepository.GetWalletByUserId(userId);

            if (wallet != null)
            {
                return wallet.CurrentBalance;
            }

            // Return 0 if the wallet is not found
            return 0m;
        }


        public void CreateWalletByUserId(string userId)
        {
            var wallet = new Wallet { UserId = userId, CurrentBalance = 0m };
            _walletRepository.Create(wallet); 
        }

        public TransactionResult MakeDeposit(string userId, decimal amount)
        {
            // Retrieve user's wallet
            var wallet = _walletRepository.GetWalletByUserId(userId);

            // Check if wallet exists
            if (wallet == null)
            {
                // Create wallet if it doesn't exist
                CreateWalletByUserId(userId);
                wallet = _walletRepository.GetWalletByUserId(userId);
            }

            // Call the method in WalletRepository to make the deposit
            _walletRepository.MakeDeposit(userId, amount);

            // Log the transaction
            var transaction = new Transaction
            {
                UserId = userId,
                Amount = amount,
                TransactionType = TransactionType.Deposit.ToString(),
                TransactionStatus = (int)TransactionStatus.Success,
                CurrentBalance = wallet.CurrentBalance,
                TransactionDate = DateTime.Now
            };

            _transactionRepository.Create(transaction);

            return new TransactionResult { Success = true, Message = "Deposit successful." };

        }

        public TransactionResult MakeWithdrawal(string userId, decimal amount)
        {
            // Retrieve user's wallet
            var wallet = _walletRepository.GetWalletByUserId(userId);

            // Check if wallet exists
            if (wallet == null)
            {
                // Create wallet if it doesn't exist
                CreateWalletByUserId(userId);
                wallet = _walletRepository.GetWalletByUserId(userId);
            }

            // Check if sufficient balance
            if (wallet.CurrentBalance < amount)
            {
                return new TransactionResult { Success = false, Message = "Insufficient balance." };
            }

            _walletRepository.Withdraw(userId, amount);

            // Log the transaction
            var transaction = new Transaction
            {
                UserId = userId,
                Amount = amount,
                TransactionType = TransactionType.Withdraw.ToString(),
                TransactionStatus = (int)TransactionStatus.Success,
                CurrentBalance = wallet.CurrentBalance,
                TransactionDate = DateTime.Now
            };

            _transactionRepository.Create(transaction);

            return new TransactionResult { Success = true, Message = "Withdrawal successful." };

        }
    }
}
