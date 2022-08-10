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

    public interface ISourceDataService
    {
        IEnumerable<SourceData> GetSourceDatas();
        void CreateSourceData(SourceData SourceData);
        void EditSourceData(SourceData SourceDataToEdit);
        void DeleteSourceData(int SourceDataId);
        void SaveSourceData();
        SourceData GetSourceDataById(int SourceDataId);


    }

    public class SourceDataService : ISourceDataService
    {
        #region Field
        private readonly ISourceDataRepository SourceDataRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public SourceDataService(ISourceDataRepository SourceDataRepository, IUnitOfWork unitOfWork)
        {
            this.SourceDataRepository = SourceDataRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region Implementation for ISourceDataService
        public IEnumerable<SourceData> GetSourceDatas()
        {
            var SourceDatas = SourceDataRepository.GetMany(b => !b.Deleted).OrderBy(b => b.Id);
            return SourceDatas;
        }

        public SourceData GetSourceDataById(int SourceDataId)
        {
            var SourceData = SourceDataRepository.GetById(SourceDataId);
            return SourceData;
        }
        public void EditSourceData(SourceData SourceDataToEdit)
        {
            SourceDataToEdit.DateCreated = DateTime.Now;
            SourceDataToEdit.LastEditedTime = DateTime.Now;
            SourceDataRepository.Update(SourceDataToEdit);
            SaveSourceData();
        }


        public void CreateSourceData(SourceData SourceData)
        {
            SourceData.DateCreated = DateTime.Now;
            SourceData.LastEditedTime = DateTime.Now;

            SourceDataRepository.Add(SourceData);
            SaveSourceData();
        }



        public void DeleteSourceData(int SourceDataId)
        {
            //Get SourceData by id.
            var SourceData = SourceDataRepository.GetById(SourceDataId);
            if (SourceData != null)
            {
                SourceData.Deleted = true;
                SourceDataRepository.Update(SourceData);
                SaveSourceData();
            }
        }
        //


        public void SaveSourceData()
        {
            unitOfWork.Commit();
        }





        #endregion







    }
}
