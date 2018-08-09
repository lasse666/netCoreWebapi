using System;
namespace JianLian.App.Model
{
    /// <summary>
    /// 映射数据库表对象
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false), Serializable]
    public class Table : Attribute
    {
        public string TableName;
        public Table(string tblName)
        {
            TableName = tblName;
        }
    }

}
