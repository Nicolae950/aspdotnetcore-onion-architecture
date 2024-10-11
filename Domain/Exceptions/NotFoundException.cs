using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions;

public class NotFoundException<T> : Exception where T : class
{
    public NotFoundException() : base($" {typeof(T).Name} not found!")
    {}
}

