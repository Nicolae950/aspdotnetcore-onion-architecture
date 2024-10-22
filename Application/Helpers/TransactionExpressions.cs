using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers;

public static class TransactionExpressions
{
    public static IQueryable<Transaction> WhereMinAmount(this IQueryable<Transaction> source, FilterDTO filter)
    {
        return source.Where(x => x.Amount >= filter.MinAmount);
    }
    
    public static IQueryable<Transaction> WhereMaxAmount(this IQueryable<Transaction> source, FilterDTO filter)
    {
        return source.Where(x => x.Amount <= filter.MaxAmount);
    }

    public static IQueryable<Transaction> WhereDescripiton(this IQueryable<Transaction> source, FilterDTO filter)
    {
        return source.Where(x => x.Description.Contains(filter.Description));
    }

    public static IQueryable<Transaction> WhereFirstName(this IQueryable<Transaction> source, FilterDTO filter)
    {
        return source.Where(x => x.DestinationAccount.FirstName.Contains(filter.DestFirstName));
    }
     
    public static IQueryable<Transaction> WhereLastName(this IQueryable<Transaction> source, FilterDTO filter)
    {
        return source.Where(x => x.DestinationAccount.LastName.Contains(filter.DestLastName));
    }

    public static IQueryable<Transaction> WhereOperation(this IQueryable<Transaction> source, FilterDTO filter)
    {
        return source.Where(t => t.OperationType == filter.OperationType);
    }
}
