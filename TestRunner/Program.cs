using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using CommandLine;

namespace TestRunner
{
    public class Program
    {
        protected Program( ) { }

        public static string mNunitPath { get; set; }
        public static string mDllPath { get; set; }
        public static string mBrowserType { get; set; }
        public static string mOutPutOption { get; set; }
        public static string mResultOption { get; set; }
        public static string mWorkersNumber { get; set; }

        private static void SetBrowserTypeInConfigTextFile( string aBrowserType )
        {
            string lCurrentBrowserType = aBrowserType == null ? "chrome" : aBrowserType.ToLowerInvariant( );

            if ( mDllPath != null )
            {
                int lIndexOfLastPathSeparatorChar = mDllPath.LastIndexOf( Path.DirectorySeparatorChar );

                if ( lIndexOfLastPathSeparatorChar > -1 )
                {
                    string lTestDllFolderPath = mDllPath.Substring( 0, lIndexOfLastPathSeparatorChar + 1 );

                    if ( lTestDllFolderPath != null )
                    {
                        string lConfigFilePath = Path.Combine( lTestDllFolderPath, "config.txt" );

                        File.WriteAllText( lConfigFilePath, lCurrentBrowserType );
                    }
                }
            }
        }

        static void Main( string[ ] args )
        {

            Options lOptions = new Options( );
            List<string> lAllArguments = new List<string>( );

            if ( Parser.Default.ParseArguments( args, lOptions ) )
            {
                mNunitPath      = lOptions.NunitPathArg;
                mDllPath        = lOptions.DllPathArg;
                mBrowserType    = lOptions.SelectedBrowserArg;
                mWorkersNumber  = lOptions.ParallelWorkersOptionArg;
                mResultOption   = lOptions.ResultOptionArg;
                mOutPutOption   = lOptions.OutputOptionArg;

                lAllArguments.Add( mNunitPath       );
                lAllArguments.Add( mDllPath         );
                lAllArguments.Add( mBrowserType     );
                lAllArguments.Add( mWorkersNumber   );
                lAllArguments.Add( mResultOption    );
                lAllArguments.Add( mOutPutOption    );
            }
            else if ( lOptions.HelpAsked )
            {
                // Display the default usage information
                Console.WriteLine( lOptions.GetUsage( ) );
            }

            IEnumerable<string> lArgumentsWithValues = lAllArguments.Where( x => x != null );

            int lArgumentsWithValuesCount = lArgumentsWithValues.Count( );

            if ( lArgumentsWithValuesCount == 2 )
            {
                string lArgumentsForNunit = " --noresult " + mDllPath;

                Process.Start( mNunitPath, lArgumentsForNunit );
            }
            else if ( lArgumentsWithValuesCount > 2 )
            {
                SetBrowserTypeInConfigTextFile( mBrowserType );

                StringBuilder lCommandBuilder = new StringBuilder( );

                if ( mResultOption == null && mOutPutOption == null )
                {
                    lCommandBuilder.Append( " --noresult" );
                }

                foreach( string lCurrentArg in lArgumentsWithValues )
                {
                    if ( lCurrentArg == mWorkersNumber )
                    {
                        lCommandBuilder.Append( $" --workers={lCurrentArg}" );
                    }
                    else if ( lCurrentArg == mOutPutOption )
                    {
                        lCommandBuilder.Append( $@" --out={lCurrentArg}" );
                    }
                    else if ( lCurrentArg == mResultOption )
                    {
                        lCommandBuilder.Append( $@" --result={lCurrentArg}" );
                    }
                }

                lCommandBuilder.Append( $@" {mDllPath}" );

                string lArgumentsForNunit = lCommandBuilder.ToString( );

                Process.Start( mNunitPath, lArgumentsForNunit );

            }
        }
    }
}
