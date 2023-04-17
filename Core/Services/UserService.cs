using Core.Dtos;
using DataLayer;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class UserService
    {
        private readonly UnitOfWork unitOfWork;

        private AuthorizationService authService { get; set; }

        public UserService(UnitOfWork unitOfWork, AuthorizationService authService)
        {
            this.unitOfWork = unitOfWork;
            this.authService = authService;
        }
        public void Register(RegisterDto registerData)
        {
            if (registerData == null)
            {
                return;
            }

            var hashedPassword = authService.HashPassword(registerData.Password);

            var user = new User
            {
                FirstName = registerData.FirstName,
                Email = registerData.Email,
                PasswordHash = hashedPassword,
            };

            unitOfWork.Users.Insert(user);
            unitOfWork.SaveChanges();
        }

        public string GetRole(User user)
        {
            return user.Role;
        }

        public string Validate(LoginDto payload)
        {
            var student = unitOfWork.Users.GetByEmail(payload.Email);

            var passwordFine = authService.VerifyHashedPassword(student.PasswordHash, payload.Password);

            if (passwordFine)
            {
                var role = GetRole(student);

                return authService.GetToken(student, role);
            }
            else
            {
                return null;
            }

        }
    }
}
