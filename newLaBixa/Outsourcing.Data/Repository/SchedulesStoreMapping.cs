using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Models;
using System;
using System.Linq.Expressions;

namespace Outsourcing.Data.Repository
{
    public class SchedulesStoreMappingRepository : RepositoryBase<SchedulesStoreMapping>, ISchedulesStoreMappingRepository
    {
        public SchedulesStoreMappingRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface ISchedulesStoreMappingRepository : IRepository<SchedulesStoreMapping>
    {

    }
}

