// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Tests;
using Newtonsoft.Json;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Json.Tests
{
    public class JsonWebKeySerializationTests
    {
        static string DP = "ErP3OpudePAY3uGFSoF16Sde69PnOra62jDEZGnPx_v3nPNpA5sr-tNc8bQP074yQl5kzSFRjRlstyW0TpBVMP0ocbD8RsN4EKsgJ1jvaSIEoP87OxduGkim49wFA0Qxf_NyrcYUnz6XSidY3lC_pF4JDJXg5bP_x0MUkQCTtQE";
        static string DQ = "YbBsthPt15Pshb8rN8omyfy9D7-m4AGcKzqPERWuX8bORNyhQ5M8JtdXcu8UmTez0j188cNMJgkiN07nYLIzNT3Wg822nhtJaoKVwZWnS2ipoFlgrBgmQiKcGU43lfB5e3qVVYUebYY0zRGBM1Fzetd6Yertl5Ae2g2CakQAcPs";
        static string Exponent = "AQAB";
        static string InverseQ = "lbljWyVY-DD_Zuii2ifAz0jrHTMvN-YS9l_zyYyA_Scnalw23fQf5WIcZibxJJll5H0kNTIk8SCxyPzNShKGKjgpyZHsJBKgL3iAgmnwk6k8zrb_lqa0sd1QWSB-Rqiw7AqVqvNUdnIqhm-v3R8tYrxzAqkUsGcFbQYj4M5_F_4";
        static string Modulus = "6-FrFkt_TByQ_L5d7or-9PVAowpswxUe3dJeYFTY0Lgq7zKI5OQ5RnSrI0T9yrfnRzE9oOdd4zmVj9txVLI-yySvinAu3yQDQou2Ga42ML_-K4Jrd5clMUPRGMbXdV5Rl9zzB0s2JoZJedua5dwoQw0GkS5Z8YAXBEzULrup06fnB5n6x5r2y1C_8Ebp5cyE4Bjs7W68rUlyIlx1lzYvakxSnhUxSsjx7u_mIdywyGfgiT3tw0FsWvki_KYurAPR1BSMXhCzzZTkMWKE8IaLkhauw5MdxojxyBVuNY-J_elq-HgJ_dZK6g7vMNvXz2_vT-SykIkzwiD9eSI9UWfsjw";
        static string P = "_avCCyuo7hHlqu9Ec6R47ub_Ul_zNiS-xvkkuYwW-4lNnI66A5zMm_BOQVMnaCkBua1OmOgx7e63-jHFvG5lyrhyYEmkA2CS3kMCrI-dx0fvNMLEXInPxd4np_7GUd1_XzPZEkPxBhqf09kqryHMj_uf7UtPcrJNvFY-GNrzlJk";
        static string Q = "7gvYRkpqM-SC883KImmy66eLiUrGE6G6_7Y8BS9oD4HhXcZ4rW6JJKuBzm7FlnsVhVGro9M-QQ_GSLaDoxOPQfHQq62ERt-y_lCzSsMeWHbqOMci_pbtvJknpMv4ifsQXKJ4Lnk_AlGr-5r5JR5rUHgPFzCk9dJt69ff3QhzG2c";
        static string P256_D = "OOX7PnYlSTE41BSclDj5Gi_sx_SPgEqStjY3doku4TQ";
        static string P256_X = "luR290c8sXxbOGhNquQ3J3rh763Os4D609cHK-L_5fA";
        static string P256_Y = "tUqUwtaVHwc7_CXnuBrCpMQTF5BJKdFnw9_JkSIXWpQ";
        static string X5C = "MIIDPjCCAiqgAwIBAgIQsRiM0jheFZhKk49YD0SK1TAJBgUrDgMCHQUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMTQwMTAxMDcwMDAwWhcNMTYwMTAxMDcwMDAwWjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuwIDAQABo2IwYDBeBgNVHQEEVzBVgBDLebM6bK3BjWGqIBrBNFeNoS8wLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldIIQsRiM0jheFZhKk49YD0SK1TAJBgUrDgMCHQUAA4IBAQCJ4JApryF77EKC4zF5bUaBLQHQ1PNtA1uMDbdNVGKCmSf8M65b8h0NwlIjGGGy/unK8P6jWFdm5IlZ0YPTOgzcRZguXDPj7ajyvlVEQ2K2ICvTYiRQqrOhEhZMSSZsTKXFVwNfW6ADDkN3bvVOVbtpty+nBY5UqnI7xbcoHLZ4wYD251uj5+lo13YLnsVrmQ16NCBYq2nQFNPuNJw6t3XUbwBHXpF46aLT1/eGf/7Xx6iy8yPJX4DyrpFTutDz882RWofGEO5t4Cw+zZg70dJ/hH/ODYRMorfXEW+8uKmXMKmX2wyxMKvfiPbTy5LmAU8Jvjs2tLg4rOBcXWLAIarZ";

        /// <summary>
        /// This test is designed to ensure that JsonDeserialize and Utf8Reader are consistent w.r.t. exceptions.
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(DeserializeTheoryData))]
        public void DeserializeCheckingExceptions(JsonSerializerTheoryData theoryData)
        {
            var context = new CompareContext(theoryData);
            JsonWebKey jsonWebKeyDeserialize = null;
            JsonWebKey jsonWebKeyRead = null;

            try
            {
                jsonWebKeyDeserialize = System.Text.Json.JsonSerializer.Deserialize<JsonWebKey>(theoryData.Json);
                theoryData.JsonSerializerExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.JsonSerializerExpectedException.ProcessException(ex, context);
            }

            try
            {
                jsonWebKeyRead = JsonWebKeySerializer.Read(theoryData.Json);
                theoryData.JsonReaderExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.JsonReaderExpectedException.ProcessException(ex, context);
            }

            IdentityComparer.AreEqual(jsonWebKeyDeserialize, jsonWebKeyRead);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonSerializerTheoryData> DeserializeTheoryData
        {
            get
            {
                var theoryData = new TheoryData<JsonSerializerTheoryData>();
                AddSingleStringTestCases(theoryData, "Alg", JsonWebKeyParameterNames.Alg);
                AddArrayOfStringsTestCases(theoryData, "KeyOps", JsonWebKeyParameterNames.KeyOps);
                return theoryData;
            }
        }

        /// <summary>
        /// Adds tests cases for a type with the property name of the class and the json property name.
        /// </summary>
        /// <param name="theoryData">place to add the test case.</param>
        /// <param name="instancePropertyName">the property name on the class.</param>
        /// <param name="jsonPropertyName">the property name in the json mapping to the class</param>
        private static void AddSingleStringTestCases(TheoryData<JsonSerializerTheoryData> theoryData, string instancePropertyName, string jsonPropertyName)
        {
            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_DuplicateProperties")
            {
                Json = $@"{{""{jsonPropertyName}"":""string"", ""{jsonPropertyName}"":""string""}}",
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_SingleString")
            {
                Json = $@"{{""{jsonPropertyName}"":""string""}}",
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_ArrayString")
            {
                Json = $@"{{""{jsonPropertyName}"":[""string1"", ""string2""]}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", typeof(InvalidOperationException))
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_Array")
            {
                Json = $@"{{""{jsonPropertyName}"":[""value"", 1]}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", typeof(InvalidOperationException))
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_true")
            {
                Json = $@"{{""{jsonPropertyName}"": true}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", typeof(InvalidOperationException))
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_false")
            {
                Json = $@"{{""{jsonPropertyName}"": false}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", typeof(InvalidOperationException))
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_Object")
            {
                Json = $@"{{""{jsonPropertyName}"":{{""property"": ""false""}}}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", typeof(InvalidOperationException))
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_Null")
            {
                Json = $@"{{""{jsonPropertyName}"":null}}",
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_Number")
            {
                Json = $@"{{""d"":""string"",""d"":""string"",""{jsonPropertyName}"":1}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", typeof(InvalidOperationException))
            });
        }

        /// <summary>
        /// Adds tests cases for a type with the property name of the class and the json property name.
        /// </summary>
        /// <param name="theoryData">place to add the test case.</param>
        /// <param name="instancePropertyName">the property name on the class.</param>
        /// <param name="jsonPropertyName">the property name in the json mapping to the class</param>
        private static void AddArrayOfStringsTestCases(TheoryData<JsonSerializerTheoryData> theoryData, string instancePropertyName, string jsonPropertyName)
        {
            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_SingleString")
            {
                Json = $@"{{""{jsonPropertyName}"":""string""}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020:"),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted")
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_ArrayString")
            {
                Json = $@"{{""{jsonPropertyName}"":[""string1"", ""string2""]}}",
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_Array")
            {
                Json = $@"{{""{jsonPropertyName}"":[""value"", 1]}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted", typeof(InvalidOperationException))
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_true")
            {
                Json = $@"{{""{jsonPropertyName}"": true}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted")
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_false")
            {
                Json = $@"{{""{jsonPropertyName}"": false}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted")
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_Object")
            {
                Json = $@"{{""{jsonPropertyName}"":{{""property"": ""false""}}}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted")
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_Null")
            {
                Json = $@"{{""{jsonPropertyName}"":null}}",
                JsonReaderExpectedException = new ExpectedException(typeof(ArgumentNullException), JsonWebKeyParameterNames.KeyOps, typeof(System.Text.Json.JsonException)),
                JsonSerializerExpectedException = new ExpectedException(typeof(ArgumentNullException), "IDX10000: ")
            });

            theoryData.Add(new JsonSerializerTheoryData($"{instancePropertyName}_Number")
            {
                Json = $@"{{""d"":""string"",""d"":""string"",""{jsonPropertyName}"":1}}",
                JsonReaderExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "IDX11020: "),
                JsonSerializerExpectedException = new ExpectedException(typeof(System.Text.Json.JsonException), "The JSON value could not be converted")
            });
        }

        /// <summary>
        /// Compares and finds differences between our internal Newtonsoft.Json and System.Text.Json
        /// We also compare the results JsonSerializer.Deserialize and Utf8JsonReader.
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(DeserializeDataSet))]
        public void Deserialize(JsonWebKeyTheoryData theoryData)
        {
            CompareContext context = new CompareContext(theoryData);

            JsonWebKey jsonWebKeySerialize = null;
            JsonWebKey jsonWebKeyReader = null;
            JsonWebKey jsonWebKey = null;

            try
            {
                jsonWebKeySerialize = System.Text.Json.JsonSerializer.Deserialize<JsonWebKey>(theoryData.Json);
                theoryData.JsonSerializerExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.JsonSerializerExpectedException.ProcessException(ex, context);
            }

            try
            {
                jsonWebKeyReader = new JsonWebKey(theoryData.Json);
                theoryData.JsonReaderExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.JsonReaderExpectedException.ProcessException(ex, context);
            }

            try
            {
                jsonWebKey = new JsonWebKey(theoryData.Json);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            // when comparing JsonWebKey (Newtonsoft.Json) and JsonWebKey (System.Text.Json) we ignore the AdditionalData property.
            // because JsonWebKey (Newtonsoft.Json) sets an object and JsonWebKey (System.Text.Json) sets a JsonElement.
            CompareContext localContext = new CompareContext(theoryData);
            localContext.PropertiesToIgnoreWhenComparing.Add(typeof(JsonWebKey), new List<string> { "AdditionalData" });
            IdentityComparer.AreEqual(jsonWebKeySerialize, jsonWebKey, localContext);
            context.Merge(localContext);

            IdentityComparer.AreEqual(jsonWebKeySerialize, jsonWebKeyReader, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonWebKeyTheoryData> DeserializeDataSet
        {
            get
            {
                var theoryData = new TheoryData<JsonWebKeyTheoryData>();
                theoryData.Add(new JsonWebKeyTheoryData("JsonWebKeyFromPing1")
                {
                    ExpectedValue = DataSets.JsonWebKeyFromPing1,
                    Json = DataSets.JsonWebKeyFromPingString1
                });

                theoryData.Add(new JsonWebKeyTheoryData("JsonWebKey1")
                {
                    ExpectedValue = DataSets.JsonWebKey1,
                    Json = DataSets.JsonWebKeyString1
                });

                theoryData.Add(new JsonWebKeyTheoryData("JsonWebKey2")
                {
                    ExpectedValue = DataSets.JsonWebKey2,
                    Json = DataSets.JsonWebKeyString2
                });

                theoryData.Add(new JsonWebKeyTheoryData("JsonWebKeyAdditionalData1")
                {
                    ExpectedValue = DataSets.JsonWebKeyAdditionalData1,
                    Json = DataSets.JsonWebKeyAdditionalDataString1,
                });

                theoryData.Add(new JsonWebKeyTheoryData("JsonWebKeyES256")
                {
                    ExpectedValue = DataSets.JsonWebKeyES256,
                    Json = DataSets.JsonWebKeyES256String
                });

                theoryData.Add(new JsonWebKeyTheoryData("JsonWebKeyES384")
                {
                    ExpectedValue = DataSets.JsonWebKeyES384,
                    Json = DataSets.JsonWebKeyES384String
                });

                JsonWebKey jsonWebKey = FullyPopulated();
                string json = JsonConvert.SerializeObject(jsonWebKey);
                theoryData.Add(new JsonWebKeyTheoryData("AllPropertiesSet")
                {
                    ExpectedValue = jsonWebKey,
                    Json = json
                });

                // System.Text.Json throws a JsonException with an inner of JsonReaderException that is internal.
                // We would have to use reflection to compare.
                theoryData.Add(new JsonWebKeyTheoryData("BadJson")
                {
                    ExpectedException = new ExpectedException(typeof(ArgumentException), "IDX10805: Error deserializing json: ", typeof(JsonReaderException)),
                    Json = "{adsd}",
                    JsonReaderExpectedException = new ExpectedException(typeExpected: typeof(System.Text.Json.JsonException), substringExpected: "'a' is an invalid start of a", ignoreInnerException: true),
                    JsonSerializerExpectedException = new ExpectedException(typeExpected: typeof(System.Text.Json.JsonException), substringExpected: "'a' is an invalid start of a", ignoreInnerException: true)
                });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(SerializeDataSet))]
        public void Serialize(JsonWebKeyTheoryData theoryData)
        {
            var context = new CompareContext(theoryData);
            try
            {
                string jsonNewtonsoft = JsonConvert.SerializeObject(theoryData.JsonWebKey6x);
                string jsonSerialize = System.Text.Json.JsonSerializer.Serialize<JsonWebKey>(theoryData.JsonWebKey);
                string jsonWrite = JsonWebKeySerializer.Write(theoryData.JsonWebKey);

                JsonWebKey jsonWebKeyNewtonsoft = JsonConvert.DeserializeObject<JsonWebKey>(jsonNewtonsoft);
                JsonWebKey jsonWebKeyDeserialize = System.Text.Json.JsonSerializer.Deserialize<JsonWebKey>(jsonSerialize);
                JsonWebKey jsonWebKeyRead = new JsonWebKey(jsonWrite);

                if (!IdentityComparer.AreEqual(jsonWebKeyNewtonsoft, theoryData.JsonWebKey6x, context))
                {
                    context.Diffs.Add("jsonWebKeyNewtonsoft != theoryData.JsonWebKey6x");
                    context.Diffs.Add("=========================================");
                }

                if (!IdentityComparer.AreEqual(jsonWebKeyDeserialize, theoryData.JsonWebKey, context))
                {
                    context.Diffs.Add("jsonWebKeyDeserialize != theoryData.JsonWebKey");
                    context.Diffs.Add("=========================================");
                }

                if (!IdentityComparer.AreEqual(jsonWebKeyRead, theoryData.JsonWebKey, context))
                {
                    context.Diffs.Add("jsonWebKeyRead != theoryData.JsonWebKey");
                    context.Diffs.Add("=========================================");
                }


                if (!IdentityComparer.AreEqual(jsonSerialize, jsonNewtonsoft, context))
                {
                    context.Diffs.Add("jsonSerialize != jsonNewtonsoft");
                    context.Diffs.Add("=========================================");
                }

                if (!IdentityComparer.AreEqual(jsonSerialize, jsonWrite, context))
                {
                    context.Diffs.Add("jsonSerialize != jsonWrite");
                    context.Diffs.Add("=========================================");
                }
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonWebKeyTheoryData> SerializeDataSet
        {
            get
            {
                var theoryData = new TheoryData<JsonWebKeyTheoryData>();
                theoryData.Add(new JsonWebKeyTheoryData("AllPropertiesSet")
                {
                    JsonWebKey = FullyPopulated(),
                    JsonWebKey6x = FullyPopulated6x()
                });

                return theoryData;
            }
        }

        private static JsonWebKey FullyPopulated()
        {
            JsonWebKey jsonWebKey = new JsonWebKey
            {
                Alg = SecurityAlgorithms.Sha256,
                Crv = "CRV",
                D = P256_D,
                DP = DP,
                DQ = DQ,
                E = Exponent,
                KeyId = "NGTFvdK-fythEuLwjpwAJOM9n-A",
                Kid = "NGTFvdK-fythEuLwjpwAJOM9n-A",
                Kty = "RSA",
                N = Modulus,
                P = P,
                Q = Q,
                QI = InverseQ,
                Use = "sig",
                X = P256_X,
                X5t = "NGTFvdK-fythEuLwjpwAJOM9n-A",
                X5u = "https://jsonkeyurl",
                Y = P256_Y
            };

            jsonWebKey.X5c.Add(X5C);
            jsonWebKey.KeyOps.Add("signing");
            jsonWebKey.Oth.Add("other1");
            jsonWebKey.Oth.Add("other2");

            return jsonWebKey;
        }

        private static JsonWebKey6x FullyPopulated6x()
        {
            JsonWebKey6x jsonWebKey = new JsonWebKey6x
            {
                Alg = SecurityAlgorithms.Sha256,
                Crv = "CRV",
                D = P256_D,
                DP = DP,
                DQ = DQ,
                E = Exponent,
                KeyId = "NGTFvdK-fythEuLwjpwAJOM9n-A",
                Kid = "NGTFvdK-fythEuLwjpwAJOM9n-A",
                Kty = "RSA",
                N = Modulus,
                P = P,
                Q = Q,
                QI = InverseQ,
                Use = "sig",
                X = P256_X,
                X5t = "NGTFvdK-fythEuLwjpwAJOM9n-A",
                X5u = "https://jsonkeyurl",
                Y = P256_Y
            };

            jsonWebKey.X5c.Add(X5C);
            jsonWebKey.KeyOps.Add("signing");
            jsonWebKey.Oth = new List<string> { "other1", "other2" };

            return jsonWebKey;
        }
    }
}
