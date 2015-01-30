﻿module Paket.DependencySetSpecs

open Paket
open Paket.BindingRedirects
open NUnit.Framework
open System.Xml.Linq
open FsUnit
open Paket.PackageResolver
open Paket.Requirements
open Paket.Domain

[<Test>]
let ``empty set filtered with empty restrictions should give empty set``() = 
    Set.empty
    |> DependencySetFilter.filterByRestrictions []
    |> shouldEqual Set.empty

[<Test>]
let ``filtered with empty restrictions should give full set``() = 
    let set = 
      [PackageName("P1"), VersionRequirement.AllReleases, []
       PackageName("P2"), VersionRequirement.AllReleases, [FrameworkRestriction.AtLeast (DotNetFramework(FrameworkVersion.V4))]
       PackageName("P3"), VersionRequirement.AllReleases, [FrameworkRestriction.Exactly (DotNetFramework(FrameworkVersion.V4_5))]]
      |> Set.ofList

    set
    |> DependencySetFilter.filterByRestrictions []
    |> shouldEqual set


[<Test>]
let ``filtered with concrete restriction should filter non-matching``() = 
    let original = 
      [PackageName("P1"), VersionRequirement.AllReleases, []
       PackageName("P2"), VersionRequirement.AllReleases, [FrameworkRestriction.AtLeast (DotNetFramework(FrameworkVersion.V4))]
       PackageName("P3"), VersionRequirement.AllReleases, [FrameworkRestriction.Exactly (DotNetFramework(FrameworkVersion.V4_5))]
       PackageName("P4"), VersionRequirement.AllReleases, [FrameworkRestriction.AtLeast (DotNetFramework(FrameworkVersion.V4_5))]
       PackageName("P5"), VersionRequirement.AllReleases, [FrameworkRestriction.Between (DotNetFramework(FrameworkVersion.V4_5),DotNetFramework(FrameworkVersion.V4_5_2))]
       PackageName("P6"), VersionRequirement.AllReleases, [FrameworkRestriction.Between (DotNetFramework(FrameworkVersion.V4),DotNetFramework(FrameworkVersion.V4_5_2))]
       PackageName("P7"), VersionRequirement.AllReleases, [FrameworkRestriction.Between (DotNetFramework(FrameworkVersion.V3),DotNetFramework(FrameworkVersion.V3_5))]]
      |> Set.ofList

    let expected = 
      [PackageName("P1"), VersionRequirement.AllReleases, []
       PackageName("P2"), VersionRequirement.AllReleases, [FrameworkRestriction.AtLeast (DotNetFramework(FrameworkVersion.V4))]
       PackageName("P6"), VersionRequirement.AllReleases, [FrameworkRestriction.Between (DotNetFramework(FrameworkVersion.V4),DotNetFramework(FrameworkVersion.V4_5_2))]]
      |> Set.ofList


    original
    |> DependencySetFilter.filterByRestrictions [FrameworkRestriction.Exactly (DotNetFramework(FrameworkVersion.V4))]
    |> shouldEqual expected