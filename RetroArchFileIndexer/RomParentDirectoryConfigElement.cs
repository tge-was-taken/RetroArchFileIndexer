using System.Configuration;

namespace RetroArchFileIndexer
{
    public class RomParentDirectoryConfigElement : ConfigurationElement
    {
        [ConfigurationProperty( "Path", IsRequired = true )]
        public string Path => ( string )this["Path"];
    }
}
