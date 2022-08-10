using Invedia.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Repository.Interface;
using XProject.Contract.Service;

namespace XProject.Service
{
    [ScopedDependency(ServiceType = typeof(ICallsHistoryService))]

    public class CallsHistoryService : Base.Service, ICallsHistoryService
    {
        private readonly ICallsHistoryRepository _callsHistoryRepository;
        //private readonly IUniof _callsHistoryRepository;

        public CallsHistoryService(IServiceProvider serviceProvider): base(serviceProvider)
        {
            _callsHistoryRepository = serviceProvider.GetRequiredService<ICallsHistoryRepository>();
        }

        public void GetAll()
        {
            var data = _callsHistoryRepository.Get(_ => _.DeletedTime != null);
        }
    }
}
