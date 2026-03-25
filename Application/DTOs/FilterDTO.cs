using Domain.Entities;
using Domain.Enums;
using System.Linq;
using System.Linq.Expressions;

namespace Application.DTOs;

public class FilterDTO
{
    public string? DestFirstName { get; set; } = null;
    public string? DestLastName { get; set; } = null;
    public OperationType? OperationType { get; set; } = null;
    public decimal? MinAmount { get; set; } = null;
    public decimal? MaxAmount { get; set; } = null;
    public string? Description { get; set; } = null; // if desc will contain something

    public string? OrderCol {  get; set; } = null;
    public string? OrderDir { get; set; } = null;

    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    private Expression<Func<Transaction, bool>> MinAmountExpression()
    {
        return t => t.Amount >= MinAmount;
    }

    private Expression<Func<Transaction, bool>> MaxAmountExpression()
    {
        return t => t.Amount <= MaxAmount;
    }

    private Expression<Func<Transaction, bool>> FirstNameExpression()
    {
        return t => t.DestinationAccount!.FirstName.Contains(DestFirstName);
    }

    private Expression<Func<Transaction, bool>> LastNameExpression()
    {
        return t => t.DestinationAccount!.LastName.Contains(DestLastName);
    }

    private Expression<Func<Transaction, bool>> DescriptionExpression()
    {
        return t => t.Description!.Contains(Description);
    }

    private Expression<Func<Transaction, bool>> OperationExpression()
    {
        return t => t.OperationType == OperationType;
    }

    private Expression<Func<Transaction, OperationType>> OrderByOperationExpression()
    {
        return t => t.OperationType;
    }

    private Expression<Func<Transaction, decimal>> OrderByAmountExpression()
    {
        return t => t.Amount;
    }

    private Expression<Func<Transaction, Guid>> OrderByIdExpression()
    {
        return t => t.Id;
    }

    private Expression<Func<Transaction, string>> OrderByFirstNameExpression()
    {
        return t => t.DestinationAccount!.FirstName;
    }

    private Expression<Func<Transaction, string>> OrderByLastNameExpression()
    {
        return t => t.DestinationAccount!.LastName;
    }

    private IQueryable<Transaction> GetDirection<T>(IQueryable<Transaction> query, Expression<Func<Transaction, T>> expression)
    {
        if (OrderDir == "asc")
            query = query.OrderBy(expression);
        else
            query = query.OrderByDescending(expression);

        return query;
    }

    private IQueryable<Transaction> GetOrdered(IQueryable<Transaction> query)
    {
        switch (OrderCol)
        {
            case "id":
                query = GetDirection(query, OrderByIdExpression());
                break;

            case "operation":
                query = GetDirection(query, OrderByOperationExpression());
                break;

            case "amount":
                query = GetDirection(query, OrderByAmountExpression());
                break;

            case "firstname":
                query = GetDirection(query, OrderByFirstNameExpression());
                break;

            case "lastname":
                query = GetDirection(query, OrderByLastNameExpression());
                break;
        }

        return query;
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
        
        if (OrderCol != null && OrderDir != null)
            query = GetOrdered(query);

        return query;
    }
}
