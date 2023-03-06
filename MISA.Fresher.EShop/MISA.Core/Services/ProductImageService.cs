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
    public class ProductImageService : BaseService<ProductImage>, IProductImageService
    {
        #region declare

        IProductImageRepository _productImageRepository;

        #endregion

        #region contructor
        public ProductImageService(IProductImageRepository productImageRepository) : base(productImageRepository)
        {
            _productImageRepository = productImageRepository;
        }

        #endregion
    }
}
