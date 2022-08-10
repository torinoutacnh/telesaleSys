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
    public interface IDataUserService
    {
        IEnumerable<DataUser> GetAvailableCategorys();

        IEnumerable<DataUser> GetDataUsers();
        DataUser GetDataUserById(int BrandId);

        DataUser GetDataUserByBrandId(int UserBrandID);
        DataUser GetDataUserByUserId(int UserId);
        DataUser GetDataUserByDate(int UserId, DateTime date);
        DataUser GetDataUserByLevelId(int LevelId);


        void CreateDataUser(DataUser DataUser);
        void EditDataUser(DataUser DataUserToEdit);
        void DeleteDataUser(int DataUserId);
        void SaveDataUser();

        //DataUser GetCategoryByUrlName(string );

    }
    class DataUserService : IDataUserService
    {
        #region Field
        private readonly IDataUserRepository DataUserRepository;
        private readonly IUserDataMappingRepository UserDataMappingsRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public DataUserService(IDataUserRepository DataUserRepository, IUnitOfWork unitOfWork, IUserDataMappingRepository UserDataMappingsRepository)
        {
            this.DataUserRepository = DataUserRepository;
            this.UserDataMappingsRepository = UserDataMappingsRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion  

        public IEnumerable<DataUser> GetAvailableCategorys()
        {
            var list = DataUserRepository.GetAll().Where(p => p.Deleted == true);
            return list;
        }

        public IEnumerable<DataUser> GetDataUsers()
        {
            try
            {
                var list = DataUserRepository.GetAll().Where(p => p.Deleted == false);
                return list;
            }
            catch (Exception)
            {

                return null;
            }

        }

        public DataUser GetDataUserById(int DataUserId)
        {
            var item = DataUserRepository.Get(p => p.Id == DataUserId);
            return item;
        }

        public void CreateDataUser(DataUser DataUser)
        {
            if (DataUser != null)
            {
                DataUserRepository.Add(DataUser);
                SaveDataUser();
            }
        }

        public void EditDataUser(DataUser DataUserToEdit)
        {
            if (DataUserToEdit != null)
            {
                DataUserRepository.Update(DataUserToEdit);
                SaveDataUser();
            }
        }

        public void DeleteDataUser(int DataUserId)
        {
            var item = DataUserRepository.Get(p => p.Id == DataUserId);
            // DataUserRepository.Delete(item);
            item.Deleted = true;
            DataUserRepository.Update(item);
            SaveDataUser();
        }

        public void SaveDataUser()
        {
            unitOfWork.Commit();
        }
        #region lay user theo brand Id
        public DataUser GetDataUserByBrandId(int UserBrandID)
        {
            var userBrandId = DataUserRepository.Get(p=>p.BrandId == UserBrandID);

            return userBrandId;
        }
        #endregion

        #region lay user theo userId
        public DataUser GetDataUserByUserId(int UserId)
        {
            var userFromUserId = DataUserRepository.Get(p => p.Id == UserId);
            return userFromUserId;
        }
        #endregion

        #region lay user da goi trong ngay
        public DataUser GetDataUserByDate(int UserId, DateTime date)
        {
            var userInDay = DataUserRepository.Get(p=>p.LastCall == date);
            throw new NotImplementedException();
        }
        #endregion
        #region lay user theo level Id
        public DataUser GetDataUserByLevelId(int LevelId)
        {
            var userLevelId = DataUserRepository.Get(p => p.LevelId == LevelId);
            return userLevelId;
        }
        #endregion
    }
}
