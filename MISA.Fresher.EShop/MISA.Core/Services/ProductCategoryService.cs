using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class ProductCategoryService : BaseService<ProductCategory>, IProductCategoryService
    {
        #region declare

        IProductCategoryRepository _productCategoryRepository;

        #endregion

        #region contructor
        public ProductCategoryService(IProductCategoryRepository productCategoryRepository) : base(productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        #endregion

    }
}
