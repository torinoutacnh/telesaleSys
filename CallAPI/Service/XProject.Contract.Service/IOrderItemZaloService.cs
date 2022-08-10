using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Repository.Models;

namespace XProject.Contract.Service
{
    public interface IOrderItemZaloService
    {
        List<OrderItemZaloEntity> GetAll();
        OrderItemZaloEntity GetById(string OrderItemZalo_id);
        OrderItemZaloEntity GetOrderItemZaloByName(string name);
        OrderItemZaloEntity Create(OrderItemZaloEntity OrderItemZaloToCreate);
        OrderItemZaloEntity Update(OrderItemZaloEntity OrderItemZaloToEdit);
        void Delete(string id);
    }
}
