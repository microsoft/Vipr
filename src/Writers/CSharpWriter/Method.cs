// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public abstract class Method : MethodSignature
    {
        public string ModelName { get; protected set; }

        public static Method ForEntityType(OdcmMethod odcmMethod)
        {
            return odcmMethod.ReturnType == null
                ? (Method)new EntityVoidMethod(odcmMethod)
                : (Method)new EntityFunctionMethod(odcmMethod);
        }
    }

    class ServerMethod : Method
    {
        protected ServerMethod(OdcmMethod odcmMethod)
        {

            BodyParameters = odcmMethod.Parameters
                .Where(p => p.CallingConvention == OdcmCallingConvention.InHttpMessageBody)
                .Select(Parameter.FromOdcmParameter);

            UriParameters = odcmMethod.Parameters
                .Where(p => p.CallingConvention == OdcmCallingConvention.InHttpRequestUri)
                .Select(Parameter.FromOdcmParameter);

            switch (odcmMethod.Verbs)
            {
                case OdcmAllowedVerbs.Any:
                    HttpMethod = "GET";
                    break;
                case OdcmAllowedVerbs.Delete:
                    HttpMethod = "DELETE";
                    break;
                case OdcmAllowedVerbs.Get:
                    HttpMethod = "GET";
                    break;
                case OdcmAllowedVerbs.Patch:
                    HttpMethod = "PATCH";
                    break;
                case OdcmAllowedVerbs.Post:
                    HttpMethod = "POST";
                    break;
                case OdcmAllowedVerbs.Put:
                    HttpMethod = "PUT";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            IsAsync = true;
            ModelName = odcmMethod.Name;
            Name = odcmMethod.Name + "Async";
            Parameters = odcmMethod.Parameters.Select(Parameter.FromOdcmParameter);
        }

        public IEnumerable<Parameter> BodyParameters { get; private set; }
        public IEnumerable<Parameter> UriParameters { get; private set; }
        public string HttpMethod { get; private set; }
    }
}
