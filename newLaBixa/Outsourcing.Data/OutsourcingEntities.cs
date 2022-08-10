using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using Outsourcing.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Outsourcing.Data
{
    public class OutsourcingEntities : IdentityDbContext<User>
    {

        public OutsourcingEntities()
            : base("OutsourcingEntities")
        {
        }
        public DbSet<ProductRelationship> ProductRelationships { get; set; }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductAttributeMapping> ProductAttributeMappings { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductPictureMapping> ProductPictureMappings { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<WebsiteAttribute> WebsiteAttributes { get; set; }
        public DbSet<CampaignDataMapping> CampaignDataMappings { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<ChanelData> ChanelData { get; set; }
        public DbSet<DataUser> DataUsers { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Logdiary> Logdiarys { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<SourceData> SourceDatas { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<UserDataMapping> UserDataMappings { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<StoreProductMapping> StoreProductMappings { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<TrackingAttendence> TrackingAttendences { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<SchedulesStoreMapping> SchedulesStoreMappings { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<OATemplate> OATemplates { get; set; }
        public DbSet<OATemplateAttribute> OATemplateAttributes { get; set; }
        public DbSet<OAZalo> OAZalo { get; set; }

        public virtual void Commit()
        {
            try
            {
                
                base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
           
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

        }


        





 
    }
}