using Invedia.Data.EF.Interfaces.DbContext;
using Invedia.DI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Repository.Interface;
using XProject.Contract.Repository.Models;
using XProject.Repository.Infrastructure;

namespace XProject.Repository
{
    [ScopedDependency(ServiceType = typeof(IWorking_DailyRepository))]
    public class Working_DailyRepository : Repository<Working_Daily>, IWorking_DailyRepository
    {
        public Working_DailyRepository(IDbContext dbContext) : base(dbContext)
        {

        }
    }
}
