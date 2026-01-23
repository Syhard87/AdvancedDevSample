using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedDevSample.Domain.Execptions
{
    public class DomainExeception : Exception
    {
        public DomainExeception(string message) : base(message) { 
        }
    }
}
