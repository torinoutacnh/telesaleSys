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

    public interface IStoreService
    {
        IEnumerable<Store> GetStores();
        void CreateStore(Store Store);
        void EditStore(Store StoreToEdit);
        void DeleteStore(int StoreId);
        void SaveStore();
        Store GetStoreById(int StoreId);
        Store CreateStoreReturnId(Store Store);


    }

    public class StoreService : IStoreService
    {
        #region Field
        private readonly IStoreRepository StoreRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public StoreService(IStoreRepository StoreRepository, IUnitOfWork unitOfWork)
        {
            this.StoreRepository = StoreRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region Implementation for IStoreService
        public IEnumerable<Store> GetStores()
        {
            var Stores = StoreRepository.GetAll().OrderBy(b => b.Id).Where(n=>n.IsDelete == false);
            return Stores;
        }

        public Store GetStoreById(int StoreId)
        {
            var Store = StoreRepository.GetById(StoreId);
            return Store;
        }
        public void EditStore(Store StoreToEdit)
        {
            StoreToEdit.DateCreated = DateTime.Now;
            StoreToEdit.LastEditedTime = DateTime.Now;
            StoreRepository.Update(StoreToEdit);
            SaveStore();
        }


        public void CreateStore(Store Store)
        {
            Store.DateCreated = DateTime.Now;
            Store.LastEditedTime = DateTime.Now;

            StoreRepository.Add(Store);
            SaveStore();
        }
        public Store CreateStoreReturnId(Store Store)
        {
            Store.DateCreated = DateTime.Now;
            Store.LastEditedTime = DateTime.Now;

            StoreRepository.Add(Store);
            SaveStore();

            return Store;
        }


        public void DeleteStore(int StoreId)
        {
            //Get Store by id.
            var Store = StoreRepository.GetById(StoreId);
            if (Store != null)
            {
                Store.IsDelete = true;
                StoreRepository.Update(Store);
                SaveStore();
            }
        }
        //


        public void SaveStore()
        {
            unitOfWork.Commit();
        }





        #endregion







    }
}
