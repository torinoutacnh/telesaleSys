using Invedia.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Repository.Interface;
using XProject.Contract.Repository.Models;
using XProject.Contract.Service;

namespace XProject.Service
{
   
    [ScopedDependency(ServiceType = typeof(IBrandService))]

    public class BrandService : Base.Service, IBrandService
    {
        private readonly IBrandRepository _BrandRepository;
        //private readonly IUniof _BrandRepository;

        public BrandService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _BrandRepository = serviceProvider.GetRequiredService<IBrandRepository>();
        }

        public List<BrandEntity> GetAll()
        {
            return _BrandRepository.Get(p => p.isActive && !p.Deleted).ToList();
        }
    }
}
