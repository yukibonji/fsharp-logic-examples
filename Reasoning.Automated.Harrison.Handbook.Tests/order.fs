﻿// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Reasoning.Automated.Harrison.Handbook.Tests

module order =
    open Reasoning.Automated.Harrison.Handbook.lib
    open Reasoning.Automated.Harrison.Handbook.folMod
    open Reasoning.Automated.Harrison.Handbook.order

    open NUnit.Framework
    open FsUnit

    let s = parset "f(x,x,x)"
    let t = parset "g(x,y)"

    [<Test>]
    let ``test termsize 1``() =
        termsize s > termsize t
        |> should be True

    // ------------------------------------------------------------------------- //
    // This fails the rewrite properties.                                        //
    // ------------------------------------------------------------------------- //

    let i = "y" |=> parset "f(x,x,x)"

    [<Test>]
    let ``test termsize 2``() =
        termsize (tsubst i s) > termsize (tsubst i t)
        |> should be False

    