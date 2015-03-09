﻿#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.AssemblyInfoFile

let buildDir = "./build/"

let projectName = "CRMSvcUtilExtensions"
let version = "1.0.1"
let authors = ["Sebastian Holager"]
let description = "A library with extensions to CRMSvcUtil"
let releasenotes = ""
let apiKey = getBuildParam "apiKey"

Target "Clean" (fun _ ->
    CleanDir buildDir
)

Target "Build" (fun _ ->
    RestorePackages()

    CreateCSharpAssemblyInfo "./src/CRMSvcUtilExtensions/Properties/AssemblyInfo.cs"
        [Attribute.Title projectName
         Attribute.Description description
         Attribute.Product projectName
         Attribute.Guid "76e6ae49-230a-472b-a132-e8ad2f821e64"
         Attribute.Version version
         Attribute.FileVersion version]

    !! "/**/*.csproj"
      |> MSBuildRelease buildDir "Build"
      |> Log "Build-Output: "
)

Target "Release" (fun _ ->
    let packageDir = "./packaging/"
    let net45Dir = packageDir @@ "lib/net45/"
    CleanDirs [packageDir;net45Dir]

    CopyFile net45Dir (buildDir @@ "CRMSvcUtilExtensions.dll")

    NuGet (fun p -> 
        {p with
            Project = projectName
            Authors = authors
            Version = version
            OutputPath = packageDir
            WorkingDir = packageDir
            ReleaseNotes = releasenotes
            Description = description
            AccessKey = apiKey
            Publish = hasBuildParam "apiKey" })
        "./src/CRMSvcUtilExtensions/CRMSvcUtilExtensions.nuspec"
)

// Dependencies
"Clean"
  ==> "Build"

"Build"
  ==> "Release"

RunTargetOrDefault "Build"