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
    public interface ISchedulesStoreMappingService
    {
        IEnumerable<SchedulesStoreMapping> GetAvailableCategorys();

        IEnumerable<SchedulesStoreMapping> GetSchedulesStoreMappings();
        SchedulesStoreMapping GetSchedulesStoreMappingById(int SchedulesStoreMappingId);
        void CreateSchedulesStoreMapping(SchedulesStoreMapping SchedulesStoreMapping);
        void EditSchedulesStoreMapping(SchedulesStoreMapping SchedulesStoreMappingToEdit);
        void DeleteSchedulesStoreMapping(int SchedulesStoreMappingId);
        void SaveSchedulesStoreMapping();

        //SchedulesStoreMapping GetCategoryByUrlName(string );

    }
    class SchedulesStoreMappingService : ISchedulesStoreMappingService
    {
        #region Field
        private readonly ISchedulesStoreMappingRepository SchedulesStoreMappingRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public SchedulesStoreMappingService(ISchedulesStoreMappingRepository SchedulesStoreMappingRepository, IUnitOfWork unitOfWork)
        {
            this.SchedulesStoreMappingRepository = SchedulesStoreMappingRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        public IEnumerable<SchedulesStoreMapping> GetAvailableCategorys()
        {
            var list = SchedulesStoreMappingRepository.GetAll();//.Where(p => p.Deleted == true)
            return list;
        }

        public IEnumerable<SchedulesStoreMapping> GetSchedulesStoreMappings()
        {
            try
            {
                var list = SchedulesStoreMappingRepository.GetAll();//.Where(p => p.Deleted == false)
                return list;
            }
            catch (Exception)
            {

                return null;
            }

        }

        public SchedulesStoreMapping GetSchedulesStoreMappingById(int SchedulesStoreMappingId)
        {
            var item = SchedulesStoreMappingRepository.Get(p => p.Id == SchedulesStoreMappingId);
            return item;
        }

        public void CreateSchedulesStoreMapping(SchedulesStoreMapping SchedulesStoreMapping)
        {
            if (SchedulesStoreMapping != null)
            {
                SchedulesStoreMappingRepository.Add(SchedulesStoreMapping);
                SaveSchedulesStoreMapping();
            }
        }

        public void EditSchedulesStoreMapping(SchedulesStoreMapping SchedulesStoreMappingToEdit)
        {
            if (SchedulesStoreMappingToEdit != null)
            {
                SchedulesStoreMappingRepository.Update(SchedulesStoreMappingToEdit);
                SaveSchedulesStoreMapping();
            }
        }

        public void DeleteSchedulesStoreMapping(int SchedulesStoreMappingId)
        {
            var item = SchedulesStoreMappingRepository.Get(p => p.Id == SchedulesStoreMappingId);
            // SchedulesStoreMappingRepository.Delete(item);
            //item.Deleted = true;
            SchedulesStoreMappingRepository.Update(item);
            SaveSchedulesStoreMapping();
        }

        public void SaveSchedulesStoreMapping()
        {
            unitOfWork.Commit();
        }
    }
}

