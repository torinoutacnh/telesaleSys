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
    public interface IBrandService
    {
        IEnumerable<Brand> GetBrands();
        void CreateBrand(Brand Brand);
        void EditBrand(Brand BrandToEdit);
        void DeleteBrand(int BrandId);
        void SaveBrand();
        Brand GetBrandById(int BrandId);


    }

    public class BrandService : IBrandService
    {
        #region Field
        private readonly IBrandRepository BrandRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public BrandService(IBrandRepository BrandRepository, IUnitOfWork unitOfWork)
        {
            this.BrandRepository = BrandRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region Implementation for IBrandService
        public IEnumerable<Brand> GetBrands()
        {
            var Brands = BrandRepository.GetMany(b => !b.IsDeleted).OrderBy(b => b.Id);
            return Brands;
        }

        public Brand GetBrandById(int BrandId)
        {
            var Brand = BrandRepository.GetById(BrandId);
            return Brand;
        }
        public void EditBrand(Brand BrandToEdit)
        {
            BrandToEdit.DateCreated = DateTime.Now;
            BrandToEdit.LastEditedTime = DateTime.Now;
            BrandRepository.Update(BrandToEdit);
            SaveBrand();
        }


        public void CreateBrand(Brand Brand)
        {
            Brand.DateCreated = DateTime.Now;
            Brand.LastEditedTime = DateTime.Now;

            BrandRepository.Add(Brand);
            SaveBrand();
        }



        public void DeleteBrand(int BrandId)
        {
            //Get Brand by id.
            var Brand = BrandRepository.GetById(BrandId);
            if (Brand != null)
            {
                Brand.IsDeleted = true;
                BrandRepository.Update(Brand);
                SaveBrand();
            }
        }
        //


        public void SaveBrand()
        {
            unitOfWork.Commit();
        }
        #endregion
    }
}
