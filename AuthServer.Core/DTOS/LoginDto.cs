using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.DTOS
{
    public class LoginDto
    {
        public string Email { get; set; } //Requestte gelecek
        public string Password { get; set; } //Veritabanındakiler ile uyuşuyorsa geriye token döneceğiz
    }
}
