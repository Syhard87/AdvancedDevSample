using System;
using System.Collections.Generic;
using System.Text;
using AdvancedDevSample.Domain.Entities;
namespace AdvancedDevSample.Domain.Interfaces
{
    public interface IproductRepository
    {
        public void Save(Product product);

        public Product GetById(Guid id);
    }
}
