using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Repository.Models;

namespace XProject.Contract.Service
{
    public interface ISocialUserService
    {
        List<SocialUserEntity> GetAll();
        SocialUserEntity GetById(string SocialUser_id);
        SocialUserEntity GetSocialUserByName(string name);
        SocialUserEntity Create(SocialUserEntity SocialUserToCreate);
        SocialUserEntity Update(SocialUserEntity SocialUserToEdit);
        void Delete(string id);
    }
}
