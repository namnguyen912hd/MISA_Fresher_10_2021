using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Core.Entities;
using MISA.Core.Entities.Filter;
using MISA.Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        #region contructor
        public ProductRepository(IConfiguration configuration) : base(configuration)
        {

        }
        #endregion

        #region method
        public override Product GetEntityById(Guid productId)
        {
            Product product = new Product();
            string sql = $"SELECT * FROM {nameof(Product)} WHERE {nameof(Product.ProductId)} = @ProductId; " +
                        $"SELECT * FROM {nameof(Product)} WHERE {nameof(Product.ProductParentId)} = @ProductId ORDER BY {nameof(Product.CreatedDate)};";
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            using (var multi = _dbConnection.QueryMultiple(sql, new { ProductId = productId }))
            {
                product = multi.Read<Product>().SingleOrDefault();
                if (product != null)
                {
                    product.Products = multi.Read<Product>().ToList();
                }
            }

            return product;
        }

        public int SaveProduct(Product product)
        {
            var result = 0;
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {   // thêm hàng hóa
                    if (product.EntityState == Core.Enums.MISAEnum.EntityState.AddNew)
                    {
                        product.ProductId = Guid.NewGuid();
                        // thêm hàng hóa cha
                        result = Add(product, transaction);

                        if (result > 0 && product.Products!=null)
                        {
                            // thêm hàng hóa con
                            foreach (Product productDetail in product.Products)
                            {
                                var temp_result = result;
                                productDetail.ProductId = Guid.NewGuid();
                                productDetail.ProductParentId = product.ProductId;
                                result += Add(productDetail, transaction);
                                if (result < temp_result)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else  // sửa hàng hóa
                    {
                        result = Update(product, transaction);
                        if (result > 0)
                        {
                            foreach (Product productDetail in product.Products)
                            {
                                var temp_result = result;
                                if (productDetail.EntityState == Core.Enums.MISAEnum.EntityState.AddNew)
                                {
                                    productDetail.ProductId = Guid.NewGuid();
                                    productDetail.ProductParentId = product.ProductId;
                                    result += Add(productDetail, transaction);
                                }
                                else if (productDetail.EntityState == Core.Enums.MISAEnum.EntityState.Update)
                                {
                                    result += Update(productDetail, transaction);
                                }
                                else if (productDetail.EntityState == Core.Enums.MISAEnum.EntityState.Delete)
                                {
                                    result += Delete(productDetail.ProductId, transaction);
                                }
                                else if (productDetail.EntityState == Core.Enums.MISAEnum.EntityState.NoAction)
                                {
                                    continue;
                                }
                                else break;                               
                            }
                        }
                        
                    }

                    if (result > 0)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception)
                {
                    result = 0;
                    transaction.Rollback();
                }
            }
            return result;
        }

        public string GetNewProductCodeSKU(string inputText)
        {
            var parameters = new DynamicParameters();
            parameters.Add($"$inputText", inputText, DbType.String);
            var result = _dbConnection.Query<String>($"Proc_GetNew{nameof(Product.ProductCodeSKU)}", parameters, commandType: CommandType.StoredProcedure);
            return result.First();
        }

        public string GetNewProductBarCode()
        {
            var result = _dbConnection.Query<String>($"Proc_GetNew{nameof(Product.ProductBarCode)}", commandType: CommandType.StoredProcedure);
            return result.First();
        }

        public int DeleteMultipleProducts(string[] arrProductId)
        {
            var rowAffects = 0;
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    string sqlQuery = "";
                    for (int i = 0; i < arrProductId.Length; i++)
                    {
                        sqlQuery = $"Delete from {nameof(Product)} where {nameof(Product.ProductId)} = @id or {nameof(Product.ProductParentId)}=@id";
                        // thực thi commandtext                    
                        rowAffects += _dbConnection.Execute(sqlQuery, new { id = arrProductId[i] }, transaction, commandType: CommandType.Text);
                    }
                    if (rowAffects > 0)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                    
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            //trả về kết quả(số bản ghi xóa được)
            return rowAffects;
        }

        public object FilterProducts(int pageIndex, int pageSize, List<ObjectFilter> objectFilters, ObjectSort objectSort)
        {
            string where = BuildSqlWhere(objectFilters);
            string sort = BuildSqlSort(objectSort);
            DynamicParameters parameters = new();
            var storeName = $"Proc_Filter{nameof(Product)}s";
            parameters.Add("$Columns", null);
            parameters.Add("$PageSize", pageSize);
            parameters.Add("$PageIndex", pageIndex);
            parameters.Add("$Where", where);
            parameters.Add("$Sort", sort);
            parameters.Add("$TotalRecord", direction: ParameterDirection.Output);
            parameters.Add("$TotalPage", direction: ParameterDirection.Output);
            // Thực thi lấy dữ liệu trong db:
            var entities = _dbConnection.Query<Product>(storeName, param: parameters, commandType: CommandType.StoredProcedure);
            var totalPage = parameters.Get<int>("$TotalPage");
            var totalRecord = parameters.Get<int>("$TotalRecord");
            return new
            {
                TotalRecord = totalRecord,
                TotalPage = totalPage,
                Data = entities
            };
        }

        /// <summary>
        /// Convert list objecFiter thành một câu lệnh điều kiện của WHERE
        /// </summary>
        /// <param name="objectFilters">Danh sách đối tượng tìm kiếm</param>
        /// <returns>Câu lệnh where trong sql</returns>
        /// CreatedBy: namnguyen(17/01/2022)
        private static string BuildSqlWhere(List<ObjectFilter> objectFilters)
        {
            string where = string.Empty;
            if (objectFilters.Count > 0)
            {
                foreach (var item in objectFilters)
                {
                    // thêm điều kiện vào chuỗi where
                    // Kiểm tra toán tử
                    if (item.ValueType.Equals(typeof(string).Name, StringComparison.OrdinalIgnoreCase))
                    { /// Nếu cột là string
                        switch (item.Operator)
                        {
                            case Core.Enums.OperatorEnum.Operator.Contain:
                                where += $" {item.Column} LIKE '%{item.Value}%' {item.AdditionalOperator}";
                                break;
                            case Core.Enums.OperatorEnum.Operator.EqualTo:
                                where += $" {item.Column} = '{item.Value}' {item.AdditionalOperator}";
                                break;
                            case Core.Enums.OperatorEnum.Operator.BeginWith:
                                where += $" {item.Column} LIKE '{item.Value}%' {item.AdditionalOperator}";
                                break;
                            case Core.Enums.OperatorEnum.Operator.EndWith:
                                where += $" {item.Column} LIKE '%{item.Value}' {item.AdditionalOperator}";
                                break;
                            case Core.Enums.OperatorEnum.Operator.NotContain:
                                where += $" {item.Column} NOT LIKE '%{item.Value}%' {item.AdditionalOperator}";
                                break;
                            default:
                                break;
                        }
                    }
                    else if (item.ValueType == typeof(int).Name)
                    {
                        switch (item.Operator)
                        {
                            case Core.Enums.OperatorEnum.Operator.EqualTo:
                                where += $" {item.Column} = {item.Value} {item.AdditionalOperator}";
                                break;
                            case Core.Enums.OperatorEnum.Operator.LessThan:
                                where += $" {item.Column} < {item.Value} {item.AdditionalOperator}";
                                break;
                            case Core.Enums.OperatorEnum.Operator.LessThanOrEqualTo:
                                where += $" {item.Column} <= {item.Value} {item.AdditionalOperator}";
                                break;
                            case Core.Enums.OperatorEnum.Operator.MoreThan:
                                where += $" {item.Column} > {item.Value} {item.AdditionalOperator}";
                                break;
                            case Core.Enums.OperatorEnum.Operator.MoreThanOrEqualTo:
                                where += $" {item.Column} >= {item.Value} {item.AdditionalOperator}";
                                break;
                            default:
                                break;
                        }
                    }             
                }
                // Cắt bỏ điều kiện AND/OR thừa ở cuối chuỗi
                where = where.Substring(0, where.LastIndexOf(" "));
            }
            return where;
        }

        /// <summary>
        /// Convert thành 1 câu lệnh điều kiện của ORDER BY
        /// </summary>
        /// <param name="objectSort">Đối tượng cần sẵp xếp</param>
        /// <returns>Câu lệnh where trong sql</returns>
        /// CreatedBy: namnguyen(17/01/2022)
        private static string BuildSqlSort(ObjectSort objectSort)
        {
            string sort = string.Empty;
            if (objectSort != null)
            {
                switch (objectSort.SortOrder)
                {
                    case Core.Enums.OperatorEnum.Sort.Asc:
                        sort = $"{objectSort.Column} {objectSort.SortOrder}";
                        break;
                    case Core.Enums.OperatorEnum.Sort.Desc:
                        sort = $"{objectSort.Column} {objectSort.SortOrder}";
                        break;
                        ;
                    default:
                        break;
                }
            }
            return sort;
        }

        #endregion

    }
}
