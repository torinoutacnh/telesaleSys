using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Models;
using System;
using System.Linq.Expressions;

namespace Outsourcing.Data.Repository
{
    public class OATemplateRepository : RepositoryBase<OATemplate>, IOATemplateRepository
    {
        public OATemplateRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IOATemplateRepository : IRepository<OATemplate>
    {

    }
   
}
