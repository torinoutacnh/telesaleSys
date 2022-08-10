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
    public interface IOATemplateAttributeService
    {
        IEnumerable<OATemplateAttribute> GetOATemplateAttributes();
        void CreateOATemplateAttribute(OATemplateAttribute OATemplateAttribute);
        void EditOATemplateAttribute(OATemplateAttribute OATemplateAttributeToEdit);
        void DeleteOATemplateAttribute(int OATemplateAttributeId);
        void SaveOATemplateAttribute();
        OATemplateAttribute GetOATemplateAttributeById(int OATemplateAttributeId);


    }

    public class OATemplateAttributeService : IOATemplateAttributeService
    {
        #region Field
        private readonly IOATemplateAttributeRepository OATemplateAttributeRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public OATemplateAttributeService(IOATemplateAttributeRepository OATemplateAttributeRepository, IUnitOfWork unitOfWork)
        {
            this.OATemplateAttributeRepository = OATemplateAttributeRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region Implementation for IOATemplateAttributeService
        public IEnumerable<OATemplateAttribute> GetOATemplateAttributes()
        {
            var OATemplateAttributes = OATemplateAttributeRepository.GetMany(b => !b.isDeleted).OrderBy(b => b.Id);
            return OATemplateAttributes;
        }

        public OATemplateAttribute GetOATemplateAttributeById(int OATemplateAttributeId)
        {
            var OATemplateAttribute = OATemplateAttributeRepository.GetById(OATemplateAttributeId);
            return OATemplateAttribute;
        }
        public void EditOATemplateAttribute(OATemplateAttribute OATemplateAttributeToEdit)
        {
            OATemplateAttributeToEdit.DateCreated = DateTime.Now;
            OATemplateAttributeRepository.Update(OATemplateAttributeToEdit);
            SaveOATemplateAttribute();
        }


        public void CreateOATemplateAttribute(OATemplateAttribute OATemplateAttribute)
        {
            OATemplateAttribute.DateCreated = DateTime.Now;

            OATemplateAttributeRepository.Add(OATemplateAttribute);
            SaveOATemplateAttribute();
        }



        public void DeleteOATemplateAttribute(int OATemplateAttributeId)
        {
            //Get OATemplateAttribute by id.
            var OATemplateAttribute = OATemplateAttributeRepository.GetById(OATemplateAttributeId);
            if (OATemplateAttribute != null)
            {
                OATemplateAttribute.isDeleted = true;
                OATemplateAttributeRepository.Update(OATemplateAttribute);
                SaveOATemplateAttribute();
            }
        }
        //


        public void SaveOATemplateAttribute()
        {
            unitOfWork.Commit();
        }





        #endregion







    }
}
