using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Models;
using System;
using System.Linq.Expressions;

namespace Outsourcing.Data.Repository
{

    public class TrackingAttendenceRepository : RepositoryBase<TrackingAttendence>, ITrackingAttendenceRepository
    {
        public TrackingAttendenceRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface ITrackingAttendenceRepository : IRepository<TrackingAttendence>
    {

    }
}
