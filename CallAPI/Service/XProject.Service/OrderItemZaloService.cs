using Invedia.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Repository.Infrastructure;
using XProject.Contract.Repository.Interface;
using XProject.Contract.Repository.Models;
using XProject.Contract.Service;

namespace XProject.Service
{

    [ScopedDependency(ServiceType = typeof(IOrderItemZaloService))]

    public class OrderItemZaloService : Base.Service, IOrderItemZaloService
    {
        private readonly IOrderItemZaloRepository _OrderItemZaloRepository;
        private readonly IUnitOfWork _uof;
        //private readonly IUniof _OrderItemZaloRepository;

        public OrderItemZaloService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _OrderItemZaloRepository = serviceProvider.GetRequiredService<IOrderItemZaloRepository>();
            _uof = serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        public OrderItemZaloEntity Create(OrderItemZaloEntity OrderItemZaloToCreate)
        {
            _OrderItemZaloRepository.Add(OrderItemZaloToCreate);
            _uof.SaveChanges();
            return OrderItemZaloToCreate;
        }

        public void Delete(string id)
        {
            var all = _OrderItemZaloRepository.Get(x => x.Id == id).FirstOrDefault();
            if (all == null)
            {
                return;
            }
            _OrderItemZaloRepository.Delete(all);
            _uof.SaveChanges();
        }

        public List<OrderItemZaloEntity> GetAll()
        {
            var all = _OrderItemZaloRepository.Get().ToList();
            return all;
        }

        public OrderItemZaloEntity GetOrderItemZaloByName(string name)
        {
            var all = _OrderItemZaloRepository.Get(x => x.Product_name == name).FirstOrDefault();
            return all;
        }

        public OrderItemZaloEntity GetById(string OrderItemZalo_id)
        {
            var all = _OrderItemZaloRepository.Get(x => x.Id == OrderItemZalo_id).FirstOrDefault();
            return all;
        }

        public OrderItemZaloEntity Update(OrderItemZaloEntity OrderItemZaloToEdit)
        {
            _OrderItemZaloRepository.Update(OrderItemZaloToEdit);
            _uof.SaveChanges();
            return OrderItemZaloToEdit;
        }
    }
}
