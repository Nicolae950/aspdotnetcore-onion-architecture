using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.DTOs;

public class AccountDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public Account MapDTOToAccount(Guid userId)
    {
        return new Account(FirstName, LastName, userId);
    }
    public Account MapDTOToAccountWithId(Guid id)
    {
        return new Account(id, FirstName, LastName);
    }
}
