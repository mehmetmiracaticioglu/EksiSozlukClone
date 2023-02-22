using EksiSozlukClone.Common.Models.RequestModels;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozlukClone.Core.Application.Features.Commands.User
{
    public class LoginUserCommandValidator:AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(i=>i.EmailAdress).
                NotNull().
                EmailAddress(EmailValidationMode.AspNetCoreCompatible).
                WithMessage("{PropertyName} not a valid ");
            
            RuleFor(i=>i.Password).
                NotNull()
                .MinimumLength(8).WithMessage("Password can not short 8 character");

        }
    }
}
