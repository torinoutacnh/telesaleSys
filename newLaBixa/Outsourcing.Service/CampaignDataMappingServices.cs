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
    public interface ICampaignDataMappingService
    {
        IEnumerable<CampaignDataMapping> GetAvailableCategorys();

        IEnumerable<CampaignDataMapping> GetCampaignDataMappings();
        CampaignDataMapping GetCampaignDataMappingById(int CampaignDataMappingId);
        void CreateCampaignDataMapping(CampaignDataMapping CampaignDataMapping);
        void EditCampaignDataMapping(CampaignDataMapping CampaignDataMappingToEdit);
        void DeleteCampaignDataMapping(int CampaignDataMappingId);
        void SaveCampaignDataMapping();

        //CampaignDataMapping GetCategoryByUrlName(string );

    }
    class CampaignDataMappingService : ICampaignDataMappingService
    {
        #region Field
        private readonly ICampaignDataMappingRepository CampaignDataMappingRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public CampaignDataMappingService(ICampaignDataMappingRepository CampaignDataMappingRepository, IUnitOfWork unitOfWork)
        {
            this.CampaignDataMappingRepository = CampaignDataMappingRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        public IEnumerable<CampaignDataMapping> GetAvailableCategorys()
        {
            var list = CampaignDataMappingRepository.GetAll().Where(p => p.IsDeleted == true);
            return list;
        }

        public IEnumerable<CampaignDataMapping> GetCampaignDataMappings()
        {
            try
            {
                var list = CampaignDataMappingRepository.GetAll().Where(p => p.IsDeleted == false);
                return list;
            }
            catch (Exception)
            {

                return null;
            }

        }

        public CampaignDataMapping GetCampaignDataMappingById(int CampaignDataMappingId)
        {
            var item = CampaignDataMappingRepository.Get(p => p.Id == CampaignDataMappingId);
            return item;
        }

        public void CreateCampaignDataMapping(CampaignDataMapping CampaignDataMapping)
        {
            if (CampaignDataMapping != null)
            {
                CampaignDataMappingRepository.Add(CampaignDataMapping);
                SaveCampaignDataMapping();
            }
        }

        public void EditCampaignDataMapping(CampaignDataMapping CampaignDataMappingToEdit)
        {
            if (CampaignDataMappingToEdit != null)
            {
                CampaignDataMappingRepository.Update(CampaignDataMappingToEdit);
                SaveCampaignDataMapping();
            }
        }

        public void DeleteCampaignDataMapping(int CampaignDataMappingId)
        {
            var item = CampaignDataMappingRepository.Get(p => p.Id == CampaignDataMappingId);
            // CampaignDataMappingRepository.Delete(item);
            item.IsDeleted = true;
            CampaignDataMappingRepository.Update(item);
            SaveCampaignDataMapping();
        }

        public void SaveCampaignDataMapping()
        {
            unitOfWork.Commit();
        }
    }
}
