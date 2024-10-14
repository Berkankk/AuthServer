using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.DTOS
{
    public class TokenDto
    {
        public string AccessToken { get; set; } //erişim token
        public DateTime AccessTokenExpiration { get; set; } //Ömrü
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; } //Ömrü
    }
}
