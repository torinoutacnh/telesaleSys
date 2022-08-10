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

    [ScopedDependency(ServiceType = typeof(IOrderZaloService))]

    public class OrderZaloService : Base.Service, IOrderZaloService
    {
        private readonly IOrderZaloRepository _OrderZaloRepository;
        private readonly IUnitOfWork _uof;
        //private readonly IUniof _OrderZaloRepository;

        public OrderZaloService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _OrderZaloRepository = serviceProvider.GetRequiredService<IOrderZaloRepository>();
            _uof = serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        public OrderZaloEntity Create(OrderZaloEntity OrderZaloToCreate)
        {
            _OrderZaloRepository.Add(OrderZaloToCreate);
            _uof.SaveChanges();
            return OrderZaloToCreate;
        }

        public void Delete(string id)
        {
            var all = _OrderZaloRepository.Get(x => x.Id == id).FirstOrDefault();
            if (all == null)
            {
                return;
            }
            _OrderZaloRepository.Delete(all);
            _uof.SaveChanges();
        }

        public List<OrderZaloEntity> GetAll()
        {
            var all = _OrderZaloRepository.Get().ToList();
            return all;
        }

        public OrderZaloEntity GetOrderZaloByName(string name)
        {
            var all = _OrderZaloRepository.Get(x => x.Contact_Name == name).FirstOrDefault();
            return all;
        }

        public OrderZaloEntity GetById(string OrderZalo_id)
        {
            var all = _OrderZaloRepository.Get(x => x.Id == OrderZalo_id).FirstOrDefault();
            return all;
        }

        public OrderZaloEntity Update(OrderZaloEntity OrderZaloToEdit)
        {
            _OrderZaloRepository.Update(OrderZaloToEdit);
            _uof.SaveChanges();
            return OrderZaloToEdit;
        }
    }
}
