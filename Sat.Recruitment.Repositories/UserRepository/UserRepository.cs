using Sat.Recruitment.Api.Entities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Sat.Recruitment.Repositories
{
    public class UserRepository
    {
        private readonly List<User> _users = new List<User>();
        /// <summary>
        /// Create the user with the data recieved
        /// </summary>
        /// <param name="inUser"></param>
        /// <returns></returns>
        public Result CreateUser(User inUser)
        {
            if (!ValidateErrors(inUser).IsSuccess)
                return ValidateErrors(inUser);

            //Normalize the email
            inUser.Email = inUser.Email.Replace('+', ' ').Trim();
            inUser.Money = GifByUserType(inUser.Money, inUser.UserType);
            
            return ValidateDuplicatedUser(inUser);
        }

        /// <summary>
        /// Read all users from File
        /// </summary>
        /// <returns></returns>
        private StreamReader ReadUsersFromFile()
        {
            var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";

            FileStream fileStream = new FileStream(path, FileMode.Open);

            StreamReader reader = new StreamReader(fileStream);
            return reader;
        }

        /// <summary>
        /// Validates if the user to be created has any field that already exist in the data base
        /// </summary>
        /// <param name="newUser">User to be created</param>
        /// <returns></returns>
        private Result ValidateDuplicatedUser(User newUser)
        {
            try
            {
                var result = new Result();
                result.IsSuccess = true;
                using (var reader = ReadUsersFromFile())
                {
                    while (reader.Peek() > -1)
                    {
                        var line = reader.ReadLine().Split(',');
                        var user = new User
                        {
                            Name = line[0],
                            Email = line[1],
                            Phone = line[2],
                            Address = line[3],
                            UserType = line[4],
                            Money = decimal.Parse(line[5]),
                        };
                        _users.Add(user);
                    }
                }
                foreach (var user in _users)
                {
                    if (user.Email == newUser.Email || user.Phone == newUser.Phone || user.Name == newUser.Name || user.Address == newUser.Address)
                    {
                        result.IsSuccess = false;
                        result.Errors = "The user is duplicated";
                    }
                }

                if (result.IsSuccess)
                {
                    result.Errors = "User Created";
                }
                Debug.WriteLine(result.Errors);
                return result;
            }
            catch
            {
                Debug.WriteLine("An error has ocurred while creating user");
                return new Result()
                {
                    IsSuccess = false,
                    Errors = "An error has ocurred while creating user"
                };
            }
        }

        /// <summary>
        /// Returns the value of the Money+Gif for the given User Type
        /// </summary>
        /// <param name="money">value for Money field</param>
        /// <param name="userType">value for User Type</param>
        /// <returns></returns>
        private decimal GifByUserType(decimal money, string userType)
        {
            if (userType == "Normal")
            {
                //If new user is normal and has more than USD100
                if (money > 100)
                    money *= (decimal)1.12;
                else
                {
                    if (money > 10)
                        money *= (decimal)1.8;
                }
            }
            if (userType == "SuperUser")
            {
                if (money > 100)
                    money *= (decimal)1.20;
            }
            if (userType == "Premium")
            {
                if (money > 100)
                    money *= 2;
            }
            return money;
        }

        /// <summary>
        /// Validate if any field is missing
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="address"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        private Result ValidateErrors(User inUser)
        {
            Result error = new Result();
            error.IsSuccess = true;

            if (string.IsNullOrEmpty(inUser.Name))
                error.Errors += "The name is required.";
            if (string.IsNullOrEmpty(inUser.Email))
                error.Errors += " The email is required.";
            if (string.IsNullOrEmpty(inUser.Address))
                error.Errors += " The address is required.";
            if (string.IsNullOrEmpty(inUser.Phone))
                error.Errors += " The phone is required.";

            if (!string.IsNullOrEmpty(error.Errors))
            {
                error.IsSuccess = false;
            }
            return error;
        }
    }
}
