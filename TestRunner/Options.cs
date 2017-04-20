using System;
using System.Linq;
using CommandLine;
using CommandLine.Text;

namespace TestRunner
{
    public class Options
    {

        [Option( 'n', "nunit", Required = true, HelpText = "This is to set the nunit3 path and to run your tests" )]
        public string NunitPathArg { get; set; }
        [Option( 'd', "dll", Required = true, HelpText = "This is to specify, which dll we would like to run" )]
        public string DllPathArg { get; set; }
        [Option( 'o', "output", Required = false, HelpText = "You can set a filepath where you would like to save your text output file." )]
        public string OutputOptionArg { get; set; }
        [Option( 'b', "browser", Required = false, HelpText = "This will determine on which browser should the tests run. You can use: chrome, firefox, edge, safari, ie for the browser type parameter." )]
        public string SelectedBrowserArg { get; set; }
        [Option( 'r', "result", Required = false, HelpText = "You can set the file path for the NUnit generated xml and other reports from your tests. For example: --result= \"TestResult.xml;format=nunit2\"" )]
        public string ResultOptionArg { get; set; }
        [Option( 'w', "workers", Required = false, HelpText = "If parallelization is set in the AssemblyInfo file, you can set how many parallel threads you'd like to run your tests on." )]
        public string ParallelWorkersOptionArg { get; set; }
        [Option( 'h', "help", Required = false)]
        public bool HelpAsked { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            HelpText lHelp = new HelpText {
              AdditionalNewLineAfterOption = true,
              AddDashesToOption = true
            };

            lHelp.Heading = new HeadingInfo( "TestRunner", "v1.0" );

            lHelp.Copyright = new CopyrightInfo( "Daniel Erdos - T#", 2017 );

            lHelp.AddPreOptionsLine( "This command line application is for help you running your NUnit based C# automation tests with custom parameters." );

            lHelp.AddPreOptionsLine( Environment.NewLine );

            lHelp.AddPreOptionsLine( "The application adds the option to set browser type based on command line argument, and writes out to a file, which can later read by your test setup." );

            lHelp.AddPreOptionsLine( "For see additional help information, please run: TestRunner.exe --help" );

            lHelp.AddPreOptionsLine( "Minimum usage is: Testrunner.exe -n/--nunit [path_to_nunit3-console.exe] -d/--dll [path_to_your_test_dll]" );

            lHelp.AddPreOptionsLine( "You can also set text output (-o/--output), xml results (-r/--result), workers number (-w/--workers), browser (-b/--browser), but these are optional parameters" );

            lHelp.AddOptions( this );

            if ( LastParserState?.Errors.Any( ) == true )
            {
              string lErrors = lHelp.RenderParsingErrorsText(this, 2); // indent with two spaces

              if ( !string.IsNullOrEmpty( lErrors ) )
              {
                lHelp.AddPreOptionsLine( string.Concat( Environment.NewLine, "ERROR(S):") );

                lHelp.AddPreOptionsLine( lErrors );
              }
            }

            return lHelp;
        }
    }
}
