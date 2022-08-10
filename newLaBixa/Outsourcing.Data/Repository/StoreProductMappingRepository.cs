using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Models;
using System;
using System.Linq.Expressions;

namespace Outsourcing.Data.Repository
{
    public class StoreProductMappingRepository : RepositoryBase<StoreProductMapping>, IStoreProductMappingRepository
    {
        public StoreProductMappingRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IStoreProductMappingRepository : IRepository<StoreProductMapping>
    {

    }
}
