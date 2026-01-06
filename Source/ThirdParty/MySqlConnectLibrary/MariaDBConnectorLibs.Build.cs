// Copyright 2026 God Mode Games, LLC. All Rights Reserved.

using System.IO;
using UnrealBuildTool;

public class MariaDBConnectorLibs : ModuleRules
{
	public MariaDBConnectorLibs(ReadOnlyTargetRules Target) : base(Target)
	{
		Type = ModuleType.External;

		if (Target.Platform == UnrealTargetPlatform.Win64)
		{
			string PlatformPath = Path.Combine(ModuleDirectory, "Win64");
			PublicIncludePaths.Add(Path.Combine(PlatformPath, "include"));
			PublicAdditionalLibraries.Add(Path.Combine(PlatformPath, "lib", "mariadbclient.x64.lib"));
		}
		else if (Target.Platform == UnrealTargetPlatform.Linux)
		{
			string PlatformPath = Path.Combine(ModuleDirectory, "Linux");
			PublicIncludePaths.Add(Path.Combine(PlatformPath, "include"));
			PublicAdditionalLibraries.Add(Path.Combine(PlatformPath, "lib", "libmariadbclient.a"));

			// System library dependencies required by MariaDB connector
			PublicSystemLibraries.Add("ssl");
			PublicSystemLibraries.Add("crypto");
			PublicSystemLibraries.Add("z");
			PublicSystemLibraries.Add("dl");
			PublicSystemLibraries.Add("pthread");
		}
	}
}
