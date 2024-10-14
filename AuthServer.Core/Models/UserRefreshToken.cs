﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Models
{
    public class UserRefreshToken
    {
        public string UserID { get; set; }
        public string RefreshTokenCode { get; set; }
        public DateTime Expiration { get; set; }
    }
}
