using Invedia.Data.EF.Interfaces.DbContext;
using Invedia.Data.EF.Services.Repository;
using Invedia.DI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Repository.Interface;
using XProject.Contract.Repository.Models;

namespace XProject.Repository
{
  
    [ScopedDependency(ServiceType = typeof(IEventZaloRepository))]
    public class EventZaloRepository : Repository<EventZaloEntity>, IEventZaloRepository
    {
        public EventZaloRepository(IDbContext dbContext) : base(dbContext)
        {

        }
    }
}
