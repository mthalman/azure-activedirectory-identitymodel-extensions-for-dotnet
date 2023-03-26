// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NET8
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text.Json;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens.Json
{
    internal static class JsonSerializerPrimitives
    {
        internal static string GetPropertyName(ref Utf8JsonReader reader, string className, bool advanceReader)
        {
            if (reader.TokenType == JsonTokenType.None)
                ReaderRead(ref reader);

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw LogHelper.LogExceptionMessage(
                    new JsonException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX11020,
                            LogHelper.MarkAsNonPII("JsonTokenType.PropertyName"),
                            LogHelper.MarkAsNonPII(reader.TokenType),
                            LogHelper.MarkAsNonPII(className),
                            LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                            LogHelper.MarkAsNonPII(reader.CurrentDepth),
                            LogHelper.MarkAsNonPII(reader.BytesConsumed))));
            }

            if (advanceReader)
            {
                string propertyName = reader.GetString();
                ReaderRead(ref reader);
                return propertyName;
            }

            return reader.GetString();
        }

        internal static bool IsReaderAtTokenType(ref Utf8JsonReader reader, JsonTokenType tokenType, bool advanceReader)
        {
            if (reader.TokenType == JsonTokenType.None)
                ReaderRead(ref reader);

            if (reader.TokenType != tokenType)
                return false;

            if (advanceReader)
                ReaderRead(ref reader);

            return true;
        }

        internal static bool ReaderRead(ref Utf8JsonReader reader)
        {
            try
            {
                return reader.Read();
            }
            catch (JsonException ex)
            {
                throw new JsonException(ex.Message, ex);
            }
        }

        internal static bool ReadBoolean(ref Utf8JsonReader reader, string propertyName, string className)
        {
            if (reader.TokenType == JsonTokenType.True || reader.TokenType == JsonTokenType.False)
            {
                bool retVal = reader.GetBoolean();
                ReaderRead(ref reader);
                return retVal;
            }

            throw LogHelper.LogExceptionMessage(
                new JsonException(
                    LogHelper.FormatInvariant(
                    LogMessages.IDX11020,
                    LogHelper.MarkAsNonPII($"{JsonTokenType.False} or {JsonTokenType.True}"),
                    LogHelper.MarkAsNonPII(reader.TokenType),
                    LogHelper.MarkAsNonPII(className),
                    LogHelper.MarkAsNonPII(propertyName),
                    LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                    LogHelper.MarkAsNonPII(reader.CurrentDepth),
                    LogHelper.MarkAsNonPII(reader.BytesConsumed))));
        }

        internal static double ReadDouble(ref Utf8JsonReader reader, string propertyName, string className)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                double retVal = reader.GetDouble();
                ReaderRead(ref reader);
                return retVal;
            }

            throw LogHelper.LogExceptionMessage(
                new JsonException(
                    LogHelper.FormatInvariant(
                    LogMessages.IDX11020,
                    LogHelper.MarkAsNonPII(reader.TokenType),
                    LogHelper.MarkAsNonPII(JsonTokenType.Number),
                    LogHelper.MarkAsNonPII(className),
                    LogHelper.MarkAsNonPII(propertyName),
                    LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                    LogHelper.MarkAsNonPII(reader.CurrentDepth),
                    LogHelper.MarkAsNonPII(reader.BytesConsumed))));
        }

        internal static int ReadInt(ref Utf8JsonReader reader, string propertyName, string className)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                int retVal = reader.GetInt32();
                ReaderRead(ref reader);
                return retVal;
            }

            throw LogHelper.LogExceptionMessage(
                new JsonException(
                    LogHelper.FormatInvariant(
                    LogMessages.IDX11020,
                    LogHelper.MarkAsNonPII(reader.TokenType),
                    LogHelper.MarkAsNonPII(JsonTokenType.Number),
                    LogHelper.MarkAsNonPII(className),
                    LogHelper.MarkAsNonPII(propertyName),
                    LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                    LogHelper.MarkAsNonPII(reader.CurrentDepth),
                    LogHelper.MarkAsNonPII(reader.BytesConsumed))));
        }

        internal static object ReadObject(ref Utf8JsonReader reader, bool advanceReader = true)
        {
            object retVal = null;

            using (JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader))
               retVal = jsonDocument.RootElement.Clone();

            if (advanceReader)
                ReaderRead(ref reader);

            return retVal;
        }

        internal static IList<object> ReadObjects(ref Utf8JsonReader reader, IList<object> objects, string propertyName, string className)
        {
            _ = objects ?? throw new ArgumentNullException(nameof(objects));

            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (!IsReaderAtTokenType(ref reader, JsonTokenType.StartArray, true))
            {
                throw LogHelper.LogExceptionMessage(
                    new JsonException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX11020,
                            LogHelper.MarkAsNonPII("JsonTokenType.StartArray"),
                            LogHelper.MarkAsNonPII(reader.TokenType),
                            LogHelper.MarkAsNonPII(className),
                            LogHelper.MarkAsNonPII(propertyName),
                            LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                            LogHelper.MarkAsNonPII(reader.CurrentDepth),
                            LogHelper.MarkAsNonPII(reader.BytesConsumed))));
            }

            do
            {
                if (IsReaderAtTokenType(ref reader, JsonTokenType.EndArray, true))
                    break;

                objects.Add(ReadObject(ref reader, false));

            } while (ReaderRead(ref reader));

            return objects;
        }

        internal static string ReadString(ref Utf8JsonReader reader, string propertyName, string className, bool advanceReader = true)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (reader.TokenType != JsonTokenType.String)
            {
                throw LogHelper.LogExceptionMessage(
                    new JsonException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX11020,
                            LogHelper.MarkAsNonPII(reader.TokenType),
                            LogHelper.MarkAsNonPII(JsonTokenType.String),
                            LogHelper.MarkAsNonPII(className),
                            LogHelper.MarkAsNonPII(propertyName),
                            LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                            LogHelper.MarkAsNonPII(reader.CurrentDepth),
                            LogHelper.MarkAsNonPII(reader.BytesConsumed))));
            }

            string retVal = reader.GetString();
            if (advanceReader)
                ReaderRead(ref reader);

            return retVal;
        }

        internal static IList<string> ReadStrings(ref Utf8JsonReader reader, IList<string> strings, string propertyName, string className)
        {
            _ = strings ?? throw new ArgumentNullException(nameof(strings));

            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (!IsReaderAtTokenType(ref reader, JsonTokenType.StartArray, true))
            {
                throw LogHelper.LogExceptionMessage(
                    new JsonException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX11020,
                            LogHelper.MarkAsNonPII("JsonTokenType.StartArray"),
                            LogHelper.MarkAsNonPII(reader.TokenType),
                            LogHelper.MarkAsNonPII(className),
                            LogHelper.MarkAsNonPII(propertyName),
                            LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                            LogHelper.MarkAsNonPII(reader.CurrentDepth),
                            LogHelper.MarkAsNonPII(reader.BytesConsumed))));
            }

            do
            {
                if (IsReaderAtTokenType(ref reader, JsonTokenType.EndArray, true))
                    break;

                strings.Add(ReadString(ref reader, propertyName, className, false));

            } while (ReaderRead(ref reader));

            return strings;
        }

        internal static void WriteStrings(ref Utf8JsonWriter writer, string propertyName, IList<string> strings)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            _ = strings ?? throw new ArgumentNullException(nameof(strings));

            writer.WritePropertyName(propertyName);
            writer.WriteStartArray();
            foreach (string str in strings)
                writer.WriteStringValue(str);

            writer.WriteEndArray();
        }
    }
}
#endif
