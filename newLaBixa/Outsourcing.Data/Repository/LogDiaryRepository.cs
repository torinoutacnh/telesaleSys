using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Models;
using System;
using System.Linq.Expressions;

namespace Outsourcing.Data.Repository
{

    public class LogDiaryRepository : RepositoryBase<Logdiary>, ILogDiaryRepository
    {
        public LogDiaryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface ILogDiaryRepository : IRepository<Logdiary>
    {

    }
}
