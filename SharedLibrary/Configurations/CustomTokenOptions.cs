using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Configurations
{
    public class CustomTokenOptions  //Burası appsetting kısmında geçtiklerimize yardımcı olur authserver appseettting
    {
        //Token oluşturma 
        public List<String> Audience { get; set; }  
        public string Issuer { get; set; }
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public string SecurityKey { get; set; }
    }
}
