using Dapper;
using FirstProjectTest.IRepository;
using FirstProjectTest.Models;
using System.Data;
using System.Data.Common;
using static Dapper.SqlMapper;

namespace FirstProjectTest.Repository
{
    public class WalletRepository : IWalletRepository
    {

        private readonly IDbConnection _dbConnection;

        public WalletRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public int Create(Wallet wallet)
        {
            const string sql = "INSERT INTO Wallet (UserId, CurrentBalance) VALUES (@UserId, @CurrentBalance); SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return _dbConnection.ExecuteScalar<int>(sql, wallet);
        }

        public bool Delete(int walletId)
        {
            const string sql = "DELETE FROM Wallet WHERE WalletId = @WalletId";

            var affectedRows = _dbConnection.Execute(sql, new { WalletId = walletId });

            return affectedRows > 0;
        }

        public Wallet GetById(int walletId)
        {
            const string sql = "SELECT * FROM Wallet WHERE WalletId = @WalletId";

            return _dbConnection.QueryFirstOrDefault<Wallet>(sql, new { WalletId = walletId });

        }

        public Wallet GetWalletByUserId(string userId)
        {
            const string sql = "SELECT * FROM Wallet WHERE UserId = @UserId";

            return _dbConnection.QueryFirstOrDefault<Wallet>(sql, new { UserId = userId });

        }

        public void MakeDeposit(string userId, decimal amount)
        {
            // Call the stored procedure to handle the deposit operation
            _dbConnection.Execute("MakeDeposit", new { UserId = userId, Amount = amount }, commandType: CommandType.StoredProcedure);

        }

        public bool Update(Wallet wallet)
        {
            const string sql = "UPDATE Wallet SET CurrentBalance = @CurrentBalance WHERE WalletId = @WalletId";

            var affectedRows = _dbConnection.Execute(sql, wallet);

            return affectedRows > 0;
        }
        public void UpdateWalletBalance(string userId, decimal amount)
        {
            // Call the stored procedure to update wallet balance
            _dbConnection.Execute("UpdateWalletBalance", new { UserId = userId, Amount = amount }, commandType: CommandType.StoredProcedure);
        }

        public void Withdraw(string userId, decimal amount)
        {
            var parameters = new { UserId = userId, Amount = amount };
            _dbConnection.Execute("WithdrawProcedure", parameters, commandType: CommandType.StoredProcedure);

        }
    }

        

        
    
}
