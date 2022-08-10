using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outsourcing.Core.Common;
using Outsourcing.Data.Models;
using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Repository;
using Outsourcing.Service.Properties;

namespace Outsourcing.Service
{
    //class AspNetUserService
    //{
    //}
    public interface IAspNetUserService
    {
        IEnumerable<AspNetUser> GetAvailableCategorys();

        IEnumerable<AspNetUser> GetAspNetUsers();
        AspNetUser GetAspNetUserById(int AspNetUserId);
        void CreateAspNetUser(AspNetUser AspNetUser);
        void EditAspNetUser(AspNetUser AspNetUserToEdit);
        void DeleteAspNetUser(int AspNetUserId);
        void SaveAspNetUser();

        //AspNetUser GetCategoryByUrlName(string );

    }
    class AspNetUserService : IAspNetUserService
    {
        #region Field
        private readonly IAspNetUserRepository AspNetUserRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public AspNetUserService(IAspNetUserRepository AspNetUserRepository, IUnitOfWork unitOfWork)
        {
            this.AspNetUserRepository = AspNetUserRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        public IEnumerable<AspNetUser> GetAvailableCategorys()
        {
            var list = AspNetUserRepository.GetAll();
            return list;
        }

        public IEnumerable<AspNetUser> GetAspNetUsers()
        {
            try
            {
                var list = AspNetUserRepository.GetAll();
                return list;
            }
            catch (Exception)
            {

                return null;
            }

        }

        public AspNetUser GetAspNetUserById(int AspNetUserId)
        {
            var item = AspNetUserRepository.Get(p => p.Id == AspNetUserId);
            return item;
        }

        public void CreateAspNetUser(AspNetUser AspNetUser)
        {
            if (AspNetUser != null)
            {
                AspNetUserRepository.Add(AspNetUser);
                SaveAspNetUser();
            }
        }

        public void EditAspNetUser(AspNetUser AspNetUserToEdit)
        {
            if (AspNetUserToEdit != null)
            {
                AspNetUserRepository.Update(AspNetUserToEdit);
                SaveAspNetUser();
            }
        }

        public void DeleteAspNetUser(int AspNetUserId)
        {
            var item = AspNetUserRepository.Get(p => p.Id == AspNetUserId);
            // AspNetUserRepository.Delete(item);
            item.Deleted = true;
            AspNetUserRepository.Update(item);
            SaveAspNetUser();
        }

        public void SaveAspNetUser()
        {
            unitOfWork.Commit();
        }
    }
}
