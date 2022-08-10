using System.Collections.Generic;
using System.Linq;
using Outsourcing.Data.Repository;
using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Models;
using Outsourcing.Core.Common;
using System;

namespace Outsourcing.Service
{

    public interface IUserService
    {
        IEnumerable<User> Getusers();
        void Saveuser();
        User GetuserById(int userId);
    }

    public class UserService : IUserService
    {
        #region Field
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region Implementation for IuserService
        public IEnumerable<User> Getusers()
        {
            var users = userRepository.GetMany(b => !b.Deleted);
            return users.ToList();
        }

        public User GetuserById(int userId)
        {
            var User = userRepository.GetById(userId);
            return User;
        }



   
        //


        public void Saveuser()
        {
            unitOfWork.Commit();
        }
        #endregion
    }
}
