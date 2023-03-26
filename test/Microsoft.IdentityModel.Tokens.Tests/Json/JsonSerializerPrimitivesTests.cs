// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
#if NET8
using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Azure.KeyVault.WebKey;
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Tests;
using Newtonsoft.Json;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Json.Tests
{
    public class JsonSerializerPrimitivesTests
    {
        /// <summary>
        /// This test is designed to ensure that JsonDeserialize and Utf8Reader are consistent w.r.t. exceptions.
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(DeserializeTheoryData))]
        public void DeserializeCheckingExceptions(JsonSerializerTheoryData theoryData)
        {
            var context = new CompareContext(theoryData);
            JsonTestClass jsonDeserialize = null;
            JsonTestClass jsonRead = null;

            //JsonTestClass jsonNewtonsoft = new JsonTestClass();
            //JsonDocument document = null;

            //try
            //{
            //    document = JsonDocument.Parse(theoryData.Json);
            //}
            //catch(Exception ex)
            //{
            //    theoryData.JsonSerializerExpectedException.ProcessException(ex, context);
            //}

            //try
            //{
            //    JsonConvert.PopulateObject(theoryData.Json, jsonNewtonsoft);
            //    theoryData.JsonSerializerExpectedException.ProcessNoException(context);
            //}
            //catch (Exception ex)
            //{
            //    theoryData.JsonSerializerExpectedException.ProcessException(ex, context);
            //}

            CompareContext localContext = new CompareContext(theoryData);
            try
            {
                jsonDeserialize = System.Text.Json.JsonSerializer.Deserialize<JsonTestClass>(theoryData.Json);
                theoryData.JsonSerializerExpectedException.ProcessNoException(localContext);
            }
            catch (Exception ex)
            {
                theoryData.JsonSerializerExpectedException.ProcessException(ex, localContext);
            }

            if (localContext.Diffs.Count > 0)
            {
                context.Diffs.Add("Difference in JsonSerializer.Deserialize");
                context.Merge(localContext);
            }

            localContext.Diffs.Clear();
            try
            {
                jsonRead = JsonTestClassSerializer.Read(theoryData.Json);
                theoryData.JsonReaderExpectedException.ProcessNoException(localContext);
            }
            catch (Exception ex)
            {
                theoryData.JsonReaderExpectedException.ProcessException(ex, localContext);
            }

            if (localContext.Diffs.Count > 0)
            {
                context.Diffs.Add("Difference in JsonTestClassSerializer.Read");
                context.Merge(localContext);
            }

            IdentityComparer.AreEqual(jsonDeserialize, jsonRead);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonSerializerTheoryData> DeserializeTheoryData
        {
            get
            {
                var theoryData = new TheoryData<JsonSerializerTheoryData>();
                JsonSerializationTestUtilities.AddSerializationTestCases(theoryData, "Boolean", "Boolean", typeof(InvalidOperationException), typeof(InvalidOperationException));
                JsonSerializationTestUtilities.AddSerializationTestCases(theoryData, "Double", "Number", typeof(InvalidOperationException), typeof(InvalidOperationException));
                JsonSerializationTestUtilities.AddSerializationTestCases(theoryData, "Int", "Number", typeof(InvalidOperationException), typeof(InvalidOperationException));
                JsonSerializationTestUtilities.AddSerializationTestCases(theoryData, "ListObject", "*", null, null);
                JsonSerializationTestUtilities.AddSerializationTestCases(theoryData, "ListString", "ListString", null, typeof(InvalidOperationException));
                JsonSerializationTestUtilities.AddSerializationTestCases(theoryData, "String", "String", typeof(InvalidOperationException), typeof(InvalidOperationException));
                return theoryData;
            }
        }

        /// <summary>
        /// This test is designed to ensure that JsonDeserialize and Utf8Reader are consistent w.r.t. exceptions.
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(SerializeTheoryData))]
        public void Serialize(JsonSerializerTheoryData theoryData)
        {
            var context = new CompareContext(theoryData);

            string jsonIdentityModel = Microsoft.IdentityModel.Json.JsonConvert.SerializeObject(theoryData.JsonTestClass);
            string jsonNewtonsoft = Newtonsoft.Json.JsonConvert.SerializeObject(theoryData.JsonTestClass);
            string jsonSerialize = System.Text.Json.JsonSerializer.Serialize(theoryData.JsonTestClass);
            string jsonWrite = JsonTestClassSerializer.Write(theoryData.JsonTestClass);

            CompareContext localContext = new CompareContext(theoryData);
            IdentityComparer.AreEqual(jsonNewtonsoft, jsonIdentityModel, localContext);
            if (localContext.Diffs.Count > 0)
            {
                context.Diffs.Add("Difference in Newtonsoft, IdentityModel");
                context.Merge(localContext);
            }

            localContext.Diffs.Clear();
            IdentityComparer.AreEqual(jsonNewtonsoft, jsonSerialize, localContext);
            if (localContext.Diffs.Count > 0)
            {
                context.Diffs.Add("Difference in Newtonsoft, JsonSerializer.Serialize");
                context.Merge(localContext);
            }

            localContext.Diffs.Clear();
            IdentityComparer.AreEqual(jsonNewtonsoft, jsonWrite, localContext);
            if (localContext.Diffs.Count > 0)
            {
                context.Diffs.Add("Difference in Newtonsoft, JsonTestClassSerializer.Write");
                context.Merge(localContext);
            }

            localContext.Diffs.Clear();
            IdentityComparer.AreEqual(jsonSerialize, jsonWrite, localContext);
            if (localContext.Diffs.Count > 0)
            {
                context.Diffs.Add("Difference in JsonSerializer.Serialize and JsonTestClassSerializer.Write");
                context.Merge(localContext);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonSerializerTheoryData> SerializeTheoryData
        {
            get
            {
                TheoryData<JsonSerializerTheoryData> theoryData = new TheoryData<JsonSerializerTheoryData>();

                theoryData.Add(new JsonSerializerTheoryData("FullyPopulated")
                {
                    JsonTestClass = CreateJsonTestClass("*")
                });

                theoryData.Add(new JsonSerializerTheoryData("AdditionalData")
                {
                    JsonTestClass = CreateJsonTestClass("AdditionalData")
                });

                theoryData.Add(new JsonSerializerTheoryData("Boolean")
                {
                    JsonTestClass = CreateJsonTestClass("Boolean")
                });

                theoryData.Add(new JsonSerializerTheoryData("Double")
                {
                    JsonTestClass = CreateJsonTestClass("Double")
                });

                theoryData.Add(new JsonSerializerTheoryData("Int")
                {
                    JsonTestClass = CreateJsonTestClass("Int")
                });

                theoryData.Add(new JsonSerializerTheoryData("ListObject")
                {
                    JsonTestClass = CreateJsonTestClass("ListObject")
                });

                theoryData.Add(new JsonSerializerTheoryData("ListString")
                {
                    JsonTestClass = CreateJsonTestClass("ListString")
                });

                theoryData.Add(new JsonSerializerTheoryData("String")
                {
                    JsonTestClass = CreateJsonTestClass("String")
                });

                return theoryData;
            }
        }

        private static JsonTestClass CreateJsonTestClass(string propertiesToSet)
        {
            JsonTestClass jsonTestClass = new JsonTestClass();

            if (propertiesToSet == "*" || propertiesToSet.Contains("AdditionalData"))
            {
                jsonTestClass.AdditionalData["Key1"] = "Data1";
                jsonTestClass.AdditionalData["Object"] = new JsonTestClass { Boolean = true, Double = 1.4, AdditionalData = new Dictionary<string, object> { { "key", "value" } } };
            }

            if (propertiesToSet == "*" || propertiesToSet.Contains("Boolean"))
                jsonTestClass.Boolean = true;

            if (propertiesToSet == "*" || propertiesToSet.Contains("Double"))
                jsonTestClass.Double = 1.1;

            if (propertiesToSet == "*" || propertiesToSet.Contains("Int"))
                jsonTestClass.Int = 1;

            if (propertiesToSet == "*" || propertiesToSet.Contains("ListObject"))
                jsonTestClass.ListObject = new List<object> { 1, "string", true, "{\"innerArray\", [1, \"innerValue\"] }" };

            if (propertiesToSet == "*" || propertiesToSet.Contains("ListString"))
                jsonTestClass.ListString = new List<string> { "string1", "string2" };

            if (propertiesToSet == "*" || propertiesToSet.Contains("String"))
                jsonTestClass.String = "string";

            return jsonTestClass;
        }
    }
}
#endif
