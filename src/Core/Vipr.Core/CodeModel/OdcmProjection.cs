// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;

namespace Vipr.Core.CodeModel
{
    public class OdcmProjection
    {
        public delegate OdcmCapability DefaultProvider(OdcmObject odcmObject, string termName);

        private static IDictionary<string, DefaultProvider> _wellKnownCapabilities = new Dictionary<string, DefaultProvider>()
        {
            { TermNames.Update, GetDefaultBooleanCapability },
            { TermNames.UpdateLink, GetDefaultNegativeLinkCapability },
            { TermNames.Delete, GetDefaultBooleanCapability },
            { TermNames.DeleteLink, GetDefaultNegativeLinkCapability },
            { TermNames.Insert, GetDefaultBooleanCapability },
            { TermNames.Expand, GetDefaultBooleanCapability },
        };

        public static DefaultProvider UserDefaultCapabilityProvider { get; set; }

        public static INameMapper NameMapper { get; set; }

        private static OdcmCapability GetDefaultNegativeLinkCapability(OdcmObject odcmObject, string term)
        {
            bool defaultValue = true;

            if (odcmObject != null &&
                odcmObject is OdcmSingleton || odcmObject is OdcmEntitySet)
            {
                defaultValue = false;
            }

            return new OdcmBooleanCapability(defaultValue, term);
        }

        private static OdcmCapability GetDefaultBooleanCapability(OdcmObject odcmObject, string term)
        {
            bool defaultValue = true;

            if (odcmObject != null && odcmObject is OdcmSingleton)
            {
                switch (term)
                {
                    case TermNames.Delete:
                        defaultValue = false;
                        break;
                }
            }
            return new OdcmBooleanCapability(defaultValue, term);
        }

        public OdcmType Type { get; set; }

        public OdcmObject BackLink { get; set; }

        public ICollection<OdcmCapability> Capabilities { get; set; }

        // Get projection unique key based on well-known boolean capabilities
        public string Key
        {
            get
            {
                return GetUniqueProjectionName(Capabilities, BackLink);
            }
        }

        internal static string GetUniqueProjectionName(IEnumerable<OdcmCapability> capabilities, OdcmObject odcmObject)
        {
            string name = string.Empty;

            var allCapabilities = GetWellKnownCapabilities(odcmObject, capabilities)
                                    .OrderBy(x => x.TermName);

            foreach (OdcmBooleanCapability c in allCapabilities)
            {
                name += ToInternal(c.TermName) + c.Value.ToString();
            }

            return name;
        }

        public IEnumerable<OdcmCapability> WellKnownCapabilities
        {
            get
            {
                var allCapabilities = GetWellKnownCapabilities(BackLink, Capabilities)
                                    .Where(x => x is OdcmBooleanCapability)
                                    .Select(x => x as OdcmBooleanCapability);

                return allCapabilities;
            }
        }

        public static IEnumerable<string> WellKnowCapabilityTerms
        {
            get
            {
                return _wellKnownCapabilities.Keys;
            }
        }

        internal static IEnumerable<OdcmCapability> GetWellKnownCapabilities(OdcmObject odcmProperty, IEnumerable<OdcmCapability> capabilities)
        {
            foreach (var term in _wellKnownCapabilities.Keys)
            {
                var c = capabilities.SingleOrDefault(FuncFromTerm(term));

                yield return c ?? DefaultCapability(odcmProperty, term);
            }
        }

        public bool IsOneOf(string term)
        {
            return Capabilities.SingleOrDefault(FuncFromTerm(term)) != null;
        }

        public bool? BooleanValueOf(string term, OdcmObject odcmObject = null)
        {
            return FindCapabilityValue<OdcmBooleanCapability, bool?>(term, odcmObject);
        }

        public string StringValueOf(string term, OdcmObject odcmObject = null)
        {
            return FindCapabilityValue<OdcmStringCapability, string>(term, odcmObject);
        }

        public IEnumerable<object> CollectionValueOf(string term, OdcmObject odcmObject = null)
        {
            return FindCapabilityValue<OdcmCollectionCapability, IList<object>>(term, odcmObject);
        }

        public IEnumerable<string> EnumValueOf(string term, OdcmObject odcmObject = null)
        {
            return FindCapabilityValue<OdcmEnumCapability, IEnumerable<string>>(term, odcmObject);
        }

        public IEnumerable<string> StringCollectionValueOf(string term, OdcmObject odcmObject = null)
        {
            var capability = FindCapability<OdcmCollectionCapability>(term, odcmObject);
            return capability?.Value.Select(x => x as string);
        }

        public bool? Supports(string term, OdcmObject odcmObject = null)
        {
            return BooleanValueOf(term, odcmObject);
        }

        public bool SupportsUpdate()
        {
            return Supports(TermNames.Update) != false;
        }

        public bool SupportsUpdateLink()
        {
            return Supports(TermNames.UpdateLink) != false;
        }

        public bool SupportsDelete()
        {
            return Supports(TermNames.Delete) != false;
        }

        public bool SupportsDeleteLink()
        {
            return Supports(TermNames.DeleteLink) != false;
        }

        public bool SupportsInsert()
        {
            return Supports(TermNames.Insert) != false;
        }

        public bool SupportsExpand()
        {
            return Supports(TermNames.Expand) != false;
        }

        public T GetCapability<T>() where T : OdcmCapability
        {
            return Capabilities.SingleOrDefault(c => c.GetType() == typeof(T)) as T;
        }

        private static OdcmCapability DefaultCapability(OdcmObject odcmObject, string term)
        {
            OdcmCapability result = UserDefaultCapabilityProvider?.Invoke(odcmObject, term);

            if (result == null)
            {
                DefaultProvider func;
                if (_wellKnownCapabilities.TryGetValue(term, out func))
                {
                    result = func(odcmObject, term);
                }
            }
            return result;
        }

        private U FindCapabilityValue<T, U>(string term, OdcmObject odcmObject = null) where T : OdcmCapability<U>
        {
            var capability = FindCapability<T>(term, odcmObject);
            if (capability != null)
            {
                return capability.Value;
            }

            return default(U);
        }

        private T FindCapability<T>(string term, OdcmObject odcmObject = null) where T : OdcmCapability
        {
            var capability = (T) Capabilities
                                .SingleOrDefault(FuncFromTerm(term));

            if (capability == null)
            {
                capability = DefaultCapability(odcmObject, term) as T;
            }

            return capability;
        }

        private static Func<OdcmCapability, bool> FuncFromTerm(string term)
        {
            var searchTerm = ToExternal(term);

            return (cap) => cap.TermName.EndsWith(searchTerm);
        }

        private static string ToExternal(string term)
        {
            if (NameMapper != null)
            {
                term = NameMapper.ToExternal(term);
            }

            return term;
        }

        public static string ToInternal(string term)
        {
            if (NameMapper != null)
            {
                term = NameMapper.ToInternal(term);
            }

            return term;
        }
    }
}
