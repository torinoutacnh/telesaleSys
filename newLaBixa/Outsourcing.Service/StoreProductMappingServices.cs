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
    public interface IStoreProductMappingService
    {
        IEnumerable<StoreProductMapping> GetStoreProductMappings();
        void CreateStoreProductMapping(StoreProductMapping StoreProductMapping);
        void EditStoreProductMapping(StoreProductMapping StoreProductMappingToEdit);
        void DeleteStoreProductMapping(int StoreProductMappingId);
        void SaveStoreProductMapping();
        StoreProductMapping GetStoreProductMappingById(int StoreProductMappingId);
    }

    public class StoreProductMappingService : IStoreProductMappingService
    {
        #region Field
        private readonly IStoreProductMappingRepository StoreProductMappingRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public StoreProductMappingService(IStoreProductMappingRepository StoreProductMappingRepository, IUnitOfWork unitOfWork)
        {
            this.StoreProductMappingRepository = StoreProductMappingRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region Implementation for IStoreProductMappingService
        public IEnumerable<StoreProductMapping> GetStoreProductMappings()
        {
            var StoreProductMappings = StoreProductMappingRepository.GetMany(b => !b.IsDeleted).OrderBy(b => b.Id);
            return StoreProductMappings;
        }

        public StoreProductMapping GetStoreProductMappingById(int StoreProductMappingId)
        {
            var StoreProductMapping = StoreProductMappingRepository.GetById(StoreProductMappingId);
            return StoreProductMapping;
        }
        public void EditStoreProductMapping(StoreProductMapping StoreProductMappingToEdit)
        {
            StoreProductMappingToEdit.DateCreated = DateTime.Now;
            StoreProductMappingToEdit.LastEditedTime = DateTime.Now;
            StoreProductMappingRepository.Update(StoreProductMappingToEdit);
            SaveStoreProductMapping();
        }


        public void CreateStoreProductMapping(StoreProductMapping StoreProductMapping)
        {
            StoreProductMapping.DateCreated = DateTime.Now;
            StoreProductMapping.LastEditedTime = DateTime.Now;

            StoreProductMappingRepository.Add(StoreProductMapping);
            SaveStoreProductMapping();
        }



        public void DeleteStoreProductMapping(int StoreProductMappingId)
        {
            //Get StoreProductMapping by id.
            var StoreProductMapping = StoreProductMappingRepository.GetById(StoreProductMappingId);
            if (StoreProductMapping != null)
            {
                StoreProductMapping.IsDeleted = true;
                StoreProductMappingRepository.Update(StoreProductMapping);
                SaveStoreProductMapping();
            }
        }
        //


        public void SaveStoreProductMapping()
        {
            unitOfWork.Commit();
        }
        #endregion
    }
}
