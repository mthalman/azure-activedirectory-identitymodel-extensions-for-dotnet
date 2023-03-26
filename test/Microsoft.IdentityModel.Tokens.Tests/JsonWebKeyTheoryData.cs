// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.TestUtils;

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class JsonWebKeyTheoryData : TheoryDataBase
    {
        public JsonWebKeyTheoryData() { }

        public JsonWebKeyTheoryData(string testId) : base(testId) { }

        public string Crv { get; set; }

        public string D { get; set; }

        public JsonWebKey ExpectedValue { get; set; }

        public JsonWebKey JsonWebKey { get; set; }

#if NET8
        internal JsonWebKeyNet8 JsonWebKeyNet8 { get; set; }
        public ExpectedException JsonReaderExpectedException { get; set; } = ExpectedException.NoExceptionExpected;
        public ExpectedException JsonSerializerExpectedException { get; set; } = ExpectedException.NoExceptionExpected;
#endif
        public string Json { get; set; }


        public bool UsePrivateKey { get; set; }

        public string X { get; set; }

        public string Y { get; set; }
    }
}
