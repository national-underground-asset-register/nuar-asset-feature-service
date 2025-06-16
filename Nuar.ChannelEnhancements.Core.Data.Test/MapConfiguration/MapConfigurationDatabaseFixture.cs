using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuar.ChannelEnhancements.Core.Data.Test.MapConfiguration
{
    /// <summary>
    /// 
    /// </summary>
    [CollectionDefinition(nameof(MapConfigurationDatabaseFixture))]
    public class MapConfigurationDatabaseFixture : ICollectionFixture<Helpers.Fixture.DatabaseFixture>;
}
