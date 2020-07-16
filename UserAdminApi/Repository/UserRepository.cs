using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserAdminApi.Data;
using UserAdminApi.Model;
using UserAdminApi.Model.Dto;
using UserAdminApi.Repository.IRepository;

namespace UserAdminApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly AppSettings _appSettings;

        public UserRepository(ApplicationDbContext db, IOptions<AppSettings> appSettings)
        {
            _db = db;
            _appSettings = appSettings.Value;
        }
        public User Authenticate(AuthUserDto authUserDto)
        {
            var user = _db.Users.FirstOrDefault(x => x.Email == authUserDto.Email && x.Password == authUserDto.Password);

            //User was not found 
            if (user == null)
            {
                return null;
            }

            //User found generate JWT Token
            return GenerateToken(user);

        }

        public User AdminAuthenticate(AuthUserDto authUserDto)
        {
            var user = _db.Users.FirstOrDefault(x => x.Email == authUserDto.Email && x.Password == authUserDto.Password && x.Role == "Admin");

            //User was not found 
            if (user == null)
            {
                return null;
            }

            //User found generate JWT Token
            return  GenerateToken(user);
           
        }

        public ICollection<User> GetAllUsers()
        {
            return _db.Users.OrderBy(x => x.FirstName).ToList();
        }

        public ICollection<User> GetNonAdminUsers()
        {
            return _db.Users.Where(x => x.Role != "Admin").ToList();
        }

        public bool IsUniqueUser(string Email)
        {
            var user = _db.Users.SingleOrDefault(x => x.Email == Email);

            if (user == null)
                return true;

            return false;
        }

        public User Register(User userObj)
        {
            if (userObj.Role != "Admin")
                userObj.Role = "User";

            _db.Users.Add(userObj);
            _db.SaveChanges();

            userObj.Password = "";

            return userObj;
        }

        private User GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials
                                (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            user.Token = tokenHandler.WriteToken(token);
            user.Password = "";

            return user;
        }
    }
}
