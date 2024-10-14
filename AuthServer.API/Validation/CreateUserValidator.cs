using AuthServer.Core.DTOS;
using FluentValidation;

namespace AuthServer.API.Validation
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Lütfen işaretli alanı doldurun").EmailAddress().WithMessage("Girmiş olduğunuz email hatalı");


            RuleFor(x => x.Password).NotEmpty().WithMessage("Lütfen işaretli alanı doğru bir şekilde doldurun");

            RuleFor(x => x.UserName).NotEmpty().WithMessage("Lütfen kullanıcı adı alanını doldurun");
        }

    }
}
