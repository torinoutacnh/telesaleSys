using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Models;
using System;
using System.Linq.Expressions;

namespace Outsourcing.Data.Repository
{
    public class CampaignDataMappingRepository : RepositoryBase<CampaignDataMapping>, ICampaignDataMappingRepository
    {
        public CampaignDataMappingRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface ICampaignDataMappingRepository : IRepository<CampaignDataMapping>
    {

    }
}
