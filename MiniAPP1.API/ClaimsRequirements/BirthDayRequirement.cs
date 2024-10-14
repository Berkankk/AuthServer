using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Validations;

namespace MiniAPP1.API.ClaimsRequirements
{
    public class BirthDayRequirement : IAuthorizationRequirement //Policy tabanlı yetkilendirme yaptık
    {
        public int Age { get; set; }

        public BirthDayRequirement(int age)
        {
            Age = age;
        }

        public class BirthDayRequirementHandler : AuthorizationHandler<BirthDayRequirement>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BirthDayRequirement requirement)
            {
                var birthDay = context.User.FindFirst("birt-date");

                if (birthDay == null)
                {
                    context.Fail();
                    return Task.CompletedTask;
                }

                var today = DateTime.Now;

                var age = today.Year - Convert.ToDateTime(birthDay.Value).Year;

                if (age >= requirement.Age) //benim zorunlu tuttuğum yaştan büyük olacak
                {
                    context.Succeed(requirement);

                }
                else
                {
                    context.Fail();
                }

                return Task.CompletedTask;
            }
        }
    }
}
