// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens.Json
{
    internal static class JsonWebKeySerializer
    {
        private static string _className = typeof(JsonWebKey).FullName;

        #region Read
        public static JsonWebKey Read(string json)
        {
            ReadOnlySpan<byte> bytes = Encoding.UTF8.GetBytes(json).AsSpan();
            Utf8JsonReader reader = new Utf8JsonReader(bytes);
            return Read(ref reader);
        }

        /// <summary>
        /// Reads a JsonWebKey.
        /// </summary>
        /// <param name="reader">a <see cref="Utf8JsonReader"/> pointing at a StartObject.</param>
        /// <returns>A <see cref="JsonWebKey"/>.</returns>
        public static JsonWebKey Read(ref Utf8JsonReader reader)
        {
            JsonWebKey jsonWebKey = new JsonWebKey();
            Read(ref reader, jsonWebKey);
            return jsonWebKey;
        }

        /// <summary>
        /// Reads a JsonWebKey.
        /// </summary>
        /// <param name="json">.</param>
        /// <param name="jsonWebKey"></param>
        /// <returns>A <see cref="JsonWebKey"/>.</returns>
        public static void Read(string json, JsonWebKey jsonWebKey)
        {
            ReadOnlySpan<byte> bytes = Encoding.UTF8.GetBytes(json).AsSpan();
            Utf8JsonReader reader = new Utf8JsonReader(bytes);
            Read(ref reader, jsonWebKey);
        }

        /// <summary>
        /// Reads a JsonWebKey. see: https://datatracker.ietf.org/doc/html/rfc7517
        /// </summary>
        /// <param name="reader">a <see cref="Utf8JsonReader"/> pointing at a StartObject.</param>
        /// <param name="jsonWebKey"></param>
        /// <returns>A <see cref="JsonWebKey"/>.</returns>
        public static void Read(ref Utf8JsonReader reader, JsonWebKey jsonWebKey)
        {
            if (!JsonSerializerPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.StartObject, true))
                throw LogHelper.LogExceptionMessage(
                    new JsonException(
                        LogHelper.FormatInvariant(
                        LogMessages.IDX11022,
                        LogHelper.MarkAsNonPII("JsonTokenType.StartObject"),
                        LogHelper.MarkAsNonPII(reader.TokenType),
                        LogHelper.MarkAsNonPII(JsonWebKey._className),
                        LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                        LogHelper.MarkAsNonPII(reader.CurrentDepth),
                        LogHelper.MarkAsNonPII(reader.BytesConsumed))));

            do
            {
                while (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = JsonSerializerPrimitives.GetPropertyName(ref reader, JsonWebKey._className, true);
                    switch (propertyName)
                    {
                        // optional
                        // https://datatracker.ietf.org/doc/html/rfc7517#section-4.4
                        case JsonWebKeyParameterNames.Alg:
                            jsonWebKey.Alg = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.Alg, _className);
                            break;
                        // required if the key is EC
                        case JsonWebKeyParameterNames.Crv:
                            jsonWebKey.Crv = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.Crv, _className);
                            break;
                        // https://datatracker.ietf.org/doc/html/rfc7518#section-6.2.2.1
                        // The length of this octet string MUST be ceiling(log-base-2(n)/8) octets (where n is the order of the curve).
                        case JsonWebKeyParameterNames.D:
                            jsonWebKey.D = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.D, _className);
                            break;
                        case JsonWebKeyParameterNames.DP:
                            jsonWebKey.DP = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.DP, _className);
                            break;
                        case JsonWebKeyParameterNames.DQ:
                            jsonWebKey.DQ = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.DQ, _className);
                            break;

                        // required for RSA keys
                        // https://datatracker.ietf.org/doc/html/rfc7518#section-6.3.1
                        case JsonWebKeyParameterNames.E:
                            jsonWebKey.E = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.E, _className);
                            break;
                        case JsonWebKeyParameterNames.K:
                            jsonWebKey.K = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.K, _className);
                            break;
                        // Optional
                        // https://datatracker.ietf.org/doc/html/rfc7517#section-4.3
                        // no duplicates in array
                        // case sensitive
                        case JsonWebKeyParameterNames.KeyOps:
                            // the value can be null if the value is 'nill'
                            if (JsonSerializerPrimitives.ReadStrings(ref reader, jsonWebKey.KeyOps, JsonWebKeyParameterNames.KeyOps, _className, false) == null)
                            {
                                throw LogHelper.LogExceptionMessage(
                                    new ArgumentNullException(
                                        JsonWebKeyParameterNames.KeyOps,
                                        new JsonException(
                                            LogHelper.FormatInvariant(
                                            LogMessages.IDX11022,
                                            LogHelper.MarkAsNonPII(reader.TokenType),
                                            LogHelper.MarkAsNonPII(JsonWebKey._className),
                                            LogHelper.MarkAsNonPII(propertyName),
                                            LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                                            LogHelper.MarkAsNonPII(reader.CurrentDepth),
                                            LogHelper.MarkAsNonPII(reader.BytesConsumed)))));
                            }
                            break;
                        case JsonWebKeyParameterNames.Keys:
                            // TODO additional data
                            break;
                        // optional
                        // https://datatracker.ietf.org/doc/html/rfc7517#section-4.5
                        // string
                        case JsonWebKeyParameterNames.Kid:
                            jsonWebKey.Kid = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.Kid, _className);
                            break;
                        // required
                        case JsonWebKeyParameterNames.Kty:
                            jsonWebKey.Kty = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.Kty, _className);
                            break;
                        case JsonWebKeyParameterNames.N:
                            jsonWebKey.N = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.N, _className);
                            break;
                        // optional
                        // https://datatracker.ietf.org/doc/html/rfc7518#section-6.3.2.7
                        case JsonWebKeyParameterNames.Oth:
                            JsonSerializerPrimitives.ReadStrings(ref reader, jsonWebKey.Oth, JsonWebKeyParameterNames.Oth, _className, false);
                            break;
                        case JsonWebKeyParameterNames.P:
                            jsonWebKey.P = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.P, _className);
                            break;
                        case JsonWebKeyParameterNames.Q:
                            jsonWebKey.Q = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.Q, _className);
                            break;
                        case JsonWebKeyParameterNames.QI:
                            jsonWebKey.QI = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.QI, _className);
                            break;
                        // optional
                        case JsonWebKeyParameterNames.Use:
                            jsonWebKey.Use = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.Use, _className);
                            break;
                        // required if the key is EC
                        case JsonWebKeyParameterNames.X:
                            jsonWebKey.X = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.X, _className);
                            break;
                        // optional
                        // https://datatracker.ietf.org/doc/html/rfc7517#section-4.7
                        case JsonWebKeyParameterNames.X5c:
                            JsonSerializerPrimitives.ReadStrings(ref reader, jsonWebKey.X5c, JsonWebKeyParameterNames.X5c, _className, false);
                            break;
                        // optional
                        // https://datatracker.ietf.org/doc/html/rfc7517#section-4.8
                        case JsonWebKeyParameterNames.X5t:
                            jsonWebKey.X5t = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.X5t, _className);
                            break;
                        // optional
                        // https://datatracker.ietf.org/doc/html/rfc7517#section-4.9
                        case JsonWebKeyParameterNames.X5tS256:
                            jsonWebKey.X5t = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.X5tS256, _className);
                            break;
                        // optional
                        // https://datatracker.ietf.org/doc/html/rfc7517#section-4.6
                        case JsonWebKeyParameterNames.X5u:
                            jsonWebKey.X5u = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.X5u, _className);
                            break;
                        // required if the key is EC
                        case JsonWebKeyParameterNames.Y:
                            jsonWebKey.Y = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.Y, _className);
                            break;
                        default:
                            using (JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader))
                                jsonWebKey.AdditionalData.Add(propertyName, jsonDocument.RootElement.Clone());

                            reader.Read();
                            break;
                    }
                }

                if (JsonSerializerPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.EndObject, true))
                    break;

            } while (reader.Read());

            return;
        }
        #endregion

        #region Write
        public static string Write(JsonWebKey jsonWebKey)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                JsonWriterOptions jsonWriterOptions = new JsonWriterOptions();
                using (Utf8JsonWriter writer = new Utf8JsonWriter(memoryStream, jsonWriterOptions))
                {
                    Write(writer, jsonWebKey);
                    writer.Flush();
                    return UTF8Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
        }

        public static void Write(Utf8JsonWriter writer, JsonWebKey jsonWebKey)
        {
            _ = jsonWebKey ?? throw new ArgumentNullException(nameof(jsonWebKey));
            _ = writer ?? throw new ArgumentNullException(nameof(writer));

            writer.WriteStartObject();
            if (!string.IsNullOrEmpty(jsonWebKey.Alg))
                writer.WriteString(JsonWebKeyParameterNames.Alg, jsonWebKey.Alg);

            if (!string.IsNullOrEmpty(jsonWebKey.Crv))
                writer.WriteString(JsonWebKeyParameterNames.Crv, jsonWebKey.Crv);

            if (!string.IsNullOrEmpty(jsonWebKey.D))
                writer.WriteString(JsonWebKeyParameterNames.D, jsonWebKey.D);

            if (!string.IsNullOrEmpty(jsonWebKey.DP))
                writer.WriteString(JsonWebKeyParameterNames.DP, jsonWebKey.DP);

            if (!string.IsNullOrEmpty(jsonWebKey.DQ))
                writer.WriteString(JsonWebKeyParameterNames.DQ, jsonWebKey.DQ);

            if (!string.IsNullOrEmpty(jsonWebKey.E))
                writer.WriteString(JsonWebKeyParameterNames.E, jsonWebKey.E);

            if (!string.IsNullOrEmpty(jsonWebKey.K))
                writer.WriteString(JsonWebKeyParameterNames.K, jsonWebKey.K);

            if (jsonWebKey.KeyOps.Count > 0)
                JsonSerializerPrimitives.WriteStrings(ref writer, JsonWebKeyParameterNames.KeyOps, jsonWebKey.KeyOps);

            if (!string.IsNullOrEmpty(jsonWebKey.Kid))
                writer.WriteString(JsonWebKeyParameterNames.Kid, jsonWebKey.Kid);

            if (!string.IsNullOrEmpty(jsonWebKey.Kty))
                writer.WriteString(JsonWebKeyParameterNames.Kty, jsonWebKey.Kty);

            if (!string.IsNullOrEmpty(jsonWebKey.N))
                writer.WriteString(JsonWebKeyParameterNames.N, jsonWebKey.N);

            if (jsonWebKey.Oth.Count > 0)
                JsonSerializerPrimitives.WriteStrings(ref writer, JsonWebKeyParameterNames.Oth, jsonWebKey.Oth);

            if (!string.IsNullOrEmpty(jsonWebKey.P))
                writer.WriteString(JsonWebKeyParameterNames.P, jsonWebKey.P);

            if (!string.IsNullOrEmpty(jsonWebKey.Q))
                writer.WriteString(JsonWebKeyParameterNames.Q, jsonWebKey.Q);

            if (!string.IsNullOrEmpty(jsonWebKey.QI))
                writer.WriteString(JsonWebKeyParameterNames.QI, jsonWebKey.QI);

            if (!string.IsNullOrEmpty(jsonWebKey.Use))
                writer.WriteString(JsonWebKeyParameterNames.Use, jsonWebKey.Use);

            if (!string.IsNullOrEmpty(jsonWebKey.X))
                writer.WriteString(JsonWebKeyParameterNames.X, jsonWebKey.X);

            if (jsonWebKey.X5c.Count > 0)
                JsonSerializerPrimitives.WriteStrings(ref writer, JsonWebKeyParameterNames.X5c, jsonWebKey.X5c);

            if (!string.IsNullOrEmpty(jsonWebKey.X5t))
                writer.WriteString(JsonWebKeyParameterNames.X5t, jsonWebKey.X5t);

            if (!string.IsNullOrEmpty(jsonWebKey.X5u))
                writer.WriteString(JsonWebKeyParameterNames.X5u, jsonWebKey.X5u);

            if (!string.IsNullOrEmpty(jsonWebKey.Y))
                writer.WriteString(JsonWebKeyParameterNames.Y, jsonWebKey.Y);

            writer.WriteEndObject();
        }
        #endregion
    }
}

