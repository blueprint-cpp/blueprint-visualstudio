-- blueprint-visualstudio

workspace("Blueprint.VisualStudio")
    configurations { "Debug", "Release" }
    location(_ACTION)

project("Blueprint.VisualStudio")
    kind("ConsoleApp")
    language("C#")

    files { "../source/**.cs" }
