// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
#if NET8
using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Tests;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Json.Tests
{
    public class JsonSerializationTestUtilities
    {
        /// <summary>
        /// Adds tests cases for a type with the property name of the class and the json property name.
        /// </summary>
        /// <param name="theoryData">place to add the test case.</param>
        /// <param name="jsonPropertyName">the property name in the json mapping to the class</param>
        /// <remarks>null was not added as null needs to be handled specially per serializer.</remarks>
        public static void AddSerializationTestCases(TheoryData<JsonSerializerTheoryData> theoryData, string jsonPropertyName, string propertyType, Type innerExceptionType, Type arrayInnerException)
        {
            theoryData.Add(new JsonSerializerTheoryData($"{jsonPropertyName}_Array")
            {
                Json = $@"{{""{jsonPropertyName}"":[""string"", 1, 1.456, true, null]}}",
                JsonReaderExpectedException = (propertyType == "Array" || propertyType == "*") ? ExpectedException.NoExceptionExpected : new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = (propertyType == "Array" || propertyType == "*") ? ExpectedException.NoExceptionExpected : new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", arrayInnerException)
            });

            theoryData.Add(new JsonSerializerTheoryData($"{jsonPropertyName}_ArrayString")
            {
                Json = $@"{{""{jsonPropertyName}"":[""string1"", ""string2""]}}",
                JsonReaderExpectedException = (propertyType == "ListString" || propertyType == "*") ? ExpectedException.NoExceptionExpected : new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = (propertyType == "ListString" || propertyType == "*") ? ExpectedException.NoExceptionExpected : new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", innerExceptionType)
            });

            theoryData.Add(new JsonSerializerTheoryData($"{jsonPropertyName}_false")
            {
                Json = $@"{{""{jsonPropertyName}"": false}}",
                JsonReaderExpectedException = (propertyType == "Boolean") ? ExpectedException.NoExceptionExpected : new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = (propertyType == "Boolean") ? ExpectedException.NoExceptionExpected : new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", innerExceptionType)
            });

            theoryData.Add(new JsonSerializerTheoryData($"{jsonPropertyName}_Number")
            {
                Json = $@"{{""{jsonPropertyName}"":1}}",
                JsonReaderExpectedException = (propertyType == "Number") ? ExpectedException.NoExceptionExpected : new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = (propertyType == "Number") ? ExpectedException.NoExceptionExpected : new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", innerExceptionType)
            });

            theoryData.Add(new JsonSerializerTheoryData($"{jsonPropertyName}_Object")
            {
                Json = $@"{{""{jsonPropertyName}"":{{""property"": ""false""}}}}",
                JsonReaderExpectedException = (propertyType == "Object") ? ExpectedException.NoExceptionExpected : new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = (propertyType == "Object") ? ExpectedException.NoExceptionExpected : new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", innerExceptionType)
            });

            theoryData.Add(new JsonSerializerTheoryData($"{jsonPropertyName}_String")
            {
                Json = $@"{{""{jsonPropertyName}"":""string""}}",
                JsonReaderExpectedException = (propertyType == "String") ? ExpectedException.NoExceptionExpected : new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = (propertyType == "String") ? ExpectedException.NoExceptionExpected : new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", innerExceptionType)
            });

            theoryData.Add(new JsonSerializerTheoryData($"{jsonPropertyName}_true")
            {
                Json = $@"{{""{jsonPropertyName}"": true}}",
                JsonReaderExpectedException = (propertyType == "Boolean") ? ExpectedException.NoExceptionExpected : new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = (propertyType == "Boolean") ? ExpectedException.NoExceptionExpected : new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", innerExceptionType)
            });
        }
    }
}
#endif
