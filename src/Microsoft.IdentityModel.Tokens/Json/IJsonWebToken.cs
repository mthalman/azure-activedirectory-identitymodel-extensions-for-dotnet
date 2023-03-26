// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NET8
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens.Json
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public interface IJsonClaimSet
    {
        public string GetPayloadValue<T>(string claimId);

        public string GetHeaderValue<T>(string claimId);
    }

    public interface IJsonClaims
    {
        public string GetPayloadValue<T>(string claimId);

        public string GetHeaderValue<T>(string claimId);
    }

    public interface IJsonWebToken
    {
        public IEnumerable<string> Audiences { get; }

        public string EncryptedKey { get; }

        public string EncodedSignature { get; }

        public string EncodedToken { get; }

        public T GetPayloadValue<T>(string claimId);

        public T GetHeaderValue<T>(string claimId);

        public abstract string Id { get; }

        public string InitializationVector { get; }
    }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
#endif // #if NET6_0
