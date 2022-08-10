using Outsourcing.Core.Common;
using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Models;
using Outsourcing.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Service
{
    public interface ISurveyService
    {

        IEnumerable<Survey> GetSurveys();
        Survey GetSurveyById(int SurveyId);
        void CreateSurvey(Survey Survey);
        void EditSurvey(Survey SurveyToEdit);
        void DeleteSurvey(int SurveyId);
        void SaveSurvey();
        IEnumerable<ValidationResult> CanAddSurvey(Survey Survey);

    }
    public class SurveyService : ISurveyService
    {
        #region Field
        private readonly ISurveyRepository SurveyRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public SurveyService(ISurveyRepository SurveyRepository, IUnitOfWork unitOfWork)
        {
            this.SurveyRepository = SurveyRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region BaseMethod

        public IEnumerable<Survey> GetSurveys()
        {
            var Surveys = SurveyRepository.GetAll();
            return Surveys;
        }

        public Survey GetSurveyById(int SurveyId)
        {
            var Survey = SurveyRepository.GetById(SurveyId);
            return Survey;
        }

        public void CreateSurvey(Survey Survey)
        {
            SurveyRepository.Add(Survey);
            SaveSurvey();
        }

        public void EditSurvey(Survey SurveyToEdit)
        {
            SurveyRepository.Update(SurveyToEdit);
            SaveSurvey();
        }

        public void DeleteSurvey(int SurveyId)
        {
            //Get Survey by id.
            var Survey = SurveyRepository.GetById(SurveyId);
            if (Survey != null)
            {
                SurveyRepository.Delete(Survey);
                SaveSurvey();
            }
        }

        public void SaveSurvey()
        {
            unitOfWork.Commit();
        }

        public IEnumerable<ValidationResult> CanAddSurvey(Survey Survey)
        {

            //    yield return new ValidationResult("Survey", "ErrorString");
            return null;
        }

        #endregion
    }
}
