using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Models;
using System;
using System.Linq.Expressions;

namespace Outsourcing.Data.Repository
{
  
    public class WardRepository : RepositoryBase<Ward>, IWardRepository
    {
        public WardRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IWardRepository : IRepository<Ward>
    {

    }
}
