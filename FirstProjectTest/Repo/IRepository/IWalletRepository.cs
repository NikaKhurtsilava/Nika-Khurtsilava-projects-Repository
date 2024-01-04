using FirstProjectTest.Models;

namespace FirstProjectTest.IRepository
{
    public interface IWalletRepository: ICrudRepository<Wallet>
    {
        Wallet GetWalletByUserId(string userId);

        void MakeDeposit(string userId, decimal amount);
        void Withdraw(string userId, decimal amount);
    }
}
