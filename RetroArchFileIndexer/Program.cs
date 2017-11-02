using System;
using System.IO;
using System.Reflection;

namespace RetroArchFileIndexer
{
    static class Program
    {
        public static readonly AssemblyName AssemblyName = Assembly.GetExecutingAssembly().GetName();

        public static readonly string AppDataPath = Environment.ExpandEnvironmentVariables( "%AppData%" );

        public static readonly string RetroArchPath = Path.Combine( AppDataPath, "RetroArch" );

        public static readonly string RetroArchPlaylistPath = Path.Combine( RetroArchPath, "playlists" );

        public static readonly string RetroArchPlaylistExtension = "lpl";

        public static readonly string RetroArchPlaylistDetectOption = "DETECT";

        public static void DisplayUsage()
        {
            Console.WriteLine($"{AssemblyName.Name} v{AssemblyName.Version.Major}.{AssemblyName.Version.Minor}.{AssemblyName.Version.Revision} by TGE (2017)" );
            Console.WriteLine( "This application can be used to generate RetroArch playlist files from directories" );
            Console.WriteLine( "Usage:" );
            Console.WriteLine($"    {AssemblyName.Name} [optional rom parent directory path]" );
            Console.WriteLine( "    If an optional rom parent path is not provided, then the path specified in the app config will be used" );
            Console.WriteLine( "    You can display this help message by providing a '-h' argument" );
            Console.WriteLine();
            Console.WriteLine( "Configuring a rom parent directory:");
            Console.WriteLine( "    To set your rom parent directory, change the value of the RomParentDirectory element's Path attribute" );
            Console.WriteLine( "    in the IndexerConfig element in the app config." );
            Console.WriteLine();
            Console.WriteLine( "Press any key to exit" );
            Console.ReadKey();
        }

        public static void Main( string[] args )
        {
            string romParentDirectoryPath;
            if ( args.Length == 0 )
            {
                romParentDirectoryPath = IndexerConfig.Instance.RomParentDirectoryConfig.Path;
            }
            else
            {
                if ( args[0] == "-h" )
                {
                    DisplayUsage();
                    return;
                }
                else
                {
                    romParentDirectoryPath = args[0];
                }
            }

            Console.WriteLine( $"Rom parent directory path = {romParentDirectoryPath}" );

            foreach ( var directoryPath in Directory.EnumerateDirectories( romParentDirectoryPath ) )
            {
                var directoryName = Path.GetFileName( directoryPath );
                var playlistName = Path.ChangeExtension( directoryName, RetroArchPlaylistExtension );
                var playListPath = Path.Combine( RetroArchPlaylistPath, playlistName );

                Console.WriteLine( $"Creating playlist: {playlistName}" );

                using ( var writer = File.CreateText( playListPath ) )
                {
                    foreach ( var filePath in Directory.EnumerateFiles( directoryPath ) )
                    {
                        var fileFullPath = Path.GetFullPath( filePath );
                        var fileName = Path.GetFileNameWithoutExtension( fileFullPath );

                        Console.WriteLine( $"Writing entry for {fileName}" );

                        // Write playlist entry
                        writer.WriteLine( fileFullPath );                       // file path
                        writer.WriteLine( fileName );                           // rom display name
                        writer.WriteLine( RetroArchPlaylistDetectOption );      // core file path
                        writer.WriteLine( RetroArchPlaylistDetectOption );      // core display name
                        writer.WriteLine( RetroArchPlaylistDetectOption );      // crc
                        writer.WriteLine( playlistName );                       // name of playlist
                    }
                }
            }

            Console.WriteLine( "Done!" );
        }
    }
}
