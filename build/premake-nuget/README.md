Premake extension to support NuGet package dependencies in Visual Studio projects

### Features ###

* Support packages for C# projects
* Support native packages for C++ projects

### Usage ###

Simply add:

```lua
nuget { "package:version", "another-package:1.0" }
```

to your project definition and adjust accordingly

### Example ###

```lua
require "nuget"

solution "MySolution"
    configurations { "release", "debug" }

    project "FirstProject"
        kind "ConsoleApp"
        language "C++"
        files "main.cpp"
        nuget { "boost:1.59.0-b1", "sdl2.v140:2.0.3", "sdl2.v140.redist:2.0.3" }

    project "AnotherProject"
        kind "ConsoleApp"
        language "C#"
        files "main.cs"
        nuget "Newtonsoft.Json:7.0.1"
```
