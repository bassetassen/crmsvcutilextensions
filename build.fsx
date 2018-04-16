#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.AssemblyInfoFile
open Fake.ReleaseNotesHelper
open Fake.AppVeyor

let buildDir = "./build/"

let projectName = "CRMSvcUtilExtensions"
let authors = ["Sebastian Holager"]
let description = "A library with extensions to CRMSvcUtil"
let release = LoadReleaseNotes "RELEASE_NOTES.md"
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
         Attribute.Version release.AssemblyVersion
         Attribute.FileVersion release.AssemblyVersion]

    !! "/**/*.csproj"
      |> MSBuildRelease buildDir "Build"
      |> Log "Build-Output: "
)

Target "Release" (fun _ ->
    let packageDir = "./packaging/"
    let net462Dir = packageDir @@ "lib/net462/"
    CleanDirs [packageDir;net462Dir]
    let dependencies = getDependencies "./src/CRMSvcUtilExtensions/packages.config"

    CopyFile net462Dir (buildDir @@ "CRMSvcUtilExtensions.dll")

    NuGet (fun p ->
        {p with
            Project = projectName
            Authors = authors
            Version = release.NugetVersion
            OutputPath = packageDir
            WorkingDir = packageDir
            ReleaseNotes = release.Notes |> toLines
            Description = description
            Dependencies = dependencies
            AccessKey = apiKey
            Publish = hasBuildParam "apiKey"
            PublishUrl = "https://www.nuget.org/api/v2/package" })
        "./src/CRMSvcUtilExtensions/CRMSvcUtilExtensions.nuspec"
)

// Dependencies
"Clean"
  ==> "Build"

"Build"
  ==> "Release"

RunTargetOrDefault "Build"
