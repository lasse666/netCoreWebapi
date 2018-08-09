
namespace Gym.App.Repository
{
    /// <summary>
    /// 服务基类
    /// </summary>
    public class ServiceBase<T> where T:class
    {
        private readonly static object _objLock = new object();
        /// <summary>
        /// Repository 属性
        /// </summary>
        private static IRepository<T> _repository;
        /// <summary>
        /// 基接口仓库
        /// </summary>
        public static IRepository<T> QuickRepository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = new Repository<T>();
                }
                return _repository;
            }
        }


        private static IRepository<T> _instance;
        public static IRepository<T> Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Repository<T>();
                        }
                    }
                }

                return _instance;
            }
        }
    }
}
