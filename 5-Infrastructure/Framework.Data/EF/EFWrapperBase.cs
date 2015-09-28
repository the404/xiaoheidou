using Framework.Common.LogOperation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Data.EF
{
    public class EFWrapperBase<T> : IDisposable
        where T : class,new()
    {
        #region Members

        private GContext _context;
        protected DbSet<T> _set;

        private static List<string> _lstLoggerPrintFilter = new List<string>() { "INSERT", "DELETE", "UPDATE", "SELECT" };

        /// <summary>
        /// 数据库上下文
        /// </summary>
        public GContext Context
        {
            get { return _context; }
        }

        /// <summary>
        /// 数据库实体集合
        /// </summary>
        public DbSet<T> DbSet
        {
            get
            {
                if (_set == null)
                {
                    _set = this._context.Set<T>();
                }
                return _set;
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">数据库上下文，如果指定一个数据库上下文则沿用指定的上下文，用于支持跨表操作</param>
        public EFWrapperBase(GContext context = null)
        {
            if (context == null)
            {
                this._context = new GContext();
            }
            else
            {
                this._context = context;
            }

            this._context.Database.Log = log =>
            {
                if (_lstLoggerPrintFilter.FirstOrDefault(x => log.StartsWith(x)) != null)
                {
                    Logger.Info("Execute SQL -> " + Environment.NewLine + log);//支持打印SQL
                }
            };
        }

        #endregion

        #region Add

        /// <summary>
        /// 添加到数据库，普通的EF操作，需要自行调用Save用来持久化到数据库
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        #endregion

        #region Delete

        /// <summary>
        /// 根据Entity删除，暂不支持DTO，且需要调用Save持久化到数据库
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        /// <summary>
        /// 根据表达式树删除，需要调用Save持久化到数据库
        /// </summary>
        /// <param name="exp"></param>
        public void Delete(Expression<Func<T, bool>> exp)
        {
            foreach (var obj in DbSet.Where(exp))
            {
                DbSet.Remove(obj);
            }
        }

        #endregion

        #region Modify

        /// <summary>
        /// 修改数据库数据，普通EF操作，需要自行调用Save方法
        /// </summary>
        /// <param name="entity"></param>
        public void Modify(T entity)
        {
            this._context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        #endregion

        #region Find

        /// <summary>
        /// 查询，返回Entity对象
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public virtual T Find(Expression<Func<T, bool>> exp)
        {
            return DbSet.FirstOrDefault(exp);
        }

        /// <summary>
        /// 查询，返回Entity对象
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> exp)
        {
            return await DbSet.FirstOrDefaultAsync(exp);
        }

        #endregion

        #region FindAll

        /// <summary>
        /// 查询，返回Entity列表
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public virtual List<T> FindAll(Expression<Func<T, bool>> exp)
        {
            return DbSet.Where(exp).ToList();
        }

        /// <summary>
        /// 查询，返回Entity列表
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> exp)
        {
            return await DbSet.Where(exp).ToListAsync();
        }

        /// <summary>
        /// 查询，返回Entity列表
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> FindAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        #endregion

        #region SqlQuery

        /// <summary>
        /// 直接通过Sql语句进行查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<T> SqlQuery(string sql, params object[] parameters)
        {
            return _context.Database.SqlQuery<T>(sql, parameters).ToList();
        }

        #endregion

        #region Save

        /// <summary>
        /// 保存,带泛型的快速操作方法以及查询操作不需要调用
        /// </summary>
        /// <returns></returns>
        public virtual int Save()
        {
            return _context.SaveChanges();
        }

        /// <summary>
        /// 保存,带泛型的快速操作方法以及查询操作不需要调用
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        #endregion

        #region GetPage

        public List<T> GetPage<OrderKey>(int currentPageIndex, int pageSize, Expression<Func<T, OrderKey>> order, bool isDescending, Expression<Func<T, bool>> whereExp = null)
        {
            int skipCount = currentPageIndex * pageSize;
            List<T> lstResult = null;

            if (whereExp != null)
            {
                if (!isDescending)
                {
                    lstResult = skipCount == 0 ? DbSet.OrderBy(order).Where(whereExp).Take(pageSize).ToList() : DbSet.OrderBy(order).Where(whereExp).Skip(skipCount).Take(pageSize).ToList();
                }
                else
                {
                    lstResult = skipCount == 0 ? DbSet.OrderByDescending(order).Where(whereExp).Take(pageSize).ToList() : DbSet.OrderByDescending(order).Where(whereExp).Skip(skipCount).Take(pageSize).ToList();
                }
            }
            else
            {
                if (!isDescending)
                {
                    lstResult = skipCount == 0 ? DbSet.OrderBy(order).Take(pageSize).ToList() : DbSet.OrderBy(order).Skip(skipCount).Take(pageSize).ToList();
                }
                else
                {
                    lstResult = skipCount == 0 ? DbSet.OrderByDescending(order).Take(pageSize).ToList() : DbSet.OrderByDescending(order).Skip(skipCount).Take(pageSize).ToList();
                }

            }
            return lstResult.ToList();
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            if (this._context != null)
            {
                this._context.Dispose();
            }
        }

        #endregion
    }
}
