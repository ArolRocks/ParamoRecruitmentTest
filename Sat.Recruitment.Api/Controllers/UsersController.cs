using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.Entities;
using Sat.Recruitment.Repositories;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class UsersController : ControllerBase
    {
        [HttpPost]
        [Route("/create-user")]
        public async Task<Result> CreateUser(User newUser)
        {
            UserRepository userRepository = new UserRepository();
            return userRepository.CreateUser(newUser);           
        }  
    }    
}
