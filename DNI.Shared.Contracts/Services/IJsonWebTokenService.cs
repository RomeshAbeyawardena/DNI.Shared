﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace DNI.Shared.Contracts.Services
{
    public interface IJsonWebTokenService
    {
        string CreateToken(Action<SecurityTokenDescriptor> populateSecurityTokenDescriptor, DateTime expiry, 
            IDictionary<string, string> claimsDictionary, string secret, Encoding encoding);
        bool TryParseToken(string token, string secret, 
            Action<TokenValidationParameters> populateTokenValidationParameters, 
            Encoding encoding, out IDictionary<string, string> claims);
    }
}