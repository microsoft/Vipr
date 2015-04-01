// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    public abstract partial class OdcmCapability : IEquatable<OdcmCapability>
    {
        public abstract string TermName { get; }

        public abstract bool Equals(OdcmCapability otherCapability);

        private static List<OdcmCapability> _defaultOdcmCapabilities;

        /// <summary>
        /// Default list of OdcmCapabilities supported in the OdcmModel.
        /// </summary>
        public static List<OdcmCapability> DefaultOdcmCapabilities
        {
            get
            {
                if (_defaultOdcmCapabilities == null)
                {
                    _defaultOdcmCapabilities = new List<OdcmCapability>();
                    var capabilityTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(OdcmCapability)) && !t.IsAbstract);

                    foreach (var capabilityType in capabilityTypes)
                    {
                        var capability = (OdcmCapability)Activator.CreateInstance(capabilityType);
                        _defaultOdcmCapabilities.Add(capability);
                    }
                }

                return _defaultOdcmCapabilities;
            }
        }
    }
}
