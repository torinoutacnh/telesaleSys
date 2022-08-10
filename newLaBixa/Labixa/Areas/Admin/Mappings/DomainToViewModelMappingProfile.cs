using AutoMapper;
using Labixa.Areas.Admin.ViewModel;
using Outsourcing.Data.Models;

using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialGoal.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {

            //Mapper.CreateMap<UserProfile, UserProfileFormModel>();

            //Mapper.CreateMap<X, XViewModel>()
            //    .ForMember(x => x.Property1, opt => opt.MapFrom(source => source.PropertyXYZ));
            //Mapper.CreateMap<Goal, GoalListViewModel>().ForMember(x => x.SupportsCount, opt => opt.MapFrom(source => source.Supports.Count))
            //                                          .ForMember(x => x.UserName, opt => opt.MapFrom(source => source.User.UserName))
            //                                          .ForMember(x => x.StartDate, opt => opt.MapFrom(source => source.StartDate.ToString("dd MMM yyyy")))
            //                                          .ForMember(x => x.EndDate, opt => opt.MapFrom(source => source.EndDate.ToString("dd MMM yyyy")));
            //Mapper.CreateMap<Group, GroupsItemViewModel>().ForMember(x => x.CreatedDate, opt => opt.MapFrom(source => source.CreatedDate.ToString("dd MMM yyyy")));

            //Mapper.CreateMap<IPagedList<Group>, IPagedList<GroupsItemViewModel>>().ConvertUsing<PagedListConverter<Group, GroupsItemViewModel>>();

        
            Mapper.CreateMap<User, AspNetUserFormModel>();
            Mapper.CreateMap<Level, LevelFormModel>();
            Mapper.CreateMap<ChanelData, ChanelDataFormModel>();
            Mapper.CreateMap<SourceData, SourceDataFormModel>();
            Mapper.CreateMap<Schedule, ScheduleFormModel>();
            Mapper.CreateMap<Campaign, CampaignFormModel>();
            Mapper.CreateMap<UserDataMapping, UserDataMappingFormModel>();
            Mapper.CreateMap<DataUser, DataUserFormModel>();
            Mapper.CreateMap<Logdiary, LogDiaryFormModel>();
            Mapper.CreateMap<ProductCategory, ProductCategoryFormModel>();
            Mapper.CreateMap<Product, ProductFormModel>();
            Mapper.CreateMap<Product, AlbumPhotoFormModel>();
            Mapper.CreateMap<ProductAttribute, ProductAttributeFormModel>();
            Mapper.CreateMap<Ward, WardFormModel>();
            Mapper.CreateMap<District, DistrictFormModel>();
            Mapper.CreateMap<Brand, BrandFormModel>();
            Mapper.CreateMap<Store, StoreFormModel>();
            Mapper.CreateMap<OATemplate, OATemplateModel>();
        }
    }
}