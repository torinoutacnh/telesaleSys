using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Models;
using System;
using System.Linq.Expressions;

namespace Outsourcing.Data.Repository
{
  
    public class SourceDataRepository : RepositoryBase<SourceData>, ISourceDataRepository
    {
        public SourceDataRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface ISourceDataRepository : IRepository<SourceData>
    {

    }
}
