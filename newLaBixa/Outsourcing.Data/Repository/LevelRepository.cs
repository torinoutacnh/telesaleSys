using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Models;
using System;
using System.Linq.Expressions;

namespace Outsourcing.Data.Repository
{
    public class LevelRepository : RepositoryBase<Level>, ILevelRepository
    {
        public LevelRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface ILevelRepository : IRepository<Level>
    {

    }
}
