// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    public class OdcmCapability<T> : OdcmCapability
    {
        public T Value { get; set; }

        public OdcmCapability(T value, string termName) : base(termName)
        {
            Value = value;
        }
    }

    public class OdcmCapability
    {
        public string TermName { get; private set; }

        public string ExternalTermName { get; private set; }

        public OdcmCapability(string termName)
        {
            TermName = termName;
        }

        private static ICollection<OdcmCapability> GetAllOdcmCapabilities()
        {
            return new List<OdcmCapability>();
        }

        /// <summary>
        /// Default list of OdcmCapabilities supported in the OdcmModel.
        /// </summary>
        public static ICollection<OdcmCapability> DefaultOdcmCapabilities
        {
            get
            {
                return GetAllOdcmCapabilities();
            }
        }

        public static ICollection<OdcmCapability> DefaultPropertyCapabilities
        {
            get
            {
                return GetAllOdcmCapabilities();
            }
        }

        public static ICollection<OdcmCapability> DefaultSingletonCapabilities
        {
            get
            {
                return new List<OdcmCapability>();
            }
        }

        public static ICollection<OdcmCapability> DefaultEntitySetCapabilities
        {
            get
            {
                return new List<OdcmCapability>();
            }
        }
    }
}
