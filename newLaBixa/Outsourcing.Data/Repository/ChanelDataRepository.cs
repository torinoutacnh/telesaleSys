using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Models;
using System;
using System.Linq.Expressions;

namespace Outsourcing.Data.Repository
{
    public class ChanelDataRepository : RepositoryBase<ChanelData>, IChanelDataRepository
    {
        public ChanelDataRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IChanelDataRepository : IRepository<ChanelData>
    {

    }
}
