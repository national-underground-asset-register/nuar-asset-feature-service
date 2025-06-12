// <copyright file="IStyleRuleRepository.cs" company="AtkinsRéalis">
// Copyright (c) AtkinsRéalis. All rights reserved.
// </copyright>

using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Styles;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces
{
    /// <summary>
    /// IStyleRuleRepository interface.
    /// </summary>
    public interface IStyleRuleRepository
    {
        /// <summary>
        /// Gets all style rules
        /// </summary>
        public Task<IEnumerable<StyleRule>?> GetAll();

        /// <summary>
        /// Gets the specific set of style rules for a given layer
        /// </summary>
        /// <param name="layerId">The layer identifier</param>
        /// <returns> The set of style rules or null if none exist </returns>
        public Task<IEnumerable<StyleRule>?> GetAllByLayerId(Guid layerId);

        /// <summary>
        /// Gets a <see cref="StyleRule"/> matching the provided unique identifier
        /// </summary>
        /// <param name="styleRuleId">The unique identifier</param>
        /// <returns>The <see cref="StyleRule"/> matching the provided identifier</returns>
        public Task<StyleRule?>? GetRuleById(Guid styleRuleId);
    }
}