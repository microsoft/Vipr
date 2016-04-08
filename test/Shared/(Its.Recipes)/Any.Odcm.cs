// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vipr.Core;
using Vipr.Core.CodeModel;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;

namespace Microsoft.Its.Recipes
{
    internal static partial class Any
    {
        /// <summary>
        ///     Generates a string.
        /// </summary>
        /// <param name="minLength">The minimum desired length.</param>
        /// <param name="maxLength">The maximum desired length.</param>
        public static string CSharpIdentifier(int minLength = 5, int? maxLength = 10)
        {
            return String(1, 1, Characters.LatinLettersAndUnderscore())
                + String(minLength - 1, maxLength, Characters.LatinLettersAndNumbersAndUnderscore());
        }

        public static OdcmEnumMember OdcmEnumMember(Action<OdcmEnumMember> config = null)
        {
            var retVal = new OdcmEnumMember(Any.CSharpIdentifier());

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmNamespace EmptyOdcmNamespace(Action<OdcmNamespace> config = null)
        {
            var retVal = new OdcmNamespace(Any.CSharpIdentifier());

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmNamespace OdcmNamespace(Action<OdcmNamespace> config = null)
        {
            var retVal = new OdcmNamespace(Any.CSharpIdentifier());

            retVal.Types.AddRange(Any.Sequence(s => Any.OdcmEnum()));

            retVal.Types.AddRange(Any.Sequence(s => Any.ComplexOdcmClass(retVal)));

            var classes = Any.Sequence(s => Any.OdcmEntityClass(retVal))
                .Concat(Any.Sequence(s => Any.MediaOdcmClass(retVal), count: 2)).ToArray();

            foreach (var @class in (from @class in retVal.Types where @class is OdcmEntityClass select @class as OdcmEntityClass))
            {
                @class.Properties.AddRange(Any.Sequence(i => Any.OdcmProperty(p =>
                {
                    p.Class = @class;
                    p.Projection = classes.RandomElement().DefaultProjection;
                })));
            }
            foreach (var @class in classes)
            {
                @class.Properties.AddRange(Any.Sequence(i => Any.OdcmProperty(p =>
                {
                    p.Class = @class;
                    p.Projection = classes.RandomElement().DefaultProjection;
                })));
            }

            classes[0].Base = classes[1];

            if (!classes[1].Derived.Contains(classes[0]))
            {
                classes[1].Derived.Add(classes[0]);
            }

            retVal.Types.AddRange(classes);

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmEnum OdcmEnum(Action<OdcmEnum> config = null)
        {
            var retVal = new OdcmEnum(Any.CSharpIdentifier(), Any.EmptyOdcmNamespace());
            retVal.UnderlyingType = Any.EnumUnderlyingType();

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmComplexClass OdcmComplexClass(Action<OdcmClass> config = null)
        {
            var retVal = new OdcmComplexClass(Any.CSharpIdentifier(), Any.OdcmNamespace());

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmProperty OdcmProperty(Action<OdcmProperty> config = null)
        {
            var retVal = new OdcmProperty(Any.CSharpIdentifier())
            {
                Projection = new OdcmProjection()
            };

            if (config != null) config(retVal);

            return retVal;
        }
#if false
        public static OdcmProjection OdcmProjection(OdcmType odcmType, Action<OdcmProjection> config = null)
        {
            var projection = new OdcmProjection
            {
                Type = odcmType,
                Capabilities = new List<OdcmCapability>()
            };

            var capabilityTerms = Vipr.Core.CodeModel.OdcmProjection
                    .BooleanCapabilityTerms.RandomSequence()
                    .Distinct();

            foreach (var term in capabilityTerms)
            {
                projection.Capabilities.Add(new OdcmBooleanCapability(Any.Bool(), term));
            }

            if (config != null) config(projection);

            return projection;
        }
        public static IEnumerable<OdcmProjection> OdcmProjections(OdcmType odcmType, int count = 5, Action<OdcmProjection> config = null)
        {
            List<OdcmProjection> projections = new List<OdcmProjection>();

            for (int i = 0; i < count; i++)
            {
                projections.Add(Any.OdcmProjection(odcmType, config));
            }

            return projections;
        }
#endif

        private static OdcmType PrimitiveOdcmType(Action<OdcmType> config = null)
        {
            var retVal = new OdcmPrimitiveType("String", Vipr.Core.CodeModel.OdcmNamespace.Edm);

            if (config != null) config(retVal);

            return retVal;
        }

        private static OdcmPrimitiveType EnumUnderlyingType(Action<OdcmPrimitiveType> config = null)
        {
            List<string> underlyingTypes = new List<string>() { "Byte", "SByte", "Int16", "Int32", "Int64" };
            var retVal = new OdcmPrimitiveType(underlyingTypes.RandomElement(), Vipr.Core.CodeModel.OdcmNamespace.Edm);

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmComplexClass ComplexOdcmClass(OdcmNamespace odcmNamespace, Action<OdcmClass> config = null)
        {
            var retVal = new OdcmComplexClass(Any.CSharpIdentifier(), odcmNamespace);

            retVal.Properties.AddRange(Any.Sequence(i => Any.PrimitiveOdcmProperty(p => p.Class = retVal)));

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmProperty PrimitiveOdcmProperty(Action<OdcmProperty> config = null)
        {
            var retVal = new OdcmProperty(Any.CSharpIdentifier())
            {
                Projection = Any.PrimitiveOdcmType().DefaultProjection
            };

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmProperty EntityOdcmProperty(OdcmNamespace odcmNamespace, Action<OdcmProperty> config = null)
        {
            return OdcmEntityProperty(Any.OdcmEntityClass(odcmNamespace), config);
        }

        private static OdcmProperty OdcmEntityProperty(OdcmClass @class, Action<OdcmProperty> config)
        {
            var projection = @class.DefaultProjection;
            var retVal = new OdcmProperty(Any.CSharpIdentifier()) { Projection = projection };

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmProperty ComplexOdcmProperty(OdcmNamespace odcmNamespace, Action<OdcmProperty> config = null)
        {
            var projection = Any.ComplexOdcmClass(odcmNamespace).DefaultProjection;
            var retVal = new OdcmProperty(Any.CSharpIdentifier()) { Projection = projection };


            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmMediaClass MediaOdcmClass(OdcmNamespace odcmNamespace, Action<OdcmEntityClass> config = null)
        {
            var retVal = new OdcmMediaClass(Any.CSharpIdentifier(), odcmNamespace);

            EntityOrMediaOdcmClass(odcmNamespace, config, retVal);

            return retVal;
        }

        public static OdcmEntityClass OdcmEntityClass(OdcmNamespace odcmNamespace, Action<OdcmEntityClass> config = null)
        {
            return Any.OdcmEntityClass(odcmNamespace, Any.CSharpIdentifier(), config);
        }

        public static OdcmEntityClass OdcmEntityClass(OdcmNamespace odcmNamespace, string name, Action<OdcmEntityClass> config = null)
        {
            var retVal = new OdcmEntityClass(name, odcmNamespace);

            EntityOrMediaOdcmClass(odcmNamespace, config, retVal);

            return retVal;
        }

        private static void EntityOrMediaOdcmClass(OdcmNamespace odcmNamespace, Action<OdcmEntityClass> config, OdcmEntityClass retVal)
        {
            retVal.Properties.AddRange(Any.Sequence(i => Any.PrimitiveOdcmProperty(p => p.Class = retVal)));

            retVal.Key.AddRange(retVal.Properties.RandomSubset(2));

            if (odcmNamespace.Classes.Any(c => c.Kind == OdcmClassKind.Complex))
                retVal.Properties.AddRange(Any.Sequence(i => Any.OdcmProperty(p =>
                {
                    p.Class = retVal;
                    p.Projection = odcmNamespace.Classes.Where(c => c.Kind == OdcmClassKind.Complex).RandomElement().DefaultProjection;
                })));

            retVal.Properties.AddRange(Any.Sequence(i => Any.OdcmEntityProperty(retVal, p => { p.Class = retVal; })));

            retVal.Properties.AddRange(Any.Sequence(i => Any.OdcmEntityProperty(retVal, p =>
            {
                p.Class = retVal;
                p.IsCollection = true;
            })));

            if (config != null) config(retVal);

            retVal.Methods.AddRange(Any.Sequence(s => Any.OdcmMethod()));
        }

        public static OdcmServiceClass ServiceOdcmClass(OdcmNamespace odcmNamespace, Action<OdcmServiceClass> config = null)
        {
            var retVal = new OdcmServiceClass(Any.CSharpIdentifier(), odcmNamespace);

            var entities = odcmNamespace.Classes
                .Where(c => c.Kind == OdcmClassKind.Entity);

            foreach (var entity in entities)
            {
                var projection = entity.DefaultProjection;

                retVal.Properties.Add(new OdcmProperty(entity.Name) { Class = retVal, Projection = projection });

                retVal.Properties.Add(new OdcmProperty(entity.Name + "s")
                {
                    Class = retVal,
                    Projection = projection,
                    IsCollection = true
                });
            }

            retVal.Methods.AddRange(Any.Sequence(s => Any.OdcmMethod()));

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmMethod OdcmMethod(Action<OdcmMethod> config = null)
        {
            var retVal = new OdcmMethod(Any.CSharpIdentifier(), Any.EmptyOdcmNamespace());

            retVal.Verbs = EnumValue<OdcmAllowedVerbs>();

            retVal.Parameters.AddRange(
                Any.Sequence(s => new OdcmParameter(Any.CSharpIdentifier()) { Type = Any.PrimitiveOdcmType() }, Any.Int(0, 3)));

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmMethod OdcmMethodPost(Action<OdcmMethod> config = null)
        {
            var retVal = Any.OdcmMethodGet();

            retVal.Verbs = OdcmAllowedVerbs.Post;

            retVal.Parameters.AddRange(
                Any.Sequence(
                    s =>
                        new OdcmParameter(Any.CSharpIdentifier())
                        {
                            Type = Any.PrimitiveOdcmType(),
                            CallingConvention = OdcmCallingConvention.InHttpMessageBody
                        }, Any.Int(1, 3)));

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmMethod OdcmMethodGet(Action<OdcmMethod> config = null)
        {
            var retVal = new OdcmMethod(Any.CSharpIdentifier(), Any.EmptyOdcmNamespace());

            retVal.Verbs = OdcmAllowedVerbs.Get;

            retVal.Parameters.AddRange(
                Any.Sequence(
                    s =>
                        new OdcmParameter(Any.CSharpIdentifier())
                        {
                            Type = Any.PrimitiveOdcmType(),
                            CallingConvention = OdcmCallingConvention.InHttpRequestUri
                        }, Any.Int(1, 3)));

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmParameter OdcmParameter(Action<OdcmParameter> config = null)
        {
            var retVal = new OdcmParameter(Any.CSharpIdentifier());

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmModel OdcmModel(Action<OdcmModel> config = null)
        {
            var retVal = new OdcmModel(Any.ServiceMetadata());

            retVal.Namespaces.AddRange(Any.Sequence(s => Any.OdcmNamespace()));

            var containerNamespace = retVal.Namespaces.RandomElement();

            containerNamespace.Types.Add(Any.ServiceOdcmClass(containerNamespace));

            if (config != null) config(retVal);

            return retVal;
        }

        public static IEnumerable<TextFile> ServiceMetadata()
        {
            yield return new TextFile("$metadata", TestConstants.ODataV4.EmptyEdmx);
        }

        public static Func<Task<String>> TokenGetterFunction(string token = "")
        {
            return () => Task.FromResult(token);
        }
    }
}
