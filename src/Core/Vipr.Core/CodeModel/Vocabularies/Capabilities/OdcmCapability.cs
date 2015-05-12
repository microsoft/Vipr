// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    public abstract class OdcmCapability : IEquatable<OdcmCapability>
    {
        public abstract string TermName { get; }

        public abstract bool Equals(OdcmCapability otherCapability);

        private static List<OdcmCapability> GetAllOdcmCapabilities()
        {
            var defaultOdcmCapabilities = new List<OdcmCapability>();
            var capabilityTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(OdcmCapability)) && !t.IsAbstract);

            foreach (var capabilityType in capabilityTypes)
            {
                var capability = (OdcmCapability)Activator.CreateInstance(capabilityType);
                defaultOdcmCapabilities.Add(capability);
            }

            return defaultOdcmCapabilities;
        }

        /// <summary>
        /// Default list of OdcmCapabilities supported in the OdcmModel.
        /// </summary>
        public static IEnumerable<OdcmCapability> DefaultOdcmCapabilities
        {
            get
            {
                return GetAllOdcmCapabilities();
            }
        }

        public static IEnumerable<OdcmCapability> DefaultPropertyCapabilities
        {
            get
            {
                return GetAllOdcmCapabilities();
            }
        }

        public static IEnumerable<OdcmCapability> DefaultSingletonCapabilities
        {
            get
            {
                var defaultSingletonCapabilities = GetAllOdcmCapabilities();

                foreach (var capability in defaultSingletonCapabilities)
                {
                    if (capability is OdcmDeleteCapability || capability is OdcmUpdateLinkCapability ||
                        capability is OdcmDeleteLinkCapability)
                    {
                        (capability as OdcmBooleanCapability).Value = false;
                    }
                }

                return defaultSingletonCapabilities;
            }
        }

        public static IEnumerable<OdcmCapability> DefaultEntitySetCapabilities
        {
            get
            {
                var defaultEntitySetCapabilities = GetAllOdcmCapabilities();

                foreach (var capability in defaultEntitySetCapabilities)
                {
                    if (capability is OdcmUpdateLinkCapability ||
                        capability is OdcmDeleteLinkCapability)
                    {
                        (capability as OdcmBooleanCapability).Value = false;
                    }
                }

                return defaultEntitySetCapabilities;
            }
        }
    }
}
