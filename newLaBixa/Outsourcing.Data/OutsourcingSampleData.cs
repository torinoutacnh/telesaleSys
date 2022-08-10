using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Outsourcing.Data.Models;


namespace Outsourcing.Data
{
    public class OutsourcingSampleData : DropCreateDatabaseIfModelChanges<OutsourcingEntities>
    {
        private UserManager<User> UserManager;
        private RoleManager<IdentityRole> RoleManager;

        protected override void Seed(OutsourcingEntities context)
        {
            UserManager = new UserManager<User>(new UserStore<User>(context));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var listRoles = new List<string>(new string[] { "Admin", "Telesale", "TeamLead" });
            foreach (var role in listRoles)
            {
                if (!RoleManager.RoleExists(role))
                {
                    RoleManager.Create(new IdentityRole(role));
                }
            }
            //Create User

            var users = new List<User>() {
                  new User(){ UserName = "admin", Extention="107",Activated=true, FirstName = "Company", LastName = "Admin"},
                  new User(){ UserName = "tele101",Extention="101",Activated=true, FirstName = "Company", LastName = "Telesale"},
                  new User(){ UserName = "tele102",Extention="102",Activated=true, FirstName = "Company", LastName = "Telesale"},
                  new User(){ UserName = "tele103",Extention="103",Activated=true, FirstName = "Company", LastName = "Telesale"},
                new User(){ UserName = "tele104",Extention="104",Activated=true, FirstName = "Company", LastName = "Telesale"}};
            foreach (var user in users)
            {
                if (UserManager.FindByName(user.UserName) == null)
                {
                    UserManager.Create(user, "123456");
                    if (user.UserName == "admin")
                    {
                        UserManager.AddToRole(user.Id, "Admin");
                    }
                    else
                    {
                        UserManager.AddToRole(user.Id, "Telesale");
                    }
                }
            }


            //create Brand 
            var brand = context.Brands.Add(new Brand() { Name = "AmazingTech", Address = "", Code = "AMA", DateCreated = DateTime.Now, Email = "truonglongkt12@gmail.com", IsActive = true, IsDeleted = false, LastEditedTime = DateTime.Now, ManagerName = "Long" });
            context.Levels.Add(new Level() { BrandCode = "AMA", BrandName = brand.Name, CodeLevel = "1", BrandId = brand.Id, DateCreated = DateTime.Now, Deleted = false, IsActive = true, LastEditedTime = DateTime.Now, LevelName = "Khach hang moi", Position = 0 });
            var source = context.SourceDatas.Add(new SourceData() { Code = "SK", DateCreated = DateTime.Now, Deleted = false, IsActive = true, LastEditedTime = DateTime.Now, Name = "Source Khác", Position = 0, ShortName = "SK" });
            context.ChanelData.Add(new ChanelData() { Name = "Khác", DateCreated = DateTime.Now, Code = "K", Deleted = false, IsActive = true, LastEditedTime = DateTime.Now, Position = 0, SourceDataId = source.Id });

            context.BlogCategories.Add(new BlogCategory() { Name = "None", Slug = "none", Status = true, IsStaticPage = false, DisplayOrder = 9999, Description = "Danh Mục Con Của None Sẽ Là Danh Mục Cấp 1" });
            context.BlogCategories.Add(new BlogCategory() { Name = "Trang Tĩnh", Slug = "trang-tinh", Status = true, IsStaticPage = true });
            context.BlogCategories.Add(new BlogCategory() { Name = "Tin tức", Slug = "tin-tuc", Status = true, IsStaticPage = false });
            context.BlogCategories.Add(new BlogCategory() { Name = "Sự kiện", Slug = "su-kien", Status = true, IsStaticPage = false });

            context.ProductCategories.Add(new ProductCategory() { Name = "DanhMuc1", Slug = "danh-muc-1", Deleted = false });
            context.ProductCategories.Add(new ProductCategory() { Name = "DanhMuc2", Slug = "danh-muc-2", Deleted = false });
            context.Staffs.Add(new Staff() { Name = "Trương Long", Phone = "0922007855", Type = 1, skype = "longt711", Yahoo = "longt711", Deleted = false });
            context.Staffs.Add(new Staff() { Name = "Ngọc Linh", Phone = "0922007855", Type = 2, skype = "longt711", Yahoo = "longt711", Deleted = false });
            context.Staffs.Add(new Staff() { Name = "Truong Long", Phone = "0922007855", Type = 3, skype = "longt711", Yahoo = "longt711", Deleted = false });
            context.Staffs.Add(new Staff() { Name = "Truong Long", Phone = "0922007855", Type = 4, skype = "longt711", Yahoo = "longt711", Deleted = false });

            context.WebsiteAttributes.Add(new WebsiteAttribute() { Name = "Labixa.PopupWebsite", IsPublic = true, Deleted = false });
            context.SaveChanges();
        }

    }
}