--
-- Name:        nuget/nuget_native.lua
-- Purpose:     Adds support for NuGet package dependencies in C++ projects.
-- Author:      Aleksi Juvani
-- Created:     2015/08/26
-- Copyright:   (c) 2008-2016 Jason Perkins and the Premake project
--

	local p = premake
	local m = p.modules.nuget

	local function packageTargetsFile(prj, package)
		return p.vstudio.path(prj, p.filename(prj.solution, string.format("packages\\%s\\build\\native\\%s.targets", m.packagename(package), m.packageid(package))))
	end

--
-- Adds the packages to the imports section of the project file.
--

	function m.importNuGetTarget(prj)
		if prj.nuget then
			for i = 1, #prj.nuget do
				local targetsFile = packageTargetsFile(prj, prj.nuget[i])
				p.x('<Import Project="%s" Condition="Exists(\'%s\')" />', targetsFile, targetsFile)
			end
		end
	end

	p.override(p.vstudio.vc2010.elements, "importExtensionTargets", function(base, prj)
		local calls = base(prj)
		table.insertafter(calls, p.vstudio.vc2010.importRuleTargets, m.importNuGetTarget)
		return calls
	end)

--
-- Adds the pre-build check that makes sure that all packages are installed.
--

	function m.ensureNuGetPackageBuildImports(prj)
		p.push('<Target Name="EnsureNuGetPackageBuildImports" BeforeTargets="PrepareForBuild">')
		p.push('<PropertyGroup>')
		p.x('<ErrorText>This project references NuGet package(s) that are missing on this computer. Use NuGet Package Restore to download them.  For more information, see http://go.microsoft.com/fwlink/?LinkID=322105. The missing file is {0}.</ErrorText>')
		p.pop('</PropertyGroup>')

		if prj.nuget then
			for i = 1, #prj.nuget do
				local targetsFile = packageTargetsFile(prj, prj.nuget[i])
				p.x('<Error Condition="!Exists(\'%s\')" Text="$([System.String]::Format(\'$(ErrorText)\', \'%s\'))" />', targetsFile, targetsFile)
			end
		end
		p.pop('</Target>')
	end

	p.override(p.vstudio.vc2010.elements, "project", function(base, prj)
		local calls = base(prj)
		table.insertafter(calls, p.vstudio.vc2010.importExtensionTargets, m.ensureNuGetPackageBuildImports)
		return calls
	end)
