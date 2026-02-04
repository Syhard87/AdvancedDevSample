using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedDevSample.Domain.Exception
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { 
        }
    }
}
