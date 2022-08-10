using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invedia.Data.EF.Interfaces.Repository;

using XProject.Contract.Repository.Models;

namespace XProject.Contract.Repository.Interface
{
    public interface IWorkingTimeRepository: IRepository<WorkingTime>
    {
    }
}
