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
   
    public interface IQuestionService
    {

        IEnumerable<Question> GetQuestions();
        Question GetQuestionById(int QuestionId);
        void CreateQuestion(Question Question);
        void EditQuestion(Question QuestionToEdit);
        void DeleteQuestion(int QuestionId);
        void SaveQuestion();
        IEnumerable<ValidationResult> CanAddQuestion(Question Question);

    }
    public class QuestionService : IQuestionService
    {
        #region Field
        private readonly IQuestionRepository QuestionRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public QuestionService(IQuestionRepository QuestionRepository, IUnitOfWork unitOfWork)
        {
            this.QuestionRepository = QuestionRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region BaseMethod

        public IEnumerable<Question> GetQuestions()
        {
            var Questions = QuestionRepository.GetAll();
            return Questions;
        }

        public Question GetQuestionById(int QuestionId)
        {
            var Question = QuestionRepository.GetById(QuestionId);
            return Question;
        }

        public void CreateQuestion(Question Question)
        {
            QuestionRepository.Add(Question);
            SaveQuestion();
        }

        public void EditQuestion(Question QuestionToEdit)
        {
            QuestionRepository.Update(QuestionToEdit);
            SaveQuestion();
        }

        public void DeleteQuestion(int QuestionId)
        {
            //Get Question by id.
            var Question = QuestionRepository.GetById(QuestionId);
            if (Question != null)
            {
                QuestionRepository.Delete(Question);
                SaveQuestion();
            }
        }

        public void SaveQuestion()
        {
            unitOfWork.Commit();
        }

        public IEnumerable<ValidationResult> CanAddQuestion(Question Question)
        {

            //    yield return new ValidationResult("Question", "ErrorString");
            return null;
        }

        #endregion
    }
}
