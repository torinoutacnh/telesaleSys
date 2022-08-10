using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Models;
using System;
using System.Linq.Expressions;
namespace Outsourcing.Data.Repository
{
    public class OAzaloRepository : RepositoryBase<OAZalo>, IOAZaloRepositroy
    {
        public OAzaloRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IOAZaloRepositroy : IRepository<OAZalo>
    {

    }
}
