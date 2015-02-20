// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class Attributes
    {
        public static IEnumerable<Attribute> ForConcrete(OdcmEntityClass odcmClass)
        {
            return new[]
            {
                Attribute.ForMicrosoftOdataClientKey(odcmClass)
            };
        }

        public static IEnumerable<Attribute> ForConcreteInterface 
        {
            get
            {
                return new [] {Attribute.ForLowerCaseProperty()};
            } 
        }

        public static IEnumerable<Attribute> ForFetcherInterface
        {
            get
            {
                return new[] { Attribute.ForLowerCaseProperty() };
            }
        }

        public static IEnumerable<Attribute> ForCollectionInterface
        {
            get
            {
                return new[] { Attribute.ForLowerCaseProperty() };
            }
        }
    }
}