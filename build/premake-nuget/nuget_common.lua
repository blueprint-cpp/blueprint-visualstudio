--
-- Name:        nuget/nuget_common.lua
-- Purpose:     Shared logic for C++ and C# projects.
-- Author:      Aleksi Juvani
-- Created:     2015/08/26
-- Copyright:   (c) 2008-2016 Jason Perkins and the Premake project
--

	local p = premake
	local m = p.modules.nuget

--
-- These functions take the package string as an argument and give you
-- information about it.
--

	function m.packagename(package)
		return package:gsub(":", ".")
	end

	function m.packageid(package)
		return package:sub(0, package:find(":") - 1)
	end

	function m.packageversion(package)
		return package:sub(package:find(":") + 1, -1)
	end

	local function packageProject(sln, package)
		for prj in p.workspace.eachproject(sln) do
			if prj.nuget then
				for i = 1, #prj.nuget do
					local projectPackage = prj.nuget[i]

					if projectPackage == package then
						return prj
					end
				end
			end
		end
	end

	function m.packageframework(sln, package)
		local prj = packageProject(sln, package)

		if prj.language == "C++" then
			return "native"
		elseif prj.language == "C#" then
			local cfg = p.project.getfirstconfig(prj)
			local action = premake.action.current()
			local framework = cfg.dotnetframework or action.vstudio.targetFramework
			framework = "net" .. framework:gsub("%.", "")
			return framework
		end
	end

--
-- Generates the packages.config file.
--

	function m.generatePackagesFile(sln, file)
		local packages = {}
		local packagecount = 0

		for prj in p.workspace.eachproject(sln) do
			if prj.nuget then
				for i = 1, #prj.nuget do
					local package = prj.nuget[i]

					if not packages[package] then
						packages[package] = true
						packagecount = packagecount + 1
					end
				end
			end
		end

		if packagecount == 0 then return end

		local function generate()
			p.w('<?xml version="1.0" encoding="utf-8"?>')
			p.push('<packages>')

			for package in pairs(packages) do
				p.x('<package id="%s" version="%s" targetFramework="%s" />', m.packageid(package), m.packageversion(package), m.packageframework(sln, package))
			end

			p.pop('</packages>')
		end

		if file then
			p.generate(
				{ location = path.join(sln.location, "packages.config") },
				nil,
				generate
			)
		else
			-- In the unit tests we aren't writing to a separate file.

			generate()
		end
	end

	function m.generateSolution(base, sln)
		base(sln)
		m.generatePackagesFile(sln, "packages.config")
	end

	p.override(p.vstudio.vs2005, "generateSolution", m.generateSolution)
