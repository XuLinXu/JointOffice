using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JointOffice.Core
{
    public class BusinessTureException : Exception
    {
        public BusinessTureException(string message) : base(message)
        {
        }
        public BusinessTureException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
