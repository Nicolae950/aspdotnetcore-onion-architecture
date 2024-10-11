using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class AccountHelper
    {
        public async Task DeleteOwnTransactions(Guid id, ITransactionRepository transactionRepository)
        {
            var transactions = await transactionRepository.GetAllTransactionsForAccount(id);
            foreach (var transaction in transactions)
            {
                await transactionRepository.ExecuteUpdate(id, transaction,
                    transaction.GetEligibleForDeleteExpression(id),
                    x => x.SetProperty(t => t.IsDeleted, true));
            }
        }
    }
}
