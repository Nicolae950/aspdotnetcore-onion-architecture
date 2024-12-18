﻿using Domain.Enums;

namespace WebApp.Models.Transaction;

public class TransactionViewModel
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public OperationType OperationType { get; set; }
    public StateOfTransaction StateOfTransaction { get; set; }
    public Guid? DestinationAccountId { get; set; }
    public string? DestinationFirstName { get; set; }
    public string? DestinationLastName { get; set; }

}
