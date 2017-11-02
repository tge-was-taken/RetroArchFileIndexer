using System.Configuration;

namespace RetroArchFileIndexer
{
    public class IndexerConfig : ConfigurationSection
    {
        public static IndexerConfig Instance => 
            ( IndexerConfig )ConfigurationManager.GetSection( "IndexerConfig" );

        [ConfigurationProperty( "RomParentDirectory", IsRequired = true )]
        public RomParentDirectoryConfigElement RomParentDirectoryConfig
        {
            get => ( RomParentDirectoryConfigElement )this["RomParentDirectory"];
        }
    }
}
