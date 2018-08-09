
using Gym.App.Common;
using System;
namespace JianLian.App.Model
{
    public class EntityBase<TKey>:IEntity<TKey> where TKey : IEquatable<TKey>
    {
        protected EntityBase()
        {
            if (typeof(TKey) == typeof(Guid))
                Key = CombHelper.NewComb().CastTo<TKey>();
        }
        /// <summary>
        /// 默认键
        /// </summary>
        public TKey Key
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime _dateTime = DateTime.Now;
        public DateTime CreDate
        {
            get { return _dateTime; }
            set { _dateTime = value; }
        }

        /// <summary>
        /// 数据状态(用于非物理删除数据)
        /// </summary>
        public int DataState
        {
            get;
            set;
        }
        /// <summary>
        /// 俱乐部ID
        /// </summary>
        public Guid ClubKey { get; set; }
    }
}
