﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.DTOS
{
    public class ClientLoginDto
    {
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
    }
}
