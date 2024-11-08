﻿using Domain.Entities;
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

    public string? OrderBy {  get; set; } = null;
    public string? OrderByDescending { get; set; } = null;

    public int PageNumber { get; set; }
    public int PageSize { get; set; }

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

    public Expression<Func<Transaction, OperationType>> OrderByOperationExpression()
    {
        return t => t.OperationType;
    }

    public Expression<Func<Transaction, decimal>> OrderByAmountExpression()
    {
        return t => t.Amount;
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
        
        
        if (OrderBy == "operation")
            query = query.OrderBy(OrderByOperationExpression());
        else
        {
            if (OrderBy == "amount")
                query = query.OrderBy(OrderByAmountExpression());
            else
                query = query.OrderByDescending(t => t.CreatedAt);
        }

        if (OrderByDescending != null)
        {
            if (OrderByDescending == "operation")
                query = query.OrderByDescending(OrderByOperationExpression());

            if (OrderByDescending == "amount")
                query = query.OrderByDescending(OrderByAmountExpression());
        }

        return query;
    }
}
