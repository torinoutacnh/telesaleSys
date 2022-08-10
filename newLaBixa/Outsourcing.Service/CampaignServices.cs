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
    public interface ICampaignService
    {
        IEnumerable<Campaign> GetAvailableCategorys();

        IEnumerable<Campaign> GetCampaigns();
        Campaign GetCampaignById(int CampaignId);
        void CreateCampaign(Campaign Campaign);
        void EditCampaign(Campaign CampaignToEdit);
        void DeleteCampaign(int CampaignId);
        void SaveCampaign();

        //Campaign GetCategoryByUrlName(string );

    }
    class CampaignService : ICampaignService
    {
        #region Field
        private readonly ICampaignRepository CampaignRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public CampaignService(ICampaignRepository CampaignRepository, IUnitOfWork unitOfWork)
        {
            this.CampaignRepository = CampaignRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        public IEnumerable<Campaign> GetAvailableCategorys()
        {
            var list = CampaignRepository.GetAll().Where(p => p.IsDeleted == true);
            return list;
        }

        public IEnumerable<Campaign> GetCampaigns()
        {
            try
            {
                var list = CampaignRepository.GetAll().Where(p => p.IsDeleted == false);
                return list;
            }
            catch (Exception)
            {

                return null;
            }

        }

        public Campaign GetCampaignById(int CampaignId)
        {
            var item = CampaignRepository.Get(p => p.Id == CampaignId);
            return item;
        }

        public void CreateCampaign(Campaign Campaign)
        {
            if (Campaign != null)
            {
                CampaignRepository.Add(Campaign);
                SaveCampaign();
            }
        }

        public void EditCampaign(Campaign CampaignToEdit)
        {
            if (CampaignToEdit != null)
            {
                CampaignRepository.Update(CampaignToEdit);
                SaveCampaign();
            }
        }

        public void DeleteCampaign(int CampaignId)
        {
            var item = CampaignRepository.Get(p => p.Id == CampaignId);
            // CampaignRepository.Delete(item);
            item.IsDeleted = true;
            CampaignRepository.Update(item);
            SaveCampaign();
        }

        public void SaveCampaign()
        {
            unitOfWork.Commit();
        }
    }
}
