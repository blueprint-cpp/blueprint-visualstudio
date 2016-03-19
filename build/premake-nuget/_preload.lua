--
-- Name:        nuget/_preload.lua
-- Purpose:     Define the NuGet APIs.
-- Author:      Aleksi Juvani
-- Created:     2015/08/26
-- Copyright:   (c) 2008-2016 Jason Perkins and the Premake project
--

	local p = premake
	local m = p.modules.nuget

--
-- Register the NuGet extension
--

	p.api.register {
		name = "nuget",
		scope = "project",
		kind = "list:string",
		tokens = true
	}

--
-- Decide when to load the full module
--

	return function (cfg)
		return cfg.language == "C++" or cfg.language == "C#"
	end
