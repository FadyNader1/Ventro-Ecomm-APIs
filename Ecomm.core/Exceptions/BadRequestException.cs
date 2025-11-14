using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Exceptions
{
    public class BadRequestException : Exception
    {

        public BadRequestException(string Message) : base(Message)
        {

        }

    }
}