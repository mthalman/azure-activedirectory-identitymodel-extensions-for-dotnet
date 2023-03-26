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

namespace Microsoft.IdentityModel.Tokens.Json.Tests
{
    internal static class JsonTestClassSerializer
    {
        private static string _className = typeof(JsonTestClass).FullName;

        #region Read
        public static JsonTestClass Read(string json)
        {
            ReadOnlySpan<byte> bytes = Encoding.UTF8.GetBytes(json).AsSpan();
            Utf8JsonReader reader = new Utf8JsonReader(bytes);
            return Read(ref reader);
        }

        /// <summary>
        /// Reads a JsonTestClass.
        /// </summary>
        /// <param name="reader">a <see cref="Utf8JsonReader"/> pointing at a StartObject.</param>
        /// <returns>A <see cref="JsonTestClass"/>.</returns>
        public static JsonTestClass Read(ref Utf8JsonReader reader)
        {
            JsonTestClass jsonWebKey = new JsonTestClass();
            Read(ref reader, jsonWebKey);
            return jsonWebKey;
        }

        /// <summary>
        /// Reads a JsonTestClass.
        /// </summary>
        /// <param name="json">.</param>
        /// <param name="jsonWebKey"></param>
        /// <returns>A <see cref="JsonTestClass"/>.</returns>
        public static void Read(string json, JsonTestClass jsonWebKey)
        {
            ReadOnlySpan<byte> bytes = Encoding.UTF8.GetBytes(json).AsSpan();
            Utf8JsonReader reader = new Utf8JsonReader(bytes);
            Read(ref reader, jsonWebKey);
        }

        /// <summary>
        /// Reads a JsonWebKeyNet8. see: https://datatracker.ietf.org/doc/html/rfc7517
        /// </summary>
        /// <param name="reader">a <see cref="Utf8JsonReader"/> pointing at a StartObject.</param>
        /// <param name="jsonTestClass"></param>
        /// <returns>A <see cref="JsonWebKeyNet8"/>.</returns>
        public static void Read(ref Utf8JsonReader reader, JsonTestClass jsonTestClass)
        {
            if (!JsonSerializerPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.StartObject, true))
                throw LogHelper.LogExceptionMessage(
                    new JsonException(
                        LogHelper.FormatInvariant(
                        LogMessages.IDX11022,
                        LogHelper.MarkAsNonPII("JsonTokenType.StartObject"),
                        LogHelper.MarkAsNonPII(reader.TokenType),
                        LogHelper.MarkAsNonPII(JsonWebKeyNet8._className),
                        LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                        LogHelper.MarkAsNonPII(reader.CurrentDepth),
                        LogHelper.MarkAsNonPII(reader.BytesConsumed))));

            do
            {
                while (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = JsonSerializerPrimitives.GetPropertyName(ref reader, JsonTestClass._className, true);
                    switch (propertyName)
                    {
                        // optional
                        // https://datatracker.ietf.org/doc/html/rfc7517#section-4.4
                        case "Boolean":
                            jsonTestClass.Boolean = JsonSerializerPrimitives.ReadBoolean(ref reader, "Boolean",_className);
                            break;
                        case "Double":
                            jsonTestClass.Double = JsonSerializerPrimitives.ReadDouble(ref reader, "Double", _className);
                            break;
                        case "Int":
                            jsonTestClass.Int = JsonSerializerPrimitives.ReadInt(ref reader, "Int", _className);
                            break;
                        case "ListObject":
                            if (JsonSerializerPrimitives.ReadObjects(ref reader, jsonTestClass.ListObject, "ListObject", _className) == null)
                            {
                                throw LogHelper.LogExceptionMessage(
                                    new ArgumentNullException(
                                        "ListString",
                                        new JsonException(
                                            LogHelper.FormatInvariant(
                                            LogMessages.IDX11022,
                                            LogHelper.MarkAsNonPII(reader.TokenType),
                                            LogHelper.MarkAsNonPII(JsonWebKeyNet8._className),
                                            LogHelper.MarkAsNonPII(propertyName),
                                            LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                                            LogHelper.MarkAsNonPII(reader.CurrentDepth),
                                            LogHelper.MarkAsNonPII(reader.BytesConsumed)))));
                            }
                            break;
                        case "ListString":
                            if (JsonSerializerPrimitives.ReadStrings(ref reader, jsonTestClass.ListString, "ListString", _className) == null)
                            {
                                throw LogHelper.LogExceptionMessage(
                                    new ArgumentNullException(
                                        "ListString",
                                        new JsonException(
                                            LogHelper.FormatInvariant(
                                            LogMessages.IDX11022,
                                            LogHelper.MarkAsNonPII(reader.TokenType),
                                            LogHelper.MarkAsNonPII(JsonWebKeyNet8._className),
                                            LogHelper.MarkAsNonPII(propertyName),
                                            LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                                            LogHelper.MarkAsNonPII(reader.CurrentDepth),
                                            LogHelper.MarkAsNonPII(reader.BytesConsumed)))));
                            }
                            break;
                        case "String":
                            jsonTestClass.String = JsonSerializerPrimitives.ReadString(ref reader, "String", _className);
                            break;
                        default:
                            using (JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader))
                                jsonTestClass.AdditionalData.Add(propertyName, jsonDocument.RootElement.Clone());

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
        public static string Write(JsonTestClass jsonWebKey)
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

        public static void Write(Utf8JsonWriter writer, JsonTestClass jsonTestClass)
        {
            _ = jsonTestClass ?? throw new ArgumentNullException(nameof(jsonTestClass));
            _ = writer ?? throw new ArgumentNullException(nameof(writer));

            writer.WriteStartObject();

            if (jsonTestClass.Boolean.HasValue)
                writer.WriteBoolean("Boolean", jsonTestClass.Boolean.Value);

            if (jsonTestClass.Double.HasValue)
                writer.WriteNumber("Double", jsonTestClass.Double.Value);

            if (jsonTestClass.Int.HasValue)
                writer.WriteNumber("Int", jsonTestClass.Int.Value);

            if (jsonTestClass.ListObject != null && jsonTestClass.ListObject.Count > 0)
            {
                writer.WriteStartArray("ListObject");
                foreach (var item in jsonTestClass.ListObject)
                {
                    if (item == null)
                        writer.WriteNullValue();
                    else
                        JsonSerializer.Serialize(writer, item);
                }

                writer.WriteEndArray();
            }

            if (jsonTestClass.ListString != null && jsonTestClass.ListString.Count > 0)
            {
                writer.WriteStartArray("ListString");

                foreach (var item in jsonTestClass.ListString)
                    writer.WriteStringValue(item);

                writer.WriteEndArray();
            }

            if (!string.IsNullOrEmpty(jsonTestClass.String))
                writer.WriteString("String", jsonTestClass.String);

            if (jsonTestClass.AdditionalData != null && jsonTestClass.AdditionalData.Count > 0)
            {
                foreach (var item in jsonTestClass.AdditionalData)
                {
                    string str = JsonSerializer.Serialize(item.Value);
#if NET6_0
                    writer.WritePropertyName(item.Key);
                    writer.WriteRawValue(str);
#endif
                }
            }

            writer.WriteEndObject();
        }
        #endregion
    }
}
#endif // #if NET6_0
