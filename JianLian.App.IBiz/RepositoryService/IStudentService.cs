using Gym.App.Repository;
using JianLian.App.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace JianLian.App.IBiz
{
    public interface IStudentService : IRepository<Student>
    {
        Student GetStudent(Guid stuKey);
    }
}
