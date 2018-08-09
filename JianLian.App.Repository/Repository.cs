using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using DapperExtensions;
using System.Data.SqlClient;
using DapperExtensions.Sql;
using System.Linq.Expressions;

namespace Gym.App.Repository
{
    /// <summary>
    /// Repository基类
    /// </summary>
    public class Repository<T> : IRepository<T> where T:class
    {
        public Repository()
        {

        }

        #region 扩展

        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<T> GetList(Expression<Func<T, bool>> expression = null, IList<ISort> sort = null,
            bool buffered = false)
        {
            using (IDbConnection conn = SessionFactory.CreateConnection())
            {
                var predicate = DapperLinqBuilder<T>.FromExpression(expression);
                return conn.GetList<T>(predicate, sort, null, null, buffered);
            }


        }
        public IEnumerable<T> GetList(IDbConnection conn, Expression<Func<T, bool>> expression = null, IList<ISort> sort = null,
            bool buffered = false)
        {
            var predicate = DapperLinqBuilder<T>.FromExpression(expression);
            return conn.GetList<T>(predicate, sort, null, null, buffered);
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="expression"></param>
        /// <param name="sort"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<T> GetList(IDbConnection conn, IDbTransaction trans = null, Expression<Func<T, bool>> expression = null, IList<ISort> sort = null,
      bool buffered = false)
        {
            var predicate = DapperLinqBuilder<T>.FromExpression(expression);
            return conn.GetList<T>(predicate, sort, trans, null, buffered);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        public T Get(Expression<Func<T, bool>> expression = null)
        {
            using (IDbConnection conn = SessionFactory.CreateConnection())
            {
                var predicate = DapperLinqBuilder<T>.FromExpression(expression);
                return conn.GetList<T>(predicate, null, null, null, true).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="buffered">缓存</param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> expression = null, bool buffered = false)
        {
            using (IDbConnection conn = SessionFactory.CreateConnection())
            {
                var predicate = DapperLinqBuilder<T>.FromExpression(expression);
                return conn.Count<T>(predicate);
            }
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="expression"></param>
        /// <param name="sort"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<T> GetPaged(int pageIndex, int pageSize, Expression<Func<T, bool>> expression = null, bool buffered = true)
        {
            using (IDbConnection conn = SessionFactory.CreateConnection())
            {
                var predicate = DapperLinqBuilder<T>.FromExpression(expression);
                return conn.GetPage<T>(predicate, pageIndex, pageSize, null, null, buffered);
            }
        }
        /// <summary>
        /// 查看指定的数据是否存在
        /// </summary>
        /// <param name="expression">Lambda表达式</param>
        /// <returns></returns>
        public bool Exists(Expression<Func<T, bool>> expression)
        {
            using (IDbConnection conn = SessionFactory.CreateConnection())
            {
                var predicate = DapperLinqBuilder<T>.FromExpression(expression);
                return conn.Count<T>(predicate) > 0;
            }

        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool Delete(Expression<Func<T, bool>> expression)
        {
            using (IDbSession session = SessionFactory.CreateSession())
            {
                var list = GetList(session.Connection, expression);
                var trans = session.BeginTrans();
                try
                {
                    foreach (var item in list)
                    { 
                        session.Connection.Delete(item, trans);
                    }
                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();//事物回滚
                    return false;
                    throw ex;
                }

            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool Delete(Expression<Func<T, bool>> expression, IDbConnection conn = null, IDbTransaction trans = null)
        {
            var list = GetList(conn, trans, expression);
            try
            {
                foreach (var item in list)
                {
                    conn.Delete(item, trans);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        #endregion




        /// <summary>
        /// 根据主键ID获取记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryId"></param>
        /// <returns></returns>
        public T GetById(dynamic primaryId)
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            try
            {
                return conn.Get<T>(primaryId as object);
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryId"></param>
        /// <returns></returns>
        public T GetById(IDbConnection conn, dynamic primaryId, IDbTransaction trans)
        {
            return conn.Get<T>(primaryId as object, trans);
        }

        /// <summary>
        /// 根据字段列名称获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public T GetDefaultByName(string colName, string value)
        {
            var dataList = GetByName(colName, value).ToList<T>();

            if (dataList.Count() > 0)
                return dataList.FirstOrDefault<T>();
            else
                return null;
        }

        /// <summary>
        /// 获取表名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetTableName()
        {
            System.Attribute attr = System.Attribute.GetCustomAttributes(typeof(T))[0];
            var tableName = (attr as dynamic).TableName;
            return tableName;
        }

        /// <summary>
        /// 根据字段列名称获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IList<T> GetByName(string colName, string value)
        {
            var tblName = GetTableName();
            var sql = string.Format("SELECT * FROM {0} WHERE {1}=@colValue", tblName, colName);

            try
            {
                using (IDbConnection conn = SessionFactory.CreateConnection())
                {
                    IList<T> dataList = SqlMapper.Query<T>(conn, sql, new { colValue = value }).ToList();
                    return dataList;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 根据多个Id获取多个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<T> GetByIds(IList<dynamic> ids)
        {
            var tblName = GetTableName();
            var idsin = string.Join(",", ids.ToArray<dynamic>());
            var sql = string.Format("SELECT * FROM dbo.{0} WHERE Id in (@ids)", tblName);

            IDbConnection conn = SessionFactory.CreateConnection();
            try
            {
                IEnumerable<T> dataList = SqlMapper.Query<T>(conn, sql, new { ids = idsin });
                return dataList;
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 获取全部数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            try
            {
                IEnumerable<T> dataList = conn.GetList<T>();
                return dataList;
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 查询匹配的一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public T GetFirst(string sql, dynamic param = null, bool buffered = true)
        {
            using (IDbConnection conn = SessionFactory.CreateConnection())
            {
                T entity = null;
                var list = SqlMapper.Query<T>(conn, sql, param as object, null, buffered).ToList();
                if (list != null && list.Count() > 0)
                {
                    entity = list[0];
                }
                return entity;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public T GetFirst(IDbConnection conn, string sql, dynamic param = null, IDbTransaction trans = null,
            bool buffered = true)
        {
            T entity = null;
            var list = SqlMapper.Query<T>(conn, sql, param as object, trans, buffered).ToList();
            if (list != null && list.Count() > 0)
            {
                entity = list[0];
            }
            return entity;

        }

        /// <summary>
        /// 根据条件筛选出数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<T> Query(string sql, dynamic param = null, bool buffered = true)
        {
            using (IDbConnection conn = SessionFactory.CreateConnection())
            {
                return SqlMapper.Query<T>(conn, sql, param as object, null, buffered);
            }
        }

        public IEnumerable<TReturn> Query<TReturn>(string sql, dynamic param = null, bool buffered = true) where TReturn : class
        {
            using (IDbConnection conn = SessionFactory.CreateConnection())
            {
                return SqlMapper.Query<TReturn>(conn, sql, param as object, null, buffered);
            }
        }


        /// <summary>
        /// 根据条件筛选出数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<T> Query(IDbConnection conn, string sql, dynamic param = null, IDbTransaction trans = null, bool buffered = true)
        {
            return SqlMapper.Query<T>(conn, sql, param as object, trans, buffered);
        }

        /// <summary>
        /// 根据表达式筛选
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(IDbConnection conn, string sql, Func<TFirst, TSecond, TReturn> map,
            dynamic param = null, IDbTransaction transaction = null, bool buffered = true,
            string splitOn = "Id", int? commandTimeout = null)
        {
            return SqlMapper.Query(conn, sql, map, param as object, transaction, buffered, splitOn);
        }

        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<T> GetList(IDbConnection conn, IPredicate predicate = null, IList<ISort> sort = null,
            bool buffered = false)
        {
            return conn.GetList<T>(predicate, sort, null, null, buffered);
        }


        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<T> GetList(IPredicate predicate = null, IList<ISort> sort = null, bool buffered = false)
        {
            using (IDbConnection conn = SessionFactory.CreateConnection())
            {
                return conn.GetList<T>(predicate, sort, null, null, buffered);
            }
        }
        /// <summary>
        /// 分页方法调用示例：
        /// 1. 单一条件
        //  using (SqlConnection cn = new SqlConnection(_connectionString))
        //  {
        //    cn.Open();
        //
        //    //排序字段
        //    var sortList = new List<DapperExtensions.ISort>();
        //    sortList.Add(new DapperExtensions.Sort { PropertyName = "ID", Ascending = false });
        //
        //    var predicate = Predicates.Field<Person>(f => f.Active, Operator.Eq, true);
        //    List<Person> list = cn.GetPaged<Person>(cn, query.PageIndex, query.PageSize, 
        //            predicate, sortList, false).ToList();
        //
        //    cn.Close();
        //  }
        //
        //  2. 组合条件
        //  using (SqlConnection cn = new SqlConnection(_connectionString))
        //  {
        //    cn.Open();
        //
        //    //排序字段
        //    var sortList = new List<DapperExtensions.ISort>();
        //    sortList.Add(new DapperExtensions.Sort { PropertyName = "ID", Ascending = false });
        //
        //    var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
        //    pg.Predicates.Add(Predicates.Field<Person>(f => f.Active, Operator.Eq, true));
        //    pg.Predicates.Add(Predicates.Field<Person>(f => f.LastName, Operator.Like, "Br%"));
        //
        //    List<Person> list = cn.GetPaged<Person>(cn, query.PageIndex, query.PageSize, 
        //            pg, sortList, false).ToList();
        //
        //    cn.Close();
        //  }
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="allRowsCount">总记录数</param>
        /// <param name="predicate">条件</param>
        /// <param name="sort">排序</param>
        /// <param name="buffered">缓存</param>
        /// <returns></returns>
        public IEnumerable<T> GetPaged(IDbConnection conn, int pageIndex, int pageSize, object predicate,
            IList<ISort> sort = null, bool buffered = false)
        {
            return conn.GetPage<T>(predicate, sort, pageIndex, pageSize, null, null, buffered);
        }

        /// <summary>
        /// 分页查询（存储过程）
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="conn">连接</param>
        /// <param name="pager">分页对象</param>
        /// <param name="buffered">缓存</param>
        /// <returns></returns>
        public IEnumerable<T> GetPaged(IDbConnection conn, Pager pager, bool buffered = false)
        {
            var tblName = string.IsNullOrEmpty(pager.TableName) ? GetTableName() : pager.TableName;
            var keyFieldName = string.IsNullOrEmpty(pager.KeyFieldName) ? "ID" : pager.KeyFieldName;

            var p = new DynamicParameters();
            p.Add("@pageIndex", pager.PageIndex);
            p.Add("@pageSize", pager.PageSize);
            p.Add("@tblName", tblName);
            p.Add("@fldName", keyFieldName);
            p.Add("@isDesc", pager.IsDesc);
            p.Add("@strWhere", pager.StrWhere);
            p.Add("@fldOrder", pager.FieldOrder);

            return conn.Query<T>("pr_sys_QueryPaged", p, null, buffered, null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 统计记录总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="buffered">缓存</param>
        /// <returns></returns>
        public int Count(IDbConnection conn, IPredicate predicate, bool buffered = false)
        {
            return conn.Count<T>(predicate);
        }

        /// <summary>
        /// 通过表达式获取数据实体
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="predicate">条件</param>
        /// <param name="buffered">缓存</param>
        /// <returns></returns>
        public  T GetByPredicates(IDbConnection conn, object predicate, bool buffered = false)
        {
            return conn.GetByPredicates<T>(predicate);
        }

        /// <summary>
        /// 通过表达式获取数据实体
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="buffered">缓存</param>
        /// <returns></returns>
        public T GetByPredicates(object predicate, bool buffered = false)
        {
            using (IDbConnection conn = SessionFactory.CreateConnection())
            {
                return conn.GetByPredicates<T>(predicate);
            }
        }

        /// <summary>
        /// 统计查询语句记录总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public int Count(IDbConnection conn, string sql, bool buffered = false)
        {
            var cmd = conn.CreateCommand();
            try
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                int count = (int)(ExecuteScalar(conn, cmd) ?? 0);
                return count;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        /// <summary>
        /// 带参数的SQL的Count求和
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Count(string sql, DynamicParameters parameters = null)
        {
            using (IDbConnection conn = SessionFactory.CreateConnection())
            {
                return conn.Query<int>(sql, parameters).Single<int>();
            }
        }

        /// <summary>
        /// 获取多实体集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public SqlMapper.GridReader GetMultiple(string sql, dynamic param = null, IDbTransaction transaction = null,
            int? commandTimeout = null, CommandType? commandType = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 执行sql操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Execute(string sql, dynamic param = null)
        {
            using (IDbConnection conn = SessionFactory.CreateConnection())
            {
                return conn.Execute(sql, param as object);
            }
        }

        /// <summary>
        /// 执行sql操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Execute(IDbConnection conn, string sql, dynamic param = null, IDbTransaction transaction = null)
        {
            return conn.Execute(sql, param as object, transaction);
        }

        /// <summary>
        /// 执行command操作
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public int ExecuteCommand(IDbCommand cmd)
        {
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteProc(string procName, DynamicParameters param = null)
        {
            using (IDbConnection conn = SessionFactory.CreateConnection())
            {
                return conn.Execute(procName, param, null, null, CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteProc(IDbConnection conn, string procName, DynamicParameters param = null)
        {
            return conn.Execute(procName, param, null, null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 存储过程执行方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IList<T> ExecProcQuery(string procName, DynamicParameters param)
        {
            using (IDbConnection conn = SessionFactory.CreateConnection())
            {
                IList<T> list = conn.Query<T>(procName, param, null, false, null, CommandType.StoredProcedure).ToList<T>();
                return list;
            }
        }

        /// <summary>
        /// 存储过程执行方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IList<T> ExecProcQuery(IDbConnection conn, string procName, DynamicParameters param)
        {
            IList<T> list = conn.Query<T>(procName, param, null, false, null, CommandType.StoredProcedure).ToList<T>();
            return list;
        }

        /// <summary>
        /// 执行SQL语句，返回查询结果
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public object ExecuteScalar(IDbConnection conn, string sql, bool buffered = false)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            return ExecuteScalar(conn, cmd);
        }

        //public object ExecuteScalar( string sql, bool buffered = false)
        //{
        //    using (IDbConnection conn = SessionFactory.CreateConnection())
        //    {
        //        var cmd = conn.CreateCommand();
        //        cmd.CommandText = sql;
        //        cmd.CommandType = CommandType.Text;

        //        return ExecuteScalar(conn, cmd);
        //    }
        //}
        public object ExecuteScalar(string sql, dynamic param = null, bool buffered = false)
        {
            using (IDbConnection conn = SessionFactory.CreateConnection())
            {
                //var cmd = conn.CreateCommand();
                //cmd.CommandText = sql;
                //cmd.Parameters = param;
                //cmd.CommandType = CommandType.Text;

                //return ExecuteScalar(conn, cmd);
                return conn.ExecuteScalar(sql, param as object, null, null, CommandType.Text);
            }
        }

        /// <summary>
        /// 执行SQL语句，并返回数值
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public object ExecuteScalar(IDbConnection conn, IDbCommand cmd)
        {
            try
            {
                bool wasClosed = conn.State == ConnectionState.Closed;
                if (wasClosed) conn.Open();

                return cmd.ExecuteScalar();
            }
            catch (System.Exception ex)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns>主键自增值</returns>
        public dynamic Insert(T entity)
        {
            dynamic newId = 0;
            var conn = SessionFactory.CreateConnection();
            //session.BeginTrans();
            try
            {
                newId = Insert(conn, entity, null);
                //session.Commit();
            }
            catch (System.Exception ex)
            {
                //session.Rollback();
                throw;
            }
            finally
            {
                conn.Dispose();
            }
            return newId;
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool Insert(IEnumerable<T> entitys)
        {

            using (IDbSession session = SessionFactory.CreateSession())
            {
                IDbTransaction tran = session.BeginTrans();
                try
                {
                    session.Connection.Insert(entitys, transaction: tran);
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();//事物回滚
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool Insert(IEnumerable<T> entitys, IDbTransaction tran, IDbConnection conn)
        {

            try
            {
                conn.Insert(entitys, transaction: tran);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 插入单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public dynamic Insert(IDbConnection conn, T entity, IDbTransaction transaction = null)
        {
            dynamic result = conn.Insert<T>(entity, transaction);
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns>是否成功</returns>
        public bool Update(T entity)
        {
            var conn = SessionFactory.CreateConnection();

            //session.BeginTrans();
            try
            {
                var isOk = Update(conn, entity, null);
                //session.Commit();

                return isOk;
            }
            catch (System.Exception ex)
            {
                //session.Rollback();
                throw;
            }
            finally
            {
                conn.Dispose();
            }
        }

        /// <summary>
        /// 更新单条记录
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns>是否成功</returns>
        public bool Update(IDbConnection conn, T entity, IDbTransaction transaction = null)
        {
            bool isOk = conn.Update(entity, transaction);
            return isOk;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(dynamic id)
        {
            var conn = SessionFactory.CreateConnection();
            //session.BeginTrans();
            try
            {
                var isOk = Delete(conn, id, null);
                //session.Commit();
                return isOk;
            }
            catch (System.Exception ex)
            {
                //session.Rollback();
                throw;
            }
            finally
            {
                conn.Dispose();
            }
        }

        /// <summary>
        /// 删除单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryId"></param>
        /// <returns></returns>
        public bool Delete(IDbConnection conn, dynamic primaryId, IDbTransaction transaction = null)
        {
            var entity = GetById(primaryId);
            var obj = entity as T;
            bool isOk = conn.Delete(obj, transaction);
            return isOk;
        }

        /// <summary>
        /// 删除单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool Delete(IDbConnection conn, IPredicate predicate, IDbTransaction transaction = null)
        {
            return conn.Delete(predicate, transaction);
        }

        /// <summary>
        /// 批量插入功能
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        public void InsertBatch(IDbConnection conn, IEnumerable<T> entityList, IDbTransaction transaction = null)
        {
            #region 补全 过滤Ignore字段(可以过滤mapping类里面设置的ignore)
            var tblName = string.Format("dbo.{0}", typeof(T).Name);
            var tran = (SqlTransaction)transaction;
            using (var bulkCopy = new SqlBulkCopy(conn as SqlConnection, SqlBulkCopyOptions.TableLock, tran))
            {
                bulkCopy.BatchSize = entityList.Count();
                bulkCopy.DestinationTableName = tblName;
                var table = new DataTable();
                DapperExtensions.Sql.ISqlGenerator sqlGenerator = new SqlGeneratorImpl(new DapperExtensionsConfiguration());
                var classMap = sqlGenerator.Configuration.GetMap<T>();
                var props = classMap.Properties.Where(x => x.Ignored == false).ToArray();
                foreach (var propertyInfo in props)
                {
                    bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                    table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyInfo.PropertyType) ?? propertyInfo.PropertyInfo.PropertyType);
                }
                var values = new object[props.Count()];
                foreach (var itemm in entityList)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].PropertyInfo.GetValue(itemm, null);
                    }
                    table.Rows.Add(values);
                }
                bulkCopy.WriteToServer(table);
            }
            #endregion
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public bool UpdateBatch(IDbConnection conn, IEnumerable<T> entityList, IDbTransaction transaction = null)
        {
            try
            {
                foreach (var item in entityList)
                {
                    Update(conn, item, transaction);
                }
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public bool UpdateBatch(IEnumerable<T> entityList)
        {
            using (IDbSession session = SessionFactory.CreateSession())
            {
                IDbTransaction tran = session.BeginTrans();
                try
                {
                    //session.Connection.Insert(entitys, transaction: tran);
                    foreach (var item in entityList)
                    {
                        Update(session.Connection, item, tran);
                    }
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();//事物回滚
                    return false;
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteBatch(IDbConnection conn, IEnumerable<dynamic> ids, IDbTransaction transaction = null)
        {
            var tblName = GetTableName();
            var idsin = string.Join(",", ids.ToArray<dynamic>());
            var sql = string.Format("DELETE FROM dbo.{0} WHERE ID in (@ids)", tblName);
            var result = SqlMapper.Execute(conn, sql, new { ids = idsin });

            return result;
        }
    }
}
