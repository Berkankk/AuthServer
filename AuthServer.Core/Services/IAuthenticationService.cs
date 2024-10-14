using AuthServer.Core.DTOS;
using SharedLibrary.Dto;
using SharedLibrary.Respo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IAuthenticationService
    {
        Task<Response<TokenDto>> CreateTokenAsync(LoginDto logindto);

        Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken); //yeni tokan oluşturma refresh yani

        Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);//refresh token çalındığında sorun çıkmaın diye tokenı null a çekmek için kullanırız

        Response<ClientTokenDto> CreateClientToken(ClientLoginDto clientLoginDto);
    }
}
