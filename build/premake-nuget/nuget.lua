--
-- Name:        nuget/nuget.lua
-- Purpose:     Adds support for NuGet package dependencies in Visual Studio projects.
-- Author:      Aleksi Juvani
-- Created:     2015/08/26
-- Copyright:   (c) 2008-2016 Jason Perkins and the Premake project
--

--
-- Always include _preload.lua so that the module works even when not embedded
--

	if not nuget then
		include "_preload.lua"
	end

--
-- Define the module
--

	local p = premake

	p.modules.nuget = {}

	local m = p.modules.nuget
	m._VERSION = p._VERSION

	include "nuget_common.lua"
	include "nuget_native.lua"
	include "nuget_csharp.lua"

	return m
