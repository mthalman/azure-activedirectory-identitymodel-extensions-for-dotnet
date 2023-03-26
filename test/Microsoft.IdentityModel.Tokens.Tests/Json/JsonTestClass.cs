// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
#if NET8
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;

namespace Microsoft.IdentityModel.Tokens.Json.Tests
{
    public class JsonTestClass
    {
        public static string _className = typeof(JsonTestClass).ToString();
        private IList<object> _listObject;
        private IList<string> _arrayString;

        /// <summary>
        /// When deserializing from JSON any properties that are not defined will be placed here.
        /// </summary>
        [Microsoft.IdentityModel.Json.JsonExtensionData(ReadData = true, WriteData = true)]
        [Newtonsoft.Json.JsonExtensionData(ReadData = true, WriteData = true)]
        [System.Text.Json.Serialization.JsonExtensionData]
        public virtual IDictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();

#if NET6_0_OR_GREATER
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public bool? Boolean { get; set; }

#if NET6_0_OR_GREATER
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public double? Double { get; set; }

#if NET6_0_OR_GREATER
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public int? Int { get; set; }

#if NET6_0_OR_GREATER
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public IList<object> ListObject
        {
            get { return _listObject; }
            set { _listObject = value ?? throw LogHelper.LogArgumentNullException(nameof(value)); }
        }

#if NET6_0_OR_GREATER
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public IList<string> ListString
        {
            get { return _arrayString; }
            set { _arrayString = value ?? throw LogHelper.LogArgumentNullException(nameof(value)); }
        }

#if NET6_0_OR_GREATER
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
#endif
        public string String { get; set; }

        public bool ShouldSerializeAdditionalData()
        {
            return AdditionalData.Count > 0;
        }

        public bool ShouldSerializeBoolean()
        {
            return Boolean.HasValue;
        }

        public bool ShouldSerializeDouble()
        {
            return Double.HasValue;
        }
        public bool ShouldSerializeInt()
        {
            return Int.HasValue;
        }
        public bool ShouldSerializeListObject()
        {
            return ListObject != null && ListObject.Count > 0;
        }

        public bool ShouldSerializeListString()
        {
            return ListString != null && ListString.Count > 0;
        }

        public bool ShouldSerializeString()
        {
            return String != null;
        }
    }
}
#endif
