using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Core.Attributes;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using static MISA.Core.Enums.MISAEnum;

namespace MISA.Infrastructure.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>, IDisposable where TEntity : BaseEntity
    {
        #region declare and contructor

        IConfiguration _configuration;

        /// <summary>
        /// chuỗi kết nối với cơ sở dữ liệu
        /// </summary>
        string _connectionString = string.Empty;
        protected IDbConnection _dbConnection = null;

        /// <summary>
        /// tên entity
        /// </summary>
        protected string _tableName;
        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MISAEshopConnectionString");
            _dbConnection = new MySqlConnector.MySqlConnection(_connectionString);
            _tableName = typeof(TEntity).Name;
        }

        #endregion

        #region method

        public int Add(TEntity entity)
        {
            // mapping với kiểu dữ liệu ở DB
            var parameters = MappingDbType(entity);
            // thực thi commandtext 
            var rowAffects = _dbConnection.Execute($"Proc_Insert{_tableName}", parameters, commandType: CommandType.StoredProcedure);
            //trả về kết quả(số bản ghi thêm mới được)
            return rowAffects;
        }
        public int Add(TEntity entity, IDbTransaction transaction)
        {
            // mapping với kiểu dữ liệu ở DB
            var parameters = MappingDbType(entity);
            var connection = transaction.Connection;
            var rowAffects = 0;
            if (connection?.State == ConnectionState.Open)
            {
                // thực thi commandtext 
                rowAffects = connection.Execute($"Proc_Insert{_tableName}", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
            }
            //trả về kết quả(số bản ghi thêm mới được)
            return rowAffects;
        }

        public int Update(TEntity entity)
        {
            var parameters = MappingDbType(entity);
            // thực thi commandtext 
            var rowAffects = _dbConnection.Execute($"Proc_Update{_tableName}", parameters, commandType: CommandType.StoredProcedure);

            // trả về kết quả (số bản ghi sửa được)
            return rowAffects;
        }
        public int Update(TEntity entity, IDbTransaction transaction)
        {
            var parameters = MappingDbType(entity);
            var connection = transaction.Connection;
            // thực thi commandtext 
            var rowAffects = connection.Execute($"Proc_Update{_tableName}", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

            // trả về kết quả (số bản ghi sửa được)
            return rowAffects;
        }
        public int Delete(Guid entityId)
        {
            var rowAffects = 0;
            var parameters = new DynamicParameters();
            parameters.Add($"${_tableName}Id", entityId, DbType.String);
            // thực thi commandtext 
            rowAffects = _dbConnection.Execute($"Proc_Delete{_tableName}", parameters, commandType: CommandType.StoredProcedure);
            //trả về kết quả(số bản ghi thêm mới được)
            return rowAffects;
        }
        public int Delete(Guid entityId, IDbTransaction transaction)
        {
            var rowAffects = 0;
            var parameters = new DynamicParameters();
            parameters.Add($"${_tableName}Id", entityId, DbType.String);
            // thực thi commandtext 
            rowAffects = _dbConnection.Execute($"Proc_Delete{_tableName}", parameters, transaction: transaction, commandType: CommandType.StoredProcedure);
            //trả về kết quả(số bản ghi thêm mới được)
            return rowAffects;
        }
        public IEnumerable<TEntity> GetEntities()
        {
            var entities = _dbConnection.Query<TEntity>($"Proc_Get{_tableName}s", commandType: CommandType.StoredProcedure);
            // trả dữ liệu
            return entities;
        }
        public virtual TEntity GetEntityById(Guid entityId)
        {
            var parameters = new DynamicParameters();
            parameters.Add($"${_tableName}Id", entityId, DbType.String);
            // thực thi commandtext 
            var entity = _dbConnection.Query<TEntity>($"Proc_Get{_tableName}ById", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return entity;
        }
        public TEntity GetEntityByProperty(TEntity entity, PropertyInfo property)
        {
            // lấy tên property
            var propertyName = property.Name;
            // lấy giá trị property
            var propertyValue = property.GetValue(entity);
            var keyValue = entity.GetType().GetProperty($"{_tableName}Id").GetValue(entity);
            var query = string.Empty;
            // check trạng thái đối tượng
            if (entity.EntityState == EntityState.AddNew) // trạng thái thêm -> lấy tất cả các thuộc tính
            {
                query = $"Select * from {_tableName} where {propertyName} = '{propertyValue}' ";
            }
            else if (entity.EntityState == EntityState.Update) // trạng thái sửa -> loại bỏ Id đối tượng
            {
                query = $"Select * from {_tableName} where {propertyName} = '{propertyValue}' and {_tableName}Id <> '{keyValue}' ";
            }
            else
            {
                return null;
            }
            // lấy đối tượng
            var entityReturn = _dbConnection.Query<TEntity>(query, commandType: CommandType.Text).FirstOrDefault();
            return entityReturn;

        }

        /// <summary>
        /// mapping với dữ liệu database
        /// </summary>
        /// <param name="entity">object</param>
        /// createdBy: namnguyen(14/01/2022)
        /// <returns>DynamicParameters</returns>
        private DynamicParameters MappingDbType(TEntity entity)
        {
            // lấy tất cả property của đối tượng
            var properties = entity.GetType().GetProperties()
                .Where(p =>
                {
                    return !p.IsDefined(typeof(DBNotMap), true) &&
                    p.CanRead &&
                    !(
                        p.PropertyType.IsGenericType &&
                        (
                            p.PropertyType.GetGenericTypeDefinition() == typeof(List<>) ||
                            p.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                        )
                    );
                })
                .ToList();
            var parameters = new DynamicParameters();
            // Xử lí các kiểu dữ liệu
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity);
                var propertyType = property.PropertyType;
                if (propertyType == typeof(Guid) || propertyType == typeof(Guid?)) // mapping với kiểu guid
                {
                    parameters.Add($"${propertyName}", propertyValue, DbType.String);
                }
                else if (propertyType == typeof(bool) || propertyType == typeof(bool?)) // mapping với kiểu bool
                {
                    var dbValue = ((bool)propertyValue == true ? 1 : 0);
                    parameters.Add($"${propertyName}", dbValue, DbType.Int32);
                }
                else // các kiểu dữ liệu còn lại
                {
                    parameters.Add($"${propertyName}", propertyValue);
                }
            }
            return parameters;
        }

        /// <summary>
        /// hàm tắt kết nối với DB khi sử dụng xong để tiết kiệm tại tài nguyên
        /// createdBy: namnguyen(14/01/2022)
        /// </summary>
        public void Dispose()
        {
            if (_dbConnection.State == ConnectionState.Open)
            {
                _dbConnection.Close();
                _dbConnection.Dispose();
            }
        }

        #endregion

    }
}
