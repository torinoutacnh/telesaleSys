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
    public interface IWardService
    {
        IEnumerable<Ward> GetWards();
        void CreateWard(Ward Ward);
        void EditWard(Ward WardToEdit);
        void DeleteWard(int WardId);
        void SaveWard();
        Ward GetWardById(int WardId);
    }

    public class WardService : IWardService
    {
        #region Field
        private readonly IWardRepository WardRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public WardService(IWardRepository WardRepository, IUnitOfWork unitOfWork)
        {
            this.WardRepository = WardRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region Implementation for IWardService
        public IEnumerable<Ward> GetWards()
        {
            var Wards = WardRepository.GetMany(b => !b.IsDeleted).OrderBy(b => b.Id);
            return Wards;
        }

        public Ward GetWardById(int WardId)
        {
            var Ward = WardRepository.GetById(WardId);
            return Ward;
        }
        public void EditWard(Ward WardToEdit)
        {
            WardToEdit.DateCreated = DateTime.Now;
            WardToEdit.LastEditedTime = DateTime.Now;
            WardRepository.Update(WardToEdit);
            SaveWard();
        }


        public void CreateWard(Ward Ward)
        {
            Ward.DateCreated = DateTime.Now;
            Ward.LastEditedTime = DateTime.Now;

            WardRepository.Add(Ward);
            SaveWard();
        }



        public void DeleteWard(int WardId)
        {
            //Get Ward by id.
            var Ward = WardRepository.GetById(WardId);
            if (Ward != null)
            {
                Ward.IsDeleted = true;
                WardRepository.Update(Ward);
                SaveWard();
            }
        }
        //


        public void SaveWard()
        {
            unitOfWork.Commit();
        }
        #endregion
    }
}
