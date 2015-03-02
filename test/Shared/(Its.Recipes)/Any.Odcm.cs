// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vipr.Core;
using Vipr.Core.CodeModel;

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

            var classes = Any.Sequence(s => Any.EntityOdcmClass(retVal)).ToArray();

            foreach (var @class in classes)
            {
                @class.Properties.AddRange(Any.Sequence(i => Any.OdcmProperty(p =>
                {
                    p.Class = @class;
                    p.Type = classes.RandomElement();
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
            var retVal = new OdcmEnum(Any.CSharpIdentifier(), Any.CSharpIdentifier());
            retVal.UnderlyingType = Any.EnumUnderlyingType();

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmClass OdcmClass(Action<OdcmClass> config = null)
        {
            var retVal = new OdcmClass(Any.CSharpIdentifier(), Any.CSharpIdentifier());

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmProperty OdcmProperty(Action<OdcmProperty> config = null)
        {
            var retVal = new OdcmProperty(Any.CSharpIdentifier());

            if (config != null) config(retVal);

            return retVal;
        }

        private static OdcmType PrimitiveOdcmType(Action<OdcmType> config = null)
        {
            var retVal = new OdcmPrimitiveType("String", "Edm");

            if (config != null) config(retVal);

            return retVal;
        }

        private static OdcmPrimitiveType EnumUnderlyingType(Action<OdcmPrimitiveType> config = null)
        {
            List<string> underlyingTypes = new List<string>() { "Byte", "SByte", "Int16", "Int32", "Int64" };
            var retVal = new OdcmPrimitiveType(underlyingTypes.RandomElement(), "Edm");

            if (config != null) config(retVal);

            return retVal;
        }


        public static OdcmClass ComplexOdcmClass(OdcmNamespace odcmNamespace, Action<OdcmClass> config = null)
        {
            var retVal = new OdcmClass(Any.CSharpIdentifier(), odcmNamespace.Name);

            retVal.Properties.AddRange(Any.Sequence(i => Any.PrimitiveOdcmProperty(p => p.Class = retVal)));

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmProperty PrimitiveOdcmProperty(Action<OdcmProperty> config = null)
        {
            var retVal = new OdcmProperty(Any.CSharpIdentifier()) { Type = Any.PrimitiveOdcmType() };

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmProperty EntityOdcmProperty(OdcmNamespace odcmNamespace, Action<OdcmProperty> config = null)
        {
            return OdcmEntityProperty(Any.EntityOdcmClass(odcmNamespace), config);
        }

        private static OdcmProperty OdcmEntityProperty(OdcmClass @class, Action<OdcmProperty> config)
        {
            var retVal = new OdcmProperty(Any.CSharpIdentifier()) { Type = @class };

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmProperty ComplexOdcmProperty(OdcmNamespace odcmNamespace, Action<OdcmProperty> config = null)
        {
            var retVal = new OdcmProperty(Any.CSharpIdentifier()) { Type = Any.ComplexOdcmClass(odcmNamespace) };


            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmEntityClass EntityOdcmClass(OdcmNamespace odcmNamespace, Action<OdcmEntityClass> config = null)
        {
            var retVal = new OdcmEntityClass(Any.CSharpIdentifier(), odcmNamespace.Name);

            retVal.Properties.AddRange(Any.Sequence(i => Any.PrimitiveOdcmProperty(p => p.Class = retVal)));

            retVal.Key.AddRange(retVal.Properties.RandomSubset(2));

            if (odcmNamespace.Classes.Any(c => c.Kind == OdcmClassKind.Complex))
                retVal.Properties.AddRange(Any.Sequence(i => Any.OdcmProperty(p =>
                {
                    p.Class = retVal;
                    p.Type = odcmNamespace.Classes.Where(c => c.Kind == OdcmClassKind.Complex).RandomElement();
                })));

            retVal.Properties.AddRange(Any.Sequence(i => Any.OdcmEntityProperty(retVal, p => { p.Class = retVal; })));

            retVal.Properties.AddRange(Any.Sequence(i => Any.OdcmEntityProperty(retVal, p => 
            { 
                p.Class = retVal;
                p.IsCollection = true;
            })));
            
            if (config != null) config(retVal);

            retVal.Methods.AddRange(Any.Sequence(s => Any.OdcmMethod()));

            return retVal;
        }

        public static OdcmServiceClass ServiceOdcmClass(OdcmNamespace odcmNamespace, Action<OdcmServiceClass> config = null)
        {
            var retVal = new OdcmServiceClass(Any.CSharpIdentifier(), odcmNamespace.Name);

            var entities = odcmNamespace.Classes
                .Where(c => c.Kind == OdcmClassKind.Entity);

            foreach (var entity in entities)
            {
                retVal.Properties.Add(new OdcmProperty(entity.Name) { Class = retVal, Type = entity });

                retVal.Properties.Add(new OdcmProperty(entity.Name + "s")
                {
                    Class = retVal,
                    Type = entity,
                    IsCollection = true
                });
            }

            retVal.Methods.AddRange(Any.Sequence(s => Any.OdcmMethod()));

            if (config != null) config(retVal);

            return retVal;
        }

        public static OdcmMethod OdcmMethod(Action<OdcmMethod> config = null)
        {
            var retVal = new OdcmMethod(Any.CSharpIdentifier());

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
            var retVal = new OdcmMethod(Any.CSharpIdentifier());

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

        public static TextFileCollection ServiceMetadata()
        {
            return new TextFileCollection
            {
                new TextFile("$metadata", "<?xml version=\"1.0\" encoding=\"utf-8\"?><edmx:Edmx Version=\"4.0\" xmlns:edmx=\"http://docs.oasis-open.org/odata/ns/edmx\"></edmx:Edmx>"),
            };
        }

        public static Func<Task<String>> TokenGetterFunction(string token = "")
        {
            return () => Task.FromResult(token);
        }
    }
}
