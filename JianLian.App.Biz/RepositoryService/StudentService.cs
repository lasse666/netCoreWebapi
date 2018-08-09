using Gym.App.Repository;
using JianLian.App.IBiz;
using JianLian.App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JianLian.App.Biz
{
    public class StudentService : Repository<Student>, IStudentService
    {
        public Student GetStudent(Guid stuKey)
        {
            return this.GetList(t => t.Key == stuKey && t.DataState == 1).FirstOrDefault();
        }
    }
}
