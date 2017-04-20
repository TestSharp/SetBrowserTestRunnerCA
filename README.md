TestRunner
======

This command line application is for help you running your NUnit based C# automation tests with custom parameters.

### Running your tests with TestRunner

You can run your tests by TestRunner with this synthax (**required**, *optional*):

>**[testrunner.exe_path]** **-n [nunit3-console.exe_path]** **-d [test_dll_file_path]** *-w [number_of_parallel_threads]*
>*-r [result_xml_file_path]* *-o [output_txt_file_path]* *-b [browsername]*


#### Additional informations

-w Only works if parallelization is set in your test project's AssemblyInfo.cs

Acceptable browser names: chrome, firefox, edge, ie, safari

You can use the parameter flags short versions ( -n, -d, -w, -o, -r, -b ),
or the long versions too ( --nunit, --dll, --output, --result, --workers, --browser ).

#### Reading in your set browser in your test setup

In order to be able to use the browser parameter in your test, you should add this code block to your C# test setup function.

```
string codeBase                 = Assembly.GetExecutingAssembly( ).CodeBase;
UriBuilder uri                  = new UriBuilder( codeBase );
string path                     = Uri.UnescapeDataString( uri.Path );
string assemblyDirectory        = Path.GetDirectoryName( path );
string configTextInAssemblyDir  = Path.Combine( assemblyDirectory, "config.txt" );

string selectedBrowser;

if ( !File.Exists( configTextInAssemblyDir ) )
{
    selectedBrowser = "chrome";
}
else
{
    string browserText = File.ReadAllText( configTextInAssemblyDir );
    browserText = browserText.Trim( ' ' );
    selectedBrowser = browserText.ToLowerInvariant( );
}

if ( selectedBrowser != null )
{
    switch ( selectedBrowser )
    {
        case "firefox":
            driver = new FirefoxDriver( );
            driver.Manage( ).Window.Maximize( );
            break;
        case "edge":
            driver = new EdgeDriver( );
            driver.Manage( ).Window.Maximize( );
            break;
        case "ie":
        case "internet explorer":
        case "internetexplorer":
        case "iexplorer":
            driver = new InternetExplorerDriver( );
            driver.Manage( ).Window.Maximize( );
            break;
        case "safari":
            driver = new SafariDriver( );
            driver.Manage( ).Window.Maximize( );
            break;
        case "chrome":
        default:
            driver = new ChromeDriver( );
            driver.Manage( ).Window.Maximize( );
            break;
    }
}
```


### How to add additional custom parameter options to the TestRunner Program

* Download the TestRunner program source code from here
* Open the Options.cs file in the project
* To the class level add a new property with the Option attribute

TestRunner using the CommandLineParser library by Giacomo Stelluti Scala.

Shoutout to him, it's a great library, you can read more about it here:

[CommandLineParser github wiki page](https://github.com/gsscoder/commandline/wiki)

Main synthax of the Option attribute:
>[Option( <short argument name, char>, <long argument name, string>, Required = <argument required or optional, bool>, HelpText = <string to show on the help screen related to this argument, string> )]
>
> Example:
>[Option( 'm', "myarg", Required = false, HelpText = "This is just an example argument" )]

And for the related property:

> Example:
>
> public string MyCustomArgument { get; set; }

You can use the primitive types for the property type: string, bool, int, char, double, decimal
