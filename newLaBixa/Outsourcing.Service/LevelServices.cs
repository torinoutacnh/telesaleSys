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

    public interface ILevelService
    {
        IEnumerable<Level> GetLevels();
        void CreateLevel(Level Level);
        void EditLevel(Level LevelToEdit);
        void DeleteLevel(int LevelId);
        void SaveLevel();
        Level GetLevelById(int LevelId);


    }

    public class LevelService : ILevelService
    {
        #region Field
        private readonly ILevelRepository LevelRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public LevelService(ILevelRepository LevelRepository, IUnitOfWork unitOfWork)
        {
            this.LevelRepository = LevelRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region Implementation for ILevelService
        public IEnumerable<Level> GetLevels()
        {
            var Levels = LevelRepository.GetMany(b => !b.Deleted).OrderBy(b => b.Id);
            return Levels;
        }

        public Level GetLevelById(int LevelId)
        {
            var Level = LevelRepository.GetById(LevelId);
            return Level;
        }
        public void EditLevel(Level LevelToEdit)
        {
            LevelToEdit.DateCreated = DateTime.Now;
            LevelToEdit.LastEditedTime = DateTime.Now;
            LevelRepository.Update(LevelToEdit);
            SaveLevel();
        }


        public void CreateLevel(Level Level)
        {
            Level.DateCreated = DateTime.Now;
            Level.LastEditedTime = DateTime.Now;

            LevelRepository.Add(Level);
            SaveLevel();
        }



        public void DeleteLevel(int LevelId)
        {
            //Get Level by id.
            var Level = LevelRepository.GetById(LevelId);
            if (Level != null)
            {
                Level.Deleted = true;
                LevelRepository.Update(Level);
                SaveLevel();
            }
        }
        //


        public void SaveLevel()
        {
            unitOfWork.Commit();
        }





        #endregion







    }
}
