using AuthServer.Core.DTOS;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]/[action]")] //Controller da oluşturdğumuz metot isminden erişecek 
    [ApiController]
    public class AuthController : CustomBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        //yapıcı metot geçtik çünkü interface de oluşturduğumuz metotlara ulaşmak istiyoruz
        {
            _authenticationService = authenticationService;
        }


        //api/controller/createtoken postmande bu şekilde aratacağız
        [HttpPost]
        public async Task<IActionResult> CreateToken(LoginDto loginDto)//Yukarıda yazdığımız action = createtoken metoduna denktir
        {
            var result = await _authenticationService.CreateTokenAsync(loginDto);
            return ActionResultInstance(result);//bu sayede kod tekrarına düşmeden durum kodlarını base controllerdan aldık
        }

        [HttpPost]
        public IActionResult CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var result = _authenticationService.CreateClientToken(clientLoginDto);
            return ActionResultInstance(result);
        }
        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authenticationService.RevokeRefreshToken(refreshTokenDto.RefreshToken);
            return ActionResultInstance(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto tokenDto)
        {
            var result = await _authenticationService.CreateTokenByRefreshToken(tokenDto.RefreshToken);
            return ActionResultInstance(result);
        }


    }
}
