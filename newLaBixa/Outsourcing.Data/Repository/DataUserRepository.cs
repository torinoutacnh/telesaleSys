using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Models;
using System;
using System.Linq.Expressions;

namespace Outsourcing.Data.Repository
{
    public class DataUserRepository : RepositoryBase<DataUser>, IDataUserRepository
    {
        public DataUserRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IDataUserRepository : IRepository<DataUser>
    {

    }
}
