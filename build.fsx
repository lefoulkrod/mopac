#I "packages/build/FAKE/tools"
#r "FakeLib.dll"

open Fake

let buildToolsPath = __SOURCE_DIRECTORY__ </> "packages"
let projectName = "Mopac"

module Build = 
    let release = ("Configuration", "Release")
    let slnAnyCPU = ("Platform", "Any CPU")
    let projAnyCPU = ("Platform", "AnyCPU")
    let debug = ("Configuration", "Debug")
    let restorePackages = ("RestorePackages", "true")

    let rebuild = "Rebuild"
    let package = "Package"
    
    let build targets props sln_or_proj = 
        let buildParams (bp : Fake.MSBuildHelper.MSBuildParams) = 
            { bp with 
                Targets = targets
                Properties = props
                Verbosity = Some(MSBuildVerbosity.Quiet) }
        Fake.MSBuildHelper.build buildParams sln_or_proj

Target "build" (fun _ -> 
    Build.build [Build.rebuild] ([Build.debug; Build.slnAnyCPU; Build.restorePackages]) (sprintf "%s.sln" projectName)
)

//Target "package" (fun _ -> 
//    let generateReleaseNotes (semVer : Fake.SemVerHelper.SemVerInfo) triggerer url_to_commit =
//        sprintf "%s triggered by %s from GitLab commit %s" 
//            (string semVer) 
//            triggerer
//            url_to_commit
//
//    let commitUrl = 
//        let currentSHA = Fake.Git.Information.getCurrentSHA1 __SOURCE_DIRECTORY__
//        let repourl = Fake.Git.CommandHelper.runSimpleGitCommand __SOURCE_DIRECTORY__ "remote get-url origin"
//        repourl.Replace(".git", sprintf "/commit/%s" currentSHA) // we need to repo name in the url to use as the base of all the per-commit urls to come
//    
//    let releaseNotes = generateReleaseNotes version (getBuildParamOrDefault "triggeredBy" "local") commitUrl
//
//    printfn "notes: %s" releaseNotes
//
//    let packParams (p : Paket.PaketPackParams) = 
//        { p with 
//            OutputPath = __SOURCE_DIRECTORY__ </> "build"
//            Version = formatNugetVersionNumber version
//            Symbols = true
//            ReleaseNotes = releaseNotes }
//    Paket.Pack packParams
//)

open Fake.Testing.XUnit2

Target "test" (fun _ ->
    let testparams (p : XUnit2Params) = {
        p with 
            XmlOutputPath = Some "xunit_output.xml"
            Parallel = NoParallelization
    }
    Paket.Restore(fun p -> {p with Group = "test" })
    let dlls = !!"test/**/bin/**/*Test*.dll"
    dlls |> xUnit2 testparams
)

Target "release" (fun _ -> 
    Build.build [Build.rebuild] ([Build.release; Build.slnAnyCPU; Build.restorePackages]) (sprintf "%s.sln" projectName)
)

"build" ==> "test"
//"release" ==> "package"
RunTargetOrDefault "build"