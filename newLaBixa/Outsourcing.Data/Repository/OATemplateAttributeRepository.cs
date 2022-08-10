using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Models;
using System;
using System.Linq.Expressions;

namespace Outsourcing.Data.Repository
{

    public class OATemplateAttributeRepository : RepositoryBase<OATemplateAttribute>, IOATemplateAttributeRepository
    {
        public OATemplateAttributeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IOATemplateAttributeRepository : IRepository<OATemplateAttribute>
    {

    }
}
