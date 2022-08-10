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

    public interface IChanelDataService
    {
        IEnumerable<ChanelData> GetChanelDatas();
        void CreateChanelData(ChanelData ChanelData);
        void EditChanelData(ChanelData ChanelDataToEdit);
        void DeleteChanelData(int ChanelDataId);
        void SaveChanelData();
        ChanelData GetChanelDataById(int ChanelDataId);


    }

    public class ChanelDataService : IChanelDataService
    {
        #region Field
        private readonly IChanelDataRepository ChanelDataRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public ChanelDataService(IChanelDataRepository ChanelDataRepository, IUnitOfWork unitOfWork)
        {
            this.ChanelDataRepository = ChanelDataRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region Implementation for IChanelDataService
        public IEnumerable<ChanelData> GetChanelDatas()
        {
            var ChanelDatas = ChanelDataRepository.GetMany(b => !b.Deleted).OrderBy(b => b.Id);
            return ChanelDatas;
        }

        public ChanelData GetChanelDataById(int ChanelDataId)
        {
            var ChanelData = ChanelDataRepository.GetById(ChanelDataId);
            return ChanelData;
        }
        public void EditChanelData(ChanelData ChanelDataToEdit)
        {
            ChanelDataToEdit.DateCreated = DateTime.Now;
            ChanelDataToEdit.LastEditedTime = DateTime.Now;
            ChanelDataRepository.Update(ChanelDataToEdit);
            SaveChanelData();
        }


        public void CreateChanelData(ChanelData ChanelData)
        {
            ChanelData.DateCreated = DateTime.Now;
            ChanelData.LastEditedTime = DateTime.Now;

            ChanelDataRepository.Add(ChanelData);
            SaveChanelData();
        }



        public void DeleteChanelData(int ChanelDataId)
        {
            //Get ChanelData by id.
            var ChanelData = ChanelDataRepository.GetById(ChanelDataId);
            if (ChanelData != null)
            {
                ChanelData.Deleted = true;
                ChanelDataRepository.Update(ChanelData);
                SaveChanelData();
            }
        }
        //


        public void SaveChanelData()
        {
            unitOfWork.Commit();
        }





        #endregion







    }
}
