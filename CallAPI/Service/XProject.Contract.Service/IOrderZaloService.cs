using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Repository.Models;

namespace XProject.Contract.Service
{
    public interface IOrderZaloService
    {
        List<OrderZaloEntity> GetAll();
        OrderZaloEntity GetById(string OrderZalo_id);
        OrderZaloEntity GetOrderZaloByName(string name);
        OrderZaloEntity Create(OrderZaloEntity OrderZaloToCreate);
        OrderZaloEntity Update(OrderZaloEntity OrderZaloToEdit);
        void Delete(string id);
    }
}
