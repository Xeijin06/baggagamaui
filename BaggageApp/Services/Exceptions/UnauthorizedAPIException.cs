using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Services.Exceptions
{
    public class UnauthorizedAPIException : Exception
    {
        public UnauthorizedAPIException(string message) : base(message)
        {

        }
    }
}
