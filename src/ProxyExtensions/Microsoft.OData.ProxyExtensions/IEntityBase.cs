// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.OData.Client;

namespace Microsoft.OData.ProxyExtensions
{
    public interface IEntityBase
    {
        /// <param name="deferSaveChanges">true to delay saving until batch is saved; false to save immediately.</param>
        Task UpdateAsync(bool deferSaveChanges = false);
        /// <param name="deferSaveChanges">true to delay saving until batch is saved; false to save immediately.</param>
        Task DeleteAsync(bool deferSaveChanges = false);

        /// <param name="deferSaveChanges">true to delay saving until batch is saved; false to save immediately.</param>
        /// <param name="saveChangesOption">Save changes option to control how change requests are sent to the service.</param>
        Task SaveChangesAsync(bool deferSaveChanges = false, SaveChangesOptions saveChangesOption = SaveChangesOptions.None);
    }
}
