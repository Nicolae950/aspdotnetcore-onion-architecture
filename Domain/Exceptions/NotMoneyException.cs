using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions;

public class NotMoneyException : Exception
{
    public NotMoneyException():base("Not enough money on account.") 
    { }
}
