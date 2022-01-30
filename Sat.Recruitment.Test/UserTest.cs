using System.Collections.Generic;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Entities;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserTest
    {
        [Fact]
        public void SuccessfulCreation()
        {
            var userController = new UsersController();
            var result = userController.CreateUser(GetUser()[0]).Result;

            Assert.Equal(true, result.IsSuccess);
            Assert.Equal("User Created", result.Errors);
        }

        [Fact]
        public void DuplicatedError()
        {
            var userController = new UsersController();
            var result = userController.CreateUser(GetUser()[1]).Result;

            Assert.Equal(false, result.IsSuccess);
            Assert.Equal("The user is duplicated", result.Errors);
        }

        private List<User> GetUser()
        {
            List<User> userList = new List<User>();
            userList.Add(new User()
            {
                Name = "Mike", 
                Email = "mike@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = 124
            });
            userList.Add(new User()
            {
                Name = "Agustina",
                Email = "Agustina@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = 124
            });

            return userList;
        } 
    }
}
