using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Repository.Models;

namespace XProject.Contract.Service
{
    public interface IBrandService
    {
        List<BrandEntity> GetAll();
    }
}
