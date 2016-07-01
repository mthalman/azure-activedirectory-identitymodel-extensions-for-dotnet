//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace System.IdentityModel.Tokens.Jwt.Tests
{
    /// <summary>
    /// Used in extensibility tests to ensure that the same token flows through validation.
    /// </summary>
    public class DerivedJwtSecurityToken : JwtSecurityToken
    {
        // TODO - need to add tests for delegates.

        public DerivedJwtSecurityToken(string encodedJwt)
            : base(encodedJwt)
        {
            Init();
        }

        public DerivedJwtSecurityToken(string issuer = null, string audience = null, IEnumerable<Claim> claims = null, DateTime? expires = null, DateTime? notbefore = null, SigningCredentials signingCredentials = null)
            : base(issuer, audience, claims, expires, notbefore, signingCredentials)
        {
            Init();
        }

        public bool ValidateAudienceCalled { get; set; }

        public bool ValidateLifetimeCalled { get; set; }

        public bool ValidateIssuerCalled { get; set; }

        public bool ValidateSignatureCalled { get; set; }

        public bool ValidateSigningKeyCalled { get; set; }

        public string Guid { get; set; }

        private void Init()
        {
            ValidateAudienceCalled = false;
            ValidateLifetimeCalled = false;
            ValidateIssuerCalled = false;
            ValidateSignatureCalled = false;
            ValidateSigningKeyCalled = false;
        }
    }

    /// <summary>
    /// Ensures that all protected types use same token.
    /// </summary>
    public class DerivedJwtSecurityTokenHandler : JwtSecurityTokenHandler
    {
        public DerivedJwtSecurityTokenHandler()
            : base()
        {
        }

        public Type DerivedTokenType
        {
            get;
            set;
        }

        public bool ReadTokenCalled { get; set; }

        public bool ValidateAudienceCalled { get; set; }

        public bool ValidateLifetimeCalled { get; set; }

        public bool ValidateIssuerCalled { get; set; }

        public bool ValidateIssuerSigningKeyCalled { get; set; }

        public bool ValidateSignatureCalled { get; set; }

        public JwtSecurityToken Jwt { get; set; }

        public override SecurityToken ReadToken(string jwtEncodedString)
        {
            ReadTokenCalled = true;
            return new DerivedJwtSecurityToken(jwtEncodedString);
        }

        protected override void ValidateAudience(IEnumerable<string> audiences, JwtSecurityToken jwt, TokenValidationParameters validationParameters)
        {
            DerivedJwtSecurityToken derivedJwt = jwt as DerivedJwtSecurityToken;
            Assert.NotNull(derivedJwt);
            ValidateAudienceCalled = true;
            base.ValidateAudience(audiences, jwt, validationParameters);
        }

        protected override string ValidateIssuer(string issuer, JwtSecurityToken jwt, TokenValidationParameters validationParameters)
        {
            DerivedJwtSecurityToken derivedJwt = jwt as DerivedJwtSecurityToken;
            Assert.NotNull(derivedJwt);
            ValidateIssuerCalled = true;
            return base.ValidateIssuer(issuer, jwt, validationParameters);
        }

        protected override void ValidateIssuerSecurityKey(SecurityKey securityKey, JwtSecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            DerivedJwtSecurityToken derivedJwt = securityToken as DerivedJwtSecurityToken;
            Assert.NotNull(derivedJwt);
            ValidateIssuerSigningKeyCalled = true;
            base.ValidateIssuerSecurityKey(securityKey, securityToken, validationParameters);
        }

        protected override void ValidateLifetime(DateTime? notBefore, DateTime? expires, JwtSecurityToken jwt, TokenValidationParameters validationParameters)
        {
            DerivedJwtSecurityToken derivedJwt = jwt as DerivedJwtSecurityToken;
            Assert.NotNull(derivedJwt);
            ValidateLifetimeCalled = true;
            base.ValidateLifetime(notBefore, expires, jwt, validationParameters);
        }

        protected override JwtSecurityToken ValidateSignature(string securityToken, TokenValidationParameters validationParameters)
        {
            Jwt = base.ValidateSignature(securityToken, validationParameters);
            DerivedJwtSecurityToken derivedJwt = Jwt as DerivedJwtSecurityToken;
            Assert.NotNull(derivedJwt);
            ValidateSignatureCalled = true;
            return Jwt;
        }

        public override ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            return base.ValidateToken(securityToken, validationParameters, out validatedToken);
        }
    }

    public class PublicJwtSecurityTokenHandler : JwtSecurityTokenHandler
    {
        public void ValidateAudiencePublic(JwtSecurityToken jwt, TokenValidationParameters validationParameters)
        {
            base.ValidateAudience(new string[]{jwt.Issuer}, jwt, validationParameters);
        }

        public string ValidateIssuerPublic(JwtSecurityToken jwt, TokenValidationParameters validationParameters)
        {
            return base.ValidateIssuer(jwt.Issuer, jwt, validationParameters);
        }

        public void ValidateLifetimePublic(JwtSecurityToken jwt, TokenValidationParameters validationParameters)
        {
            base.ValidateLifetime(DateTime.UtcNow, DateTime.UtcNow, jwt, validationParameters);
        }

        public void ValidateSigningTokenPublic(SecurityKey securityKey, JwtSecurityToken jwt, TokenValidationParameters validationParameters)
        {
            base.ValidateIssuerSecurityKey(securityKey, jwt, validationParameters);
        }
    }
}