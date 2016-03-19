-- blueprint-visualstudio

workspace("Blueprint.VisualStudio")
    configurations { "Debug", "Release" }
    location(_ACTION)

    configuration( "Debug" )
        targetdir( "../output/bin/Debug" )
        objdir( "../output/obj" )

    configuration( "Release" )
        targetdir( "../output/bin/Release" )
        objdir( "../output/obj" )

project("Blueprint.VisualStudio")
    kind("ConsoleApp")
    language("C#")

    files { "../source/**.cs" }

    links {
        "Microsoft.Build.dll",
        "Microsoft.Build.Engine.dll",
        "Microsoft.Build.Framework.dll",
        "System.Xml.dll"
    }
