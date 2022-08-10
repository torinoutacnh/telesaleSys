using Microsoft.EntityFrameworkCore;
using XProject.Contract.Repository.Models;

namespace XProject.Repository.Infrastructure
{
    public sealed partial class AppDbContext
    {
        public DbSet<CallsHistoryEntity> CallHistories { get; set; }
        public DbSet<BrandEntity> Brands { get; set; }
        public DbSet<EventZaloEntity> EventZalos { get; set; }
        public DbSet<OrderItemZaloEntity> OrderItemZalos { get; set; }
        public DbSet<OrderZaloEntity> OrderZalos { get; set; }
        public DbSet<SocialUserEntity> SocialUsers { get; set; }
        public DbSet<ZaloHistoryEntity> ZaloHistories { get; set; }
        public DbSet<Working_Daily> WorkingDailies { get; set; }
        public DbSet<Working_Time> WorkingTimes { get; set; }
    }
}