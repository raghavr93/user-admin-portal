using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UserAdminApi.Model;
using UserAdminApi.Model.Dto;

namespace UserAdminApi.Repository.IRepository
{
    public interface IUserRepository
    {
        ICollection<User> GetAllUsers();

        ICollection<User> GetNonAdminUsers();
        bool IsUniqueUser(string username);
        User Authenticate(AuthUserDto authUserDto);
        User AdminAuthenticate(AuthUserDto authUserDto);
        User Register(User user);
    }
}
