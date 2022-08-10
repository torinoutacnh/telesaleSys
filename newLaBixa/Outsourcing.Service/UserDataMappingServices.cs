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
    public interface IUserDataMappingService
    {
        IEnumerable<UserDataMapping> GetAvailableCategorys();

        IEnumerable<UserDataMapping> GetUserDataMappings();
        UserDataMapping GetUserDataMappingById(int UserDataMappingId);
        UserDataMapping GetUserDataByDataUserId(int dataUserId);
        void CreateUserDataMapping(UserDataMapping UserDataMapping);
        UserDataMapping CreateUserDataMappingReturnItem(UserDataMapping UserDataMapping);
        void EditUserDataMapping(UserDataMapping UserDataMappingToEdit);
        void DeleteUserDataMapping(int UserDataMappingId);
        void SaveUserDataMapping();

        //UserDataMapping GetCategoryByUrlName(string );

    }
    class UserDataMappingService : IUserDataMappingService
    {
        #region Field
        private readonly IUserDataMappingRepository UserDataMappingRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public UserDataMappingService(IUserDataMappingRepository UserDataMappingRepository, IUnitOfWork unitOfWork)
        {
            this.UserDataMappingRepository = UserDataMappingRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        public IEnumerable<UserDataMapping> GetAvailableCategorys()
        {
            var list = UserDataMappingRepository.GetAll().Where(p => p.Deleted == true);
            return list;
        }

        public IEnumerable<UserDataMapping> GetUserDataMappings()
        {
            try
            {
                var list = UserDataMappingRepository.GetAll();
                return list;
            }
            catch (Exception)
            {

                return null;
            }

        }

        public UserDataMapping GetUserDataMappingById(int UserDataMappingId)
        {
            var item = UserDataMappingRepository.Get(p => p.Id == UserDataMappingId);
            return item;
        }
        public UserDataMapping GetUserDataByDataUserId(int dataUserId)
        {
            var item = UserDataMappingRepository.Get(p => p.DataUserId == dataUserId);
            return item;
        }

        public void CreateUserDataMapping(UserDataMapping UserDataMapping)
        {
            if (UserDataMapping != null)
            {
                UserDataMappingRepository.Add(UserDataMapping);
                SaveUserDataMapping();
            }
        }

        public UserDataMapping CreateUserDataMappingReturnItem(UserDataMapping UserDataMapping)
        {
            if (UserDataMapping != null)
            {
                UserDataMappingRepository.Add(UserDataMapping);
                SaveUserDataMapping();
            }

            return UserDataMapping;
        }

        public void EditUserDataMapping(UserDataMapping UserDataMappingToEdit)
        {
            if (UserDataMappingToEdit != null)
            {
                UserDataMappingRepository.Update(UserDataMappingToEdit);
                SaveUserDataMapping();
            }
        }

        public void DeleteUserDataMapping(int UserDataMappingId)
        {
            var item = UserDataMappingRepository.Get(p => p.Id == UserDataMappingId);
            // UserDataMappingRepository.Delete(item);
            item.Deleted = true;
            UserDataMappingRepository.Update(item);
            SaveUserDataMapping();
        }

        public void SaveUserDataMapping()
        {
            unitOfWork.Commit();
        }
    }
}
