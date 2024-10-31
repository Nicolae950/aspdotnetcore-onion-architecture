using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels;

public class PaginatedVM
{
    public IEnumerable<Transaction> Transactions { get; set; }
    public int TotalTransactions { get; set; }

    public PaginatedVM(IEnumerable<Transaction> transactions, int totalTransactions)
    {
        Transactions = transactions;
        TotalTransactions = totalTransactions;
    }
}
