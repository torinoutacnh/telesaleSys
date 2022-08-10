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
    public interface IOATemplateService
    {
        IEnumerable<OATemplate> GetOATemplates();
        IEnumerable<OATemplate> GetOATemplatesByBrandId(int BrandID);
        void CreateOATemplate(OATemplate OATemplate);
        void EditOATemplate(OATemplate OATemplateToEdit);
        void DeleteOATemplate(int OATemplateId);
        void SaveOATemplate();
        OATemplate GetOATemplateById(int OATemplateId);


    }

    public class OATemplateService : IOATemplateService
    {
        #region Field
        private readonly IOATemplateRepository OATemplateRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public OATemplateService(IOATemplateRepository OATemplateRepository, IUnitOfWork unitOfWork)
        {
            this.OATemplateRepository = OATemplateRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region Implementation for IOATemplateService
        public IEnumerable<OATemplate> GetOATemplates()
        {
            var OATemplates = OATemplateRepository.GetMany(b => !b.Deleted).OrderBy(b => b.Id);
            return OATemplates;
        }

        public OATemplate GetOATemplateById(int OATemplateId)
        {
            var OATemplate = OATemplateRepository.GetById(OATemplateId);
            return OATemplate;
        }
        public void EditOATemplate(OATemplate OATemplateToEdit)
        {
            OATemplateToEdit.DateCreated = DateTime.Now;
            OATemplateToEdit.LastEditedTime = DateTime.Now;
            OATemplateRepository.Update(OATemplateToEdit);
            SaveOATemplate();
        }


        public void CreateOATemplate(OATemplate OATemplate)
        {
            OATemplate.DateCreated = DateTime.Now;
            OATemplate.LastEditedTime = DateTime.Now;

            OATemplateRepository.Add(OATemplate);
            SaveOATemplate();
        }



        public void DeleteOATemplate(int OATemplateId)
        {
            //Get OATemplate by id.
            var OATemplate = OATemplateRepository.GetById(OATemplateId);
            if (OATemplate != null)
            {
                OATemplate.Deleted = true;
                OATemplateRepository.Update(OATemplate);
                SaveOATemplate();
            }
        }
        //


        public void SaveOATemplate()
        {
            unitOfWork.Commit();
        }

        public IEnumerable<OATemplate> GetOATemplatesByBrandId(int BrandID)
        {
            var OATemplates = OATemplateRepository.GetMany(b => !b.Deleted && b.BrandId==BrandID).OrderBy(b => b.Id);
            return OATemplates;
        }





        #endregion







    }
}
