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
    public interface  IOAZaloService
    {
        IEnumerable<OAZalo> GetOAZalo();
        IEnumerable<OAZalo> GetOAZaloByBrandId(int BrandID);
        void CreateOATZalo(OAZalo OAZalo);
        void EditOAZalo(OAZalo OAZaloToEdit);
        void DeleteOAZalo(int OA_Id);
        void SaveOAZalo();
        OAZalo GetOAZaloById(int OA_Id);

    }

    public class OAZaloService : IOAZaloService
    {
        #region Field
        private readonly IOAZaloRepositroy OAZaloRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public OAZaloService(IOAZaloRepositroy OAZaloRepository, IUnitOfWork unitOfWork)
        {
            this.OAZaloRepository = OAZaloRepository;
            this.unitOfWork = unitOfWork;
        }


        #endregion

        #region Implementation for IOAZaloService
        public IEnumerable<OAZalo> GetOAZalo()
        {
            var OAZalo = OAZaloRepository.GetMany(b => !b.Deleted).OrderBy(b => b.Id);
            return OAZalo;
        }

        public IEnumerable<OAZalo> GetOAZaloByBrandId(int BrandID)
        {
            var OAZalo = OAZaloRepository.GetMany(b => !b.Deleted && b.Brand_Id == BrandID).OrderBy(b => b.Id);
            return OAZalo;
        }

        public OAZalo GetOAZaloById(int OA_Id)
        {
            var OAZalo = OAZaloRepository.GetById(OA_Id);
            return OAZalo;
        }
        public void CreateOATZalo(OAZalo OAZalo)
        {
            OAZalo.DateCreated = DateTime.Now;

            OAZaloRepository.Add(OAZalo);
            SaveOAZalo();
        }

        public void EditOAZalo(OAZalo OAZaloToEdit)
        {
            OAZaloToEdit.DateCreated = DateTime.Now;
            OAZaloRepository.Update(OAZaloToEdit);
            SaveOAZalo();
        }
        public void DeleteOAZalo(int OA_Id)
        {
            //Get OAZalo by id.
            var OAZalo = OAZaloRepository.GetById(OA_Id);
            if (OAZalo != null)
            {
                OAZalo.Deleted = true;
                OAZaloRepository.Update(OAZalo);
                SaveOAZalo();
            }
        }
        public void SaveOAZalo()
        {
            unitOfWork.Commit();
        }
        #endregion
    }
}
