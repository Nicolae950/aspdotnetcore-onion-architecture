using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions;

public class SaveException : Exception
{
    public SaveException() : base("Some error has appeared during save operation!")
    { }
}
