using Domain.Entities;
using Domain.Enums;
using System.Linq.Expressions;

namespace Application.DTOs;

public class FilterDTO
{
    public string? DestFirstName { get; set; }
    public string? DestLastName { get; set; }
    public OperationType? OperationType { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public string? Description { get; set; } // if desc will contain something

    public Expression<Func<Transaction, bool>> MinAmountExpression()
    {
        return t => t.Amount >= MinAmount;
    }

    public Expression<Func<Transaction, bool>> MaxAmountExpression()
    {
        return t => t.Amount <= MaxAmount;
    }

    public Expression<Func<Transaction, bool>> FirstNameExpression()
    {
        return t => t.DestinationAccount.FirstName.Contains(DestFirstName);
    }

    public Expression<Func<Transaction, bool>> LastNameExpression()
    {
        return t => t.DestinationAccount.LastName.Contains(DestLastName);
    }

    public Expression<Func<Transaction, bool>> DescriptionExpression()
    {
        return t => t.Description.Contains(Description);
    }

    public Expression<Func<Transaction, bool>> OperationExpression()
    {
        return t => t.OperationType == OperationType;
    }

    public IQueryable<Transaction> GetFilter(IQueryable<Transaction> query)
    {
        if (DestFirstName != null)
            query = query.Where(FirstNameExpression());
        if (DestLastName != null)
            query = query.Where(LastNameExpression());
        if (MinAmount != null)
            query = query.Where(MinAmountExpression());
        if (MaxAmount != null)
            query = query.Where(MaxAmountExpression());
        if (!string.IsNullOrWhiteSpace(Description))
            query = query.Where(DescriptionExpression());
        if (OperationType != null)
            query = query.Where(OperationExpression());
        
        return query;
    }
}
