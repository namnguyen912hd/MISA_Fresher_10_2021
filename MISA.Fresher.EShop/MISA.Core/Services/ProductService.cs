using MISA.Core.Entities;
using MISA.Core.Entities.Filter;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class ProductService : BaseService<Product>, IProductService
    {
        #region declare

        IProductRepository _productRepository;

        #endregion

        #region contructor
        public ProductService(IProductRepository productRepository) : base(productRepository)
        {
            _productRepository = productRepository;
        }

        #endregion

        #region method

        public override ServiceResult Add(Product product)
        {
            product.EntityState = Enums.MISAEnum.EntityState.AddNew;
            // validate dữ liệu
            var isValidate = Validate(product);
            // validate hàng hóa con
            if (product.Products?.Count > 0)
            {
                foreach (var pro in product.Products)
                {
                    isValidate = Validate(pro);
                    if (isValidate == false) break;
                }
            }

            if (isValidate == true)
            {
                _serviceResult.Data = _productRepository.SaveProduct(product);
                _serviceResult.MISACode = Enums.MISAEnum.MISACode.IsValid;
            }
            return _serviceResult;
        }

        public ServiceResult DeleteMultipleProducts(string[] arrProductId)
        {
            var res = _productRepository.DeleteMultipleProducts(arrProductId);
            if (res != 0)
            {
                _serviceResult.Data = res;
                _serviceResult.MISACode = Enums.MISAEnum.MISACode.Success;
            }
            else
            {
                _serviceResult.Message = Properties.Resources.Msg_IsNotValid;
                _serviceResult.MISACode = Enums.MISAEnum.MISACode.NotValid;
            }
            return _serviceResult;
        }

        public object FilterProducts(int pageIndex, int pageSize, string objectFilters, string objectSort)
        {
            // Convert string json => list object
            List<ObjectFilter> objectFiltersJson;
            ObjectSort objectSortJson;

            objectFiltersJson = JsonConvert.DeserializeObject<List<ObjectFilter>>(objectFilters);

            if (objectSort == null)
            {
                objectSortJson = null;
            }
            else
            {
                objectSortJson = JsonConvert.DeserializeObject<ObjectSort>(objectSort);
            }

            return _productRepository.FilterProducts(pageIndex, pageSize, objectFiltersJson, objectSortJson);
        }

        public string GetNewProductBarCode()
        {
            return _productRepository.GetNewProductBarCode();
        }

        public string GetNewProductCodeSKU(string inputText)
        {
            inputText = inputText.Replace("  ", " ").ToUpper();
            string productCode = "";
            // Lấy ra chữ cái đầu của từng từ và viết hoa lên
            foreach (var part in inputText.Split(' '))
            {
                productCode += part.Substring(0, 1);
            }
            return _productRepository.GetNewProductCodeSKU(productCode);
        }

        public override ServiceResult Update(Product product)
        {
            product.EntityState = Enums.MISAEnum.EntityState.Update;
            var isValidate = Validate(product);
            if (product.Products.Count > 0)
            {
                foreach (var pro in product.Products)
                {
                    isValidate = Validate(pro);
                    if (isValidate == false) break;
                }
            }
            if (isValidate == true)
            {
                _serviceResult.Data = _productRepository.SaveProduct(product);
                _serviceResult.MISACode = Enums.MISAEnum.MISACode.IsValid;
            }
            return _serviceResult;
        }

        #endregion
    }
}
