using System.Security.Claims;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Labixa.Areas.Admin.ViewModel;
using Outsourcing.Data.Models;

namespace SocialGoal.Mappings
{

    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {

            //Mapper.CreateMap<UserFormModel, User>();
            //Mapper.CreateMap<UserFormViewModel, User>().ForMember(x => x.Id, opt => opt.MapFrom(source => source.UserId));
            //Mapper.CreateMap<XViewModel, X()
            //    .ForMember(x => x.PropertyXYZ, opt => opt.MapFrom(source => source.Property1));     
           
            Mapper.CreateMap<AspNetUserFormModel, User>();
            Mapper.CreateMap<LevelFormModel, Level>();
            Mapper.CreateMap<ChanelDataFormModel, ChanelData>();
            Mapper.CreateMap<SourceDataFormModel, SourceData>();
            Mapper.CreateMap<ScheduleFormModel, Schedule>();
            Mapper.CreateMap<CampaignFormModel, Campaign>();
            Mapper.CreateMap<UserDataMappingFormModel, UserDataMapping>();
            Mapper.CreateMap<DataUserFormModel, DataUser>();
            Mapper.CreateMap<LogDiaryFormModel, Logdiary>();
            Mapper.CreateMap<ProductCategoryFormModel, ProductCategory>();
            Mapper.CreateMap<ProductFormModel, Product>();
            Mapper.CreateMap<AlbumPhotoFormModel, Product>();
            Mapper.CreateMap<ProductAttributeFormModel, ProductAttribute>();
            Mapper.CreateMap<WardFormModel, Ward>();
            Mapper.CreateMap<DistrictFormModel, District>();
            Mapper.CreateMap<BrandFormModel, Brand>();
            Mapper.CreateMap<StoreFormModel, Store>();
            Mapper.CreateMap<OATemplateModel, OATemplate>();

        }
    }
}
