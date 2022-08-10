using Invedia.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Repository.Infrastructure;
using XProject.Contract.Repository.Interface;
using XProject.Contract.Repository.Models;
using XProject.Contract.Service;

namespace XProject.Service
{

    [ScopedDependency(ServiceType = typeof(ISocialUserService))]

    public class SocialUserService : Base.Service, ISocialUserService
    {
        private readonly ISocialUserRepository _SocialUserRepository;
        private readonly IUnitOfWork _uof;
        //private readonly IUniof _SocialUserRepository;

        public SocialUserService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _SocialUserRepository = serviceProvider.GetRequiredService<ISocialUserRepository>();
            _uof = serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        public SocialUserEntity Create(SocialUserEntity SocialUserToCreate)
        {
            _SocialUserRepository.Add(SocialUserToCreate);
            _uof.SaveChanges();
            return SocialUserToCreate;
        }

        public void Delete(string id)
        {
            var all = _SocialUserRepository.Get(x => x.Id == id).FirstOrDefault();
            if (all == null)
            {
                return;
            }
            _SocialUserRepository.Delete(all);
            _uof.SaveChanges();
        }

        public List<SocialUserEntity> GetAll()
        {
            var all = _SocialUserRepository.Get().ToList();
            return all;
        }

        public SocialUserEntity GetSocialUserByName(string name)
        {
            var all = _SocialUserRepository.Get(x => x.Name == name).FirstOrDefault();
            return all;
        }

        public SocialUserEntity GetById(string SocialUser_id)
        {
            var all = _SocialUserRepository.Get(x => x.Id == SocialUser_id).FirstOrDefault();
            return all;
        }

        public SocialUserEntity Update(SocialUserEntity SocialUserToEdit)
        {
            _SocialUserRepository.Update(SocialUserToEdit);
            _uof.SaveChanges();
            return SocialUserToEdit;
        }
    }
}
