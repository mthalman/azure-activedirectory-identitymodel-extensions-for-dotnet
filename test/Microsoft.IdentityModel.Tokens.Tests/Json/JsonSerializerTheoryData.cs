// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
#if NET8

using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Json.Tests;

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class JsonSerializerTheoryData : TheoryDataBase
    {
        public JsonSerializerTheoryData(string testId) : base(testId) { }

        public ExpectedException JsonConvertExpectedException { get; set; } = ExpectedException.NoExceptionExpected;

        public ExpectedException JsonDocumentExpectedException { get; set; } = ExpectedException.NoExceptionExpected;

        public ExpectedException JsonSerializerExpectedException { get; set; } = ExpectedException.NoExceptionExpected;

        public ExpectedException JsonReaderExpectedException { get; set; } = ExpectedException.NoExceptionExpected;

        public JsonTestClass JsonTestClass { get; set; }

        public string Json { get; set; }
    }
}

#endif
