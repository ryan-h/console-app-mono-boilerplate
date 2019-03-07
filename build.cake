//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");

var configuration = EnvironmentVariable("configuration") ?? "Release";
var runtime = EnvironmentVariable("runtime");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var buildOutputDirectory = Directory("./dist");

var solutionFile = File("./ConsoleAppMonoBoilerplate.sln");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

var tasks = new
{
    Clean = "Clean",
    Publish = "Publish"
};

Task(tasks.Clean)
    .Description("Cleans the build directories.")
    .Does(() =>
    {
        Information("Cleaning the build directories...");

        // clean each project bin directory
        var binDirectories = GetDirectories("./src/**/bin");

        foreach (var binDir in binDirectories)
        {
            Information($"Cleaning {binDir}...");

            CleanDirectory(binDir);
        }

        Information($"Cleaning {MakeAbsolute(buildOutputDirectory)}...");

         // clean the main build output directory
        CleanDirectory(buildOutputDirectory);
    });

Task(tasks.Publish)
    .Description("Publishes the solution.")
    .IsDependentOn(tasks.Clean)
    .Does(() =>
    {
        var publishSettings = new DotNetCorePublishSettings
        {
            Verbosity = DotNetCoreVerbosity.Normal,
            Configuration = configuration,
            OutputDirectory = buildOutputDirectory
        };

        if (runtime == null)
        {
            Information("Publishing the solution for a framework-dependent deployment...");

            DotNetCorePublish(solutionFile, publishSettings);
        }
        else
        {
            // expects a single or semi-colon separated list of runtime identifiers matching those defined in the csproj file,
            // these are passed as a build variable (ie "win-x64;osx-x64;linux-x64"),
            // see https://docs.microsoft.com/en-us/dotnet/core/rid-catalog for the possible values
            var runtimes = runtime.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (var runtime in runtimes)
            {
                Information($"Publishing the solution for {runtime}...");

                publishSettings.Runtime = runtime;
                publishSettings.OutputDirectory = buildOutputDirectory + Directory(runtime);

                // publish the cli for the specified runtime
                DotNetCorePublish(solutionFile, publishSettings);
            }
        }
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .Description("Default cake build task.")
    .IsDependentOn(tasks.Publish);

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
