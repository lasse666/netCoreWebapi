using System;
namespace JianLian.App.Model
{
    /// <summary>
    /// 基类接口
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntity<out TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        TKey Key { get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreDate { get; }
        /// <summary>
        /// 数据状态(用于非物理删除数据)
        /// </summary>
        int DataState { get; }
        /// <summary>
        /// 俱乐部ID
        /// </summary>
        Guid ClubKey { get; }
    }
}
