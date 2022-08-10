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
    public interface IDistrictService
    {
        IEnumerable<District> GetDistricts();
        void CreateDistrict(District District);
        void EditDistrict(District DistrictToEdit);
        void DeleteDistrict(int DistrictId);
        void SaveDistrict();
        District GetDistrictById(int DistrictId);


    }

    public class DistrictService : IDistrictService
    {
        #region Field
        private readonly IDistrictRepository DistrictRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public DistrictService(IDistrictRepository DistrictRepository, IUnitOfWork unitOfWork)
        {
            this.DistrictRepository = DistrictRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region Implementation for IDistrictService
        public IEnumerable<District> GetDistricts()
        {
            var Districts = DistrictRepository.GetMany(b => !b.IsDeleted).OrderBy(b => b.Id);
            return Districts;
        }

        public District GetDistrictById(int DistrictId)
        {
            var District = DistrictRepository.GetById(DistrictId);
            return District;
        }
        public void EditDistrict(District DistrictToEdit)
        {
            DistrictToEdit.DateCreated = DateTime.Now;
            DistrictToEdit.LastEditedTime = DateTime.Now;
            DistrictRepository.Update(DistrictToEdit);
            SaveDistrict();
        }


        public void CreateDistrict(District District)
        {
            District.DateCreated = DateTime.Now;
            District.LastEditedTime = DateTime.Now;

            DistrictRepository.Add(District);
            SaveDistrict();
        }



        public void DeleteDistrict(int DistrictId)
        {
            //Get District by id.
            var District = DistrictRepository.GetById(DistrictId);
            if (District != null)
            {
                District.IsDeleted = true;
                DistrictRepository.Update(District);
                SaveDistrict();
            }
        }
        //


        public void SaveDistrict()
        {
            unitOfWork.Commit();
        }
        #endregion
    }
}
