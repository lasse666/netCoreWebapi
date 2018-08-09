using System;
using System.Collections.Generic;
using System.Text;

namespace JianLian.App.Model
{
    [Table("Student")]
    public class Student : EntityBase<Guid>
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int Sex { get; set; }
    }
}
