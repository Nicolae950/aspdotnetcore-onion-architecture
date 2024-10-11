using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class WaitingTransferException : Exception
    {
        public WaitingTransferException() : base("This transaction doesn't have (anymore) \" waiting \" status.")
        { }
    }
}
