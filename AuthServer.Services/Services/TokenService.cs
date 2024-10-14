using AuthServer.Core.Configuration;
using AuthServer.Core.DTOS;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using SharedLibrary.Services;


namespace AuthServer.Services.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly CustomTokenOptions _customTokenOptions;

        public TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOptions> customTokenOptions)
        {
            _userManager = userManager;
            _customTokenOptions = customTokenOptions.Value;
        }

        //Refresh token üretmesi için 

        private string CreateRefreshToken()
        {
            var numberByte = new Byte[32];
            using var random = RandomNumberGenerator.Create(); //Random değer üretmesi için kullanıyoruz 

            random.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }

        private async Task<IEnumerable<Claim>> GetClaims(UserApp userApp, List<string> audience) //KUllanıcı
        {
            var userRoles = await _userManager.GetRolesAsync(userApp); //Rolünü veriyoruz
            //["admin"],["manager"],["user"] gibi roller atıyoruz burada



            var userList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userApp.Id), //Kullanıcıların id lerini aldık benzersiz tabii
                new Claim(JwtRegisteredClaimNames.Email, userApp.Email), // benzersiz email adreslerini aldık
                new Claim(ClaimTypes.Name,userApp.UserName), //kullanıcı adını aldık
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim("city",userApp.City),
                new Claim("birth-date" ,userApp.BirthDate.ToShortDateString()),
                //Claimtypes ile jwtregisteredCalimnames aynı amacı var ikisini de kullansak olur o yüzden
            };
            userList.AddRange(audience.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x))); //Hedef kullanıcı bilgilerini claimlere ekledik
            userList.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));
            return userList;
        }

        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>(); //Yeni talep açtım e talep bir tane olmadığı için liste olarak açtım hadi hayırlı olsun
            claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            // Client'in 'Audiences' listesindeki her eleman için bir 'audience' claim'i ekleniyor.

            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
            new Claim(JwtRegisteredClaimNames.Sub, client.ClientID.ToString());
            return claims;
        }


        public TokenDto CreateToken(UserApp userApp) //Kullanıcın authenticate olması için gerekli bilgiler
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.AccessTokenExpiration);//Burada token süresi ayarladık
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.RefreshTokenExpiration); //Burada token süresi ayarladık
            var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOptions.SecurityKey); //Güvenliksiz olmaz yakışıklı güvenliği verdik

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature); //imza yemini verdik
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _customTokenOptions.Issuer, //Tokenın kafa kağıdı kimliği
                expires: accessTokenExpiration, //Tokenin tavsiye edilen son tüketim tarihi
                notBefore: DateTime.Now, //Bu zamandan önce token kullanılmaz 
                claims: GetClaims(userApp, _customTokenOptions.Audience).Result, //Bilgi ve yetkin var mı bakim
                signingCredentials: signingCredentials); //imza token:)

            var handler = new JwtSecurityTokenHandler(); //Jwt token oluşturmak ve işlem yapmak için kullanılan sınıftır kendisi 

            var token = handler.WriteToken(jwtSecurityToken); //AA şok string değere dönüştü 
            var tokenDto = new TokenDto()
            {
                AccessToken = token, //Tokenı sakla kimselere verme sakın
                AccessTokenExpiration = accessTokenExpiration, //Süreyi tut 
                RefreshTokenExpiration = refreshTokenExpiration, //Eğer diğeri süreyi kaybederse sen de tut aynı anda tutun o durduğu yerde sen devam et
                RefreshToken = CreateRefreshToken(), // Rastgele token üret sende salla bakam 
            };
            return tokenDto; // DÖNN 

        }

        public ClientTokenDto CreateTokenbyClient(Client client)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.AccessTokenExpiration);//Burada token süresi ayarladık
            var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOptions.SecurityKey); //Güvenliksiz olmaz yakışıklı güvenliği verdik

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature); //imza yemini verdik
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _customTokenOptions.Issuer, //Tokenın kafa kağıdı kimliği
                expires: accessTokenExpiration, //Tokenin tavsiye edilen son tüketim tarihi
                notBefore: DateTime.Now, //Bu zamandan önce token kullanılmaz 

                claims: GetClaimsByClient(client), //Bilgi ve yetkin var mı bakim
                signingCredentials: signingCredentials); //imza token:)

            var handler = new JwtSecurityTokenHandler(); //Jwt token oluşturmak ve işlem yapmak için kullanılan sınıftır kendisi 

            var token = handler.WriteToken(jwtSecurityToken); //AA şok string değere dönüştü 
            var tokenDto = new ClientTokenDto()
            {
                AccessToken = token, //Tokenı sakla kimselere verme sakın
                AccessTokenExpiration = accessTokenExpiration, //Süreyi tut 
            };
            return tokenDto; // DÖNN 
        }
    }
}
