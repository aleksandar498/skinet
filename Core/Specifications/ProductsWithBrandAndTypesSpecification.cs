using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithBrandAndTypesSpecification : BaseSpecification<Product>
    {
        public ProductsWithBrandAndTypesSpecification(string sort)
        {
            AddInclude(x => x.ProducteType);
            AddInclude(x => x.ProductBrand);
            AddOrderBy(x => x.Name);
        }

        public ProductsWithBrandAndTypesSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProducteType);
            AddInclude(x => x.ProductBrand);
        }
    }
}