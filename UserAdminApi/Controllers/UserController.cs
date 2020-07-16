using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAdminApi.Model;
using UserAdminApi.Model.Dto;
using UserAdminApi.Repository.IRepository;

namespace UserAdminApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _uRepo;
        private readonly IMapper _mapper;

        public UserController(IUserRepository uRepo, IMapper mapper)
        {
            _uRepo = uRepo;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthUserDto userDto)
        {
            var user = _uRepo.Authenticate(userDto);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect." });
            }

            var tokenObj = _mapper.Map<TokenDto>(user);

            return Ok(tokenObj);
        }

        [AllowAnonymous]
        [HttpPost("admin-auth")]
        public IActionResult AdminAuth([FromBody] AuthUserDto userDto)
        {
            var user = _uRepo.AdminAuthenticate(userDto);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect." });
            }

            var tokenObj = _mapper.Map<TokenDto>(user);

            return Ok(tokenObj);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("exists")]
        public IActionResult UserExists([FromBody] string email)
        {
            var user = _uRepo.IsUniqueUser(email);
            if (user == false)
            {
                return BadRequest(new { message = "User Already Exists!" });
            }

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDto model)
        {
            if (model == null)
            {
                return BadRequest(ModelState);
            }

            bool isUserNameUnique = _uRepo.IsUniqueUser(model.Email);

            if (!isUserNameUnique)
            {
                return BadRequest(new { message = "User Already Exists!" });
            }

            var userObj = _mapper.Map<User>(model);

            var user = _uRepo.Register(userObj);

            if (user == null)
            {
                return BadRequest(new { message = "Error while registering!" });
            }

            return Ok(model);

        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            var idClaim = claim.Where(x => x.Type == ClaimTypes.Role)
                .FirstOrDefault().Value;

            ICollection<User> objList;

            if (idClaim == "Admin")
            {
                  objList = _uRepo.GetAllUsers();
            }
            else
            {
                  objList = _uRepo.GetNonAdminUsers();
            }

            var objDto = new List<GetUserDto>();

            foreach (var obj in objList)
            {
                int age = CalculateAge(obj);
                var getObj = _mapper.Map<GetUserDto>(obj);
                getObj.Age = age;
                objDto.Add(getObj);
            }

            return Ok(objDto);
        }

        private int CalculateAge(User obj)
        {
            DateTime zeroTime = new DateTime(1, 1, 1);
            DateTime a = DateTime.Now;
            DateTime b = obj.DateOfBirth;
            TimeSpan span = a - b;
            int years = (zeroTime + span).Year - 1;
            return years;
        }


    }
}
