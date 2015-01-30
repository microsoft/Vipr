// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Practices.Unity;
using System.Web.Http.Dependencies;

namespace ODataV4TestService.Extensions
{
    public static class DependencyResolverExtensions
    {
        public static T GetServiceOrDefault<T>(this IDependencyResolver dependencyResolver) where T : class
        {
            try
            {
                return dependencyResolver.GetService(typeof(T)) as T;
            }
            catch (ResolutionFailedException) { }

            return null;
        }
    }
}