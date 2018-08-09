using Dapper;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Gym.App.Repository
{
    /// <summary>
    /// 数据 仓库
    /// </summary>
    public interface IRepository<T> where T:class
    {
        #region 扩展
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="sort">排序</param>
        /// <param name="buffered">缓存</param>
        /// <returns></returns>
        IEnumerable<T> GetList(Expression<Func<T, bool>> expression = null, IList<ISort> sort = null, bool buffered = false);
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        T Get(Expression<Func<T, bool>> expression = null);
        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> expression = null, bool buffered = false);
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="expression"></param>
        /// <param name="sort"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        IEnumerable<T> GetPaged(int pageIndex, int pageSize, Expression<Func<T, bool>> expression = null, bool buffered = false);
        /// <summary>
        /// 查看指定的数据是否存在
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <returns></returns>
        bool Exists(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool Delete(Expression<Func<T, bool>> expression);
        bool Delete(Expression<Func<T, bool>> expression, IDbConnection conn = null, IDbTransaction trans = null);
        #endregion


        //select
        T GetById(dynamic primaryId);//;

        T GetById(IDbConnection conn, dynamic primaryId, IDbTransaction trans);
        T GetDefaultByName(string colName, string value);
        T GetFirst(string sql, dynamic param = null, bool buffered = true);
        T GetFirst(IDbConnection conn, string sql, dynamic param = null, IDbTransaction trans = null, bool buffered = true);
        /// <summary>
        /// 通过表达式获取实体
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="predicate">条件</param>
        /// <param name="buffered">缓存</param>
        /// <returns></returns>
        T GetByPredicates(IDbConnection conn, object predicate, bool buffered = false);
        /// <summary>
        /// 通过表达式获取实体
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="buffered">缓存</param>
        /// <returns></returns>
        T GetByPredicates(object predicate, bool buffered = false);


        IEnumerable<T> GetByIds(IList<dynamic> ids);
        IEnumerable<T> GetAll();
        IEnumerable<T> Query(string sql, dynamic param = null, bool buffered = true);
        IEnumerable<TReturn> Query<TReturn>(string sql, dynamic param = null, bool buffered = true) where TReturn : class;
        IEnumerable<T> Query(IDbConnection conn, string sql, dynamic param = null, IDbTransaction trans = null, bool buffered = true);
        IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(IDbConnection conn, string sql, Func<TFirst, TSecond, TReturn> map,
             dynamic param = null, IDbTransaction transaction = null, bool buffered = true,
            string splitOn = "Id", int? commandTimeout = null);
        //SqlMapper.GridReader GetMultiple(string sql, IDbConnection conn, dynamic param = null, 
        //    IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        //
        int Count(IDbConnection conn, IPredicate predicate, bool buffered = false);
        int Count(IDbConnection conn, string sql, bool buffered = false);
        int Count(string sql, DynamicParameters parameters = null);

        //
        IEnumerable<T> GetList(IDbConnection conn, IPredicate predicate = null,
            IList<ISort> sort = null, bool buffered = false);
        IEnumerable<T> GetList(IPredicate predicate = null, IList<ISort> sort = null, bool buffered = false);

        //
        IEnumerable<T> GetPaged(IDbConnection conn, int pageIndex, int pageSize, object predictate,
            IList<ISort> sort = null, bool buffered = false);

        //
        IEnumerable<T> GetPaged(IDbConnection conn, Pager pager, bool buffered = false);

        //
        Int32 Execute(string sql, dynamic param = null);
        Int32 Execute(IDbConnection conn, string sql, dynamic param = null, IDbTransaction transaction = null);
        Int32 ExecuteCommand(IDbCommand cmd);
        Int32 ExecuteProc(string procName, DynamicParameters param = null);
        Int32 ExecuteProc(IDbConnection conn, string procName, DynamicParameters param = null);
        IList<T> ExecProcQuery(string procName, DynamicParameters param);
        IList<T> ExecProcQuery(IDbConnection conn, string procName, DynamicParameters param);

        object ExecuteScalar(string sql, dynamic param = null, bool buffered = false);

        //
        dynamic Insert(T entity);
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        bool Insert(IEnumerable<T> entitys);
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        bool Insert(IEnumerable<T> entitys, IDbTransaction tran, IDbConnection conn);
        dynamic Insert(IDbConnection conn, T entity, IDbTransaction transaction = null);
        void InsertBatch(IDbConnection conn, IEnumerable<T> entityList, IDbTransaction transaction = null);
        bool Update(T entity);
        bool Update(IDbConnection conn, T entity, IDbTransaction transaction = null);
        bool UpdateBatch(IDbConnection conn, IEnumerable<T> entityList, IDbTransaction transaction = null);
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        bool UpdateBatch(IEnumerable<T> entityList);
        bool Delete(dynamic primaryId);
        bool Delete(IDbConnection conn, dynamic primaryId, IDbTransaction transaction = null);
        bool Delete(IDbConnection conn, IPredicate predicate, IDbTransaction transaction = null);
        int DeleteBatch(IDbConnection conn, IEnumerable<dynamic> ids, IDbTransaction transaction = null);
        
    }
}
