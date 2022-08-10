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
    public interface IConfiguationService
    {
        //IEnumerable<Configuation> GetConfiguations();
        void CreateConfiguation(Configuation Configuation);
        //void EditConfiguation(Configuation ConfiguationToEdit);
        void DeleteConfiguation(int ConfiguationId);
        void SaveConfiguation();
        //Configuation GetConfiguationById(int ConfiguationId);


    }

    public class ConfiguationService : IConfiguationService
    {
        #region Field
        private readonly IConfiguationRepository ConfiguationRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public ConfiguationService(IConfiguationRepository ConfiguationRepository, IUnitOfWork unitOfWork)
        {
            this.ConfiguationRepository = ConfiguationRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region Implementation for IConfiguationService
        //public IEnumerable<Configuation> GetConfiguations()
        //{
        //    var Configuations = ConfiguationRepository.GetMany(b => !b.IsDeleted).OrderBy(b => b.Id);
        //    return Configuations;
        //}

        //public Configuation GetConfiguationById(int ConfiguationId)
        //{
        //    var Configuation = ConfiguationRepository.GetById(ConfiguationId);
        //    return Configuation;
        //}
        //public void EditConfiguation(Configuation ConfiguationToEdit)
        //{  
        //    ConfiguationRepository.Update(ConfiguationToEdit);
        //    SaveConfiguation();
        //}


        public void CreateConfiguation(Configuation Configuation)
        {

            ConfiguationRepository.Add(Configuation);
            SaveConfiguation();
        }



        public void DeleteConfiguation(int ConfiguationId)
        {
            //Get Configuation by id.
            var Configuation = ConfiguationRepository.GetById(ConfiguationId);
            if (Configuation != null)
            {
                Configuation.IsDeleted = true;
                ConfiguationRepository.Update(Configuation);
                SaveConfiguation();
            }
        }
        //


        public void SaveConfiguation()
        {
            unitOfWork.Commit();
        }
        #endregion
    }
}
