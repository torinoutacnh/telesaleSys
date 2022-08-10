using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Models;
using System;
using System.Linq.Expressions;

namespace Outsourcing.Data.Repository
{

    public class ConfiguationRepository : RepositoryBase<Configuation>, IConfiguationRepository
    {
        public ConfiguationRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IConfiguationRepository : IRepository<Configuation>
    {
        void Add(Configuation configuation);
    }
}
