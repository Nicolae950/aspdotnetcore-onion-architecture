using Domain.Entities;

namespace WebAPI.ViewModels
{
    public class AccountTransactionsVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Balance { get; set; }
        public IEnumerable<TransactionVM>? Transactions { get; set; }

        public AccountTransactionsVM(Account account, IEnumerable<Transaction>? transactions)
        {
            FirstName = account.FirstName;
            LastName = account.LastName;
            Balance = account.Balance;
            Transactions = transactions?.Select(x => new TransactionVM(x));
        }
    }
}
