﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.DTOS
{
    public class ClientTokenDto
    {
        public string AccessToken { get; set; } //erişim token
        public DateTime AccessTokenExpiration { get; set; } //Ömrü
    }
}
