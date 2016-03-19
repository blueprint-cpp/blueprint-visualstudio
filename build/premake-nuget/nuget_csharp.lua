--
-- Name:        nuget/nuget_csharp.lua
-- Purpose:     Adds support for NuGet package dependencies in C# projects.
-- Author:      Aleksi Juvani
-- Created:     2015/08/26
-- Copyright:   (c) 2008-2016 Jason Perkins and the Premake project
--

	local p = premake
	local m = p.modules.nuget

	local function packageAssemblyPath(prj, package, framework)
		return p.vstudio.path(prj, p.filename(prj.solution, string.format("packages\\%s\\lib\\%s\\%s.dll", m.packagename(package), framework, m.packageid(package))))
	end

--
-- Adds the packages to the assembly references.
--

	local function assemblyReferences(base, prj)
		-- Visual Studio actually puts these in the same ItemGroup as the rest
		-- of the assembly references, but we don't want to repeat the code
		-- from the original function here. It still works if we create a new
		-- group so we do just that.

		if prj.nuget and #prj.nuget > 0 then
			p.push('<ItemGroup>')
				for i = 1, #prj.nuget do
					local package = prj.nuget[i]
					p.push('<Reference Include="%s">', m.packageid(package))

					local targetFramework = m.packageframework(prj.solution, package)

					-- Strip off the "net" prefix so we can compare it.

					local targetFrameworkVersion = tonumber(targetFramework:sub(4))

					-- If the package doesn't support the target framework, we
					-- need to check if it exists in the folders for any of
					-- the previous framework versions. The last HintPath will
					-- override any previous HintPaths (if the condition is
					-- met that is).

					local frameworks = {}
					if targetFrameworkVersion >= 11 then table.insert(frameworks, "net10") end
					if targetFrameworkVersion >= 20 then table.insert(frameworks, "net11") end
					if targetFrameworkVersion >= 30 then table.insert(frameworks, "net20") end
					if targetFrameworkVersion >= 35 then table.insert(frameworks, "net30") end
					if targetFrameworkVersion >= 40 then table.insert(frameworks, "net35") end
					if targetFrameworkVersion >= 45 then table.insert(frameworks, "net40") end
					table.insert(frameworks, targetFramework)

					for _, framework in pairs(frameworks) do
						local assemblyPath = packageAssemblyPath(prj, package, framework)
						p.x('<HintPath Condition="Exists(\'%s\')">%s</HintPath>', assemblyPath, assemblyPath)
					end

					p.x('<Private>True</Private>')
					p.pop('</Reference>')
				end
			p.pop('</ItemGroup>')
		end

		base(prj)
	end

	p.override(p.vstudio.cs2005, "assemblyReferences", assemblyReferences)
