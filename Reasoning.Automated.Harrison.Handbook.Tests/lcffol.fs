﻿// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Reasoning.Automated.Harrison.Handbook.Tests.lcffol

open Reasoning.Automated.Harrison.Handbook.lib
open Reasoning.Automated.Harrison.Handbook.folMod
open Reasoning.Automated.Harrison.Handbook.lcf
open Reasoning.Automated.Harrison.Handbook.lcfprop
open Reasoning.Automated.Harrison.Handbook.lcffol
open NUnit.Framework
open FsUnit

// pg. 501

let private lcfrefute_results = [| 
                                   @"p(1) /\ ~q(1) /\ (forall x. p(x) ==> q(x)) ==> false";
                                   @"(exists x. ~p(x)) /\ (forall x. p(x)) ==> (~(~p(f_1)) ==> (forall x. ~(~p(x)))) ==> false";
                                    |]

[<TestCase(@"p(1) /\ ~q(1) /\ (forall x. p(x) ==> q(x))", 0)>]
[<TestCase(@"(exists x. ~p(x)) /\ (forall x. p(x))", 1)>]
let ``lcfrefute`` (f, idx) =
    lcfrefute (parse f) 1 simpcont 
    |> should equal (parse lcfrefute_results.[idx])

// pg. 504
//  ------------------------------------------------------------------------- // 
//  Examples in the text.                                                     // 
//  ------------------------------------------------------------------------- // 

let private lcffol_results = [| 
                                @"forall x.
                                    exists v w.
                                    forall y z. P(x) /\ Q(y) ==> (P(v) \/ R(w)) /\ (R(z) ==> Q(v))"; // 0
                                @"(forall x. x <= x) /\
                                    (forall x y z. x <= y /\ y <= z ==> x <= z) /\
                                    (forall x y. f(x) <= y <=> x <= g(y)) ==>
                                    (forall x y. x <= y ==> f(x) <= f(y))"; // 1

                                @"p ==> q <=> ~q ==> ~p"; // 2
                                @"~(~p) <=> p"; // 3
                                @"~(p ==> q) ==> q ==> p"; // 4
                                
                                @"~p ==> q <=> ~q ==> p"; // 5
                                @"(p \/ q ==> p \/ r) ==> p \/ (q ==> r)"; // 6
                                @"p \/ ~p"; // 7
                                @"p \/ ~(~(~p))"; // 8
                                @"((p ==> q) ==> p) ==> p"; // 9
                                @"(p \/ q) /\ (~p \/ q) /\ (p \/ ~q) ==> ~(~q \/ ~q)"; // 10
                                @"(q ==> r) /\ (r ==> p /\ q) /\ (p ==> q /\ r) ==> (p <=> q)"; // 11
                                @"p <=> p"; // 12
                                @"((p <=> q) <=> r) <=> p <=> q <=> r"; // 13
                                @"p \/ q /\ r <=> (p \/ q) /\ (p \/ r)"; // 14
                                @"(p <=> q) <=> (q \/ ~p) /\ (~q \/ p)"; // 15
                                @"p ==> q <=> ~p \/ q"; // 16
                                @"(p ==> q) \/ (q ==> p)"; // 17
                                
                                @"p /\ (q ==> r) ==> s <=> (~p \/ q \/ s) /\ (~p \/ ~r \/ s)"; // 18
                                @"p ==> q <=> ~q ==> ~p"; // 19
                                @"~(~p) <=> p"; // 20
                                @"~(p ==> q) ==> q ==> p"; // 21

                                @"~p ==> q <=> ~q ==> p"; // 22
                                @"(p \/ q ==> p \/ r) ==> p \/ (q ==> r)"; // 23
                                @"p \/ ~p"; // 24
                                @"p \/ ~(~(~p))"; // 25
                                @"((p ==> q) ==> p) ==> p"; // 26
                                @"(p \/ q) /\ (~p \/ q) /\ (p \/ ~q) ==> ~(~q \/ ~q)"; // 27
                                @"(q ==> r) /\ (r ==> p /\ q) /\ (p ==> q /\ r) ==> (p <=> q)"; // 28
                                @"((p <=> q) <=> r) <=> p <=> q <=> r"; // 29
                                @"p \/ q /\ r <=> (p \/ q) /\ (p \/ r)"; // 30
                                @"(p <=> q) <=> (q \/ ~p) /\ (~q \/ p)"; // 31
                                @"p ==> q <=> ~p \/ q"; // 32
                                @"(p ==> q) \/ (q ==> p)"; // 33

                                @"p /\ (q ==> r) ==> s <=> (~p \/ q \/ s) /\ (~p \/ ~r \/ s)"; // 34
                                @"exists y. forall x. P(y) ==> P(x)"; // 35
                                @"exists x. forall y z. (P(y) ==> Q(z)) ==> P(x) ==> Q(x)"; // 36
                                @"(forall x y. exists z. forall w. P(x) /\ Q(y) ==> R(z) /\ U(w)) ==> (exists x y. P(x) /\ Q(y)) ==> (exists z. R(z))"; // 37
                                @"(exists x. P ==> Q(x)) /\ (exists x. Q(x) ==> P) ==> (exists x. P <=> Q(x))"; // 38
                                @"(forall x. P <=> Q(x)) ==> (P <=> (forall x. Q(x)))"; // 39
                                @"(forall x. P \/ Q(x)) <=> P \/ (forall x. Q(x))"; // 40
                                @"~(exists x. U(x) /\ Q(x)) /\ (forall x. P(x) ==> Q(x) \/ R(x)) /\ ~(exists x. P(x) ==> (exists x. Q(x))) /\ (forall x. Q(x) /\ R(x) ==> U(x)) ==> (exists x. P(x) /\ R(x))"; // 41
                                @"(exists x. P(x)) /\ (forall x. U(x) ==> ~G(x) /\ R(x)) /\ (forall x. P(x) ==> G(x) /\ U(x)) /\ ((forall x. P(x) ==> Q(x)) \/ (exists x. Q(x) /\ P(x))) ==> (exists x. Q(x) /\ P(x))"; // 42
                                @"((exists x. P(x)) <=> (exists x. Q(x))) /\ (forall x y. P(x) /\ Q(y) ==> (R(x) <=> U(y))) ==> ((forall x. P(x) ==> R(x)) <=> (forall x. Q(x) ==> U(x)))"; // 43
                                @"(exists x. P(x) /\ ~Q(x)) /\ (forall x. P(x) ==> R(x)) /\ (forall x. U(x) /\ V(x) ==> P(x)) /\ (exists x. R(x) /\ ~Q(x)) ==> (forall x. U(x) ==> ~R(x)) ==> (forall x. U(x) ==> ~V(x))"; // 44
                                @"(forall x. P(x) ==> (forall x. Q(x))) /\ ((forall x. Q(x) \/ R(x)) ==> (exists x. Q(x) /\ R(x))) /\ ((exists x. R(x)) ==> (forall x. L(x) ==> M(x))) ==> (forall x. P(x) /\ L(x) ==> M(x))"; // 45
                                @"(exists x. P(x)) /\ (exists x. G(x)) ==> ((forall x. P(x) ==> H(x)) /\ (forall x. G(x) ==> J(x)) <=> (forall x y. P(x) /\ G(y) ==> H(x) /\ J(y)))"; // 46
                                @"(forall x. P(x) \/ G(x) ==> ~H(x)) /\ (forall x. (G(x) ==> ~U(x)) ==> P(x) /\ H(x)) ==> (forall x. U(x))"; // 47
                                @"~(exists x. P(x) /\ (G(x) \/ H(x))) /\ (exists x. Q(x) /\ P(x)) /\ (forall x. ~H(x) ==> J(x)) ==> (exists x. Q(x) /\ J(x))"; // 48
                                @"(forall x. P(x) /\ (G(x) \/ H(x)) ==> Q(x)) /\ (forall x. Q(x) /\ H(x) ==> J(x)) /\ (forall x. R(x) ==> H(x)) ==> (forall x. P(x) /\ R(x) ==> J(x))"; // 49
                                @"(forall x. P(a) /\ (P(x) ==> P(b)) ==> P(c)) <=> (forall x. P(a) ==> P(x) \/ P(c)) /\ (P(a) ==> P(b) ==> P(c))"; // 50
                                    |]

[<TestCase(@"p ==> q <=> ~q ==> ~p", 2)>]
[<TestCase(@"~ ~p <=> p", 3)>]
[<TestCase(@"~(p ==> q) ==> q ==> p", 4)>]
[<TestCase(@"~p ==> q <=> ~q ==> p", 5)>]
[<TestCase(@"(p \/ q ==> p \/ r) ==> p \/ (q ==> r)", 6)>]
[<TestCase(@"p \/ ~p", 7)>]
[<TestCase(@"p \/ ~ ~ ~p", 8)>]
[<TestCase(@"((p ==> q) ==> p) ==> p", 9)>]
[<TestCase(@"(p \/ q) /\ (~p \/ q) /\ (p \/ ~q) ==> ~(~q \/ ~q)", 10)>]
[<TestCase(@"(q ==> r) /\ (r ==> p /\ q) /\ (p ==> q /\ r) ==> (p <=> q)", 11)>]
[<TestCase(@"p <=> p", 12)>]
[<TestCase(@"((p <=> q) <=> r) <=> (p <=> (q <=> r))", 13)>]
[<TestCase(@"p \/ q /\ r <=> (p \/ q) /\ (p \/ r)", 14)>]
[<TestCase(@"(p <=> q) <=> (q \/ ~p) /\ (~q \/ p)", 15)>]
[<TestCase(@"p ==> q <=> ~p \/ q", 16)>]
[<TestCase(@"(p ==> q) \/ (q ==> p)", 17)>]
[<TestCase(@"p /\ (q ==> r) ==> s <=> (~p \/ q \/ s) /\ (~p \/ ~r \/ s)", 18)>]
[<TestCase(@"p ==> q <=> ~q ==> ~p", 19)>]
[<TestCase(@"~ ~p <=> p", 20)>]
[<TestCase(@"~(p ==> q) ==> q ==> p", 21)>]
let ``lcftaut`` (f, idx) =
    lcftaut (parse f) 
    |> should equal (parse lcffol_results.[idx])

[<TestCase(@"forall x. exists v. exists w. forall y. forall z. ((P(x) /\ Q(y)) ==> ((P(v) \/ R(w))  /\ (R(z) ==> Q(v))))", 0)>]
[<TestCase(@"(forall x. x <= x) /\ (forall x y z. x <= y /\ y <= z ==> x <= z) /\ (forall x y. f(x) <= y <=> x <= g(y)) ==> (forall x y. x <= y ==> f(x) <= f(y))", 1)>]
[<TestCase(@"~p ==> q <=> ~q ==> p", 22)>]
[<TestCase(@"(p \/ q ==> p \/ r) ==> p \/ (q ==> r)", 23)>]
[<TestCase(@"p \/ ~p", 24)>]
[<TestCase(@"p \/ ~ ~ ~p", 25)>]
[<TestCase(@"((p ==> q) ==> p) ==> p", 26)>]
[<TestCase(@"(p \/ q) /\ (~p \/ q) /\ (p \/ ~q) ==> ~(~q \/ ~q)", 27)>]
[<TestCase(@"(q ==> r) /\ (r ==> p /\ q) /\ (p ==> q /\ r) ==> (p <=> q)", 28)>]
[<TestCase(@"((p <=> q) <=> r) <=> (p <=> (q <=> r))", 29)>]
[<TestCase(@"p \/ q /\ r <=> (p \/ q) /\ (p \/ r)", 30)>]
[<TestCase(@"(p <=> q) <=> (q \/ ~p) /\ (~q \/ p)", 31)>]
[<TestCase(@"p ==> q <=> ~p \/ q", 32)>]
[<TestCase(@"(p ==> q) \/ (q ==> p)", 33)>]
[<TestCase(@"p /\ (q ==> r) ==> s <=> (~p \/ q \/ s) /\ (~p \/ ~r \/ s)", 34)>]
[<TestCase(@"exists y. forall x. P(y) ==> P(x)", 35)>]
[<TestCase(@"exists x. forall y z. (P(y) ==> Q(z)) ==> P(x) ==> Q(x)", 36)>]
[<TestCase(@"(forall x y. exists z. forall w. P(x) /\ Q(y) ==> R(z) /\ U(w)) ==> (exists x y. P(x) /\ Q(y)) ==> (exists z. R(z))", 37)>]
[<TestCase(@"(exists x. P ==> Q(x)) /\ (exists x. Q(x) ==> P) ==> (exists x. P <=> Q(x))", 38)>]
[<TestCase(@"(forall x. P <=> Q(x)) ==> (P <=> (forall x. Q(x)))", 39)>]
[<TestCase(@"(forall x. P \/ Q(x)) <=> P \/ (forall x. Q(x))", 40)>]
[<TestCase(@"~(exists x. U(x) /\ Q(x)) /\ (forall x. P(x) ==> Q(x) \/ R(x)) /\ ~(exists x. P(x) ==> (exists x. Q(x))) /\ (forall x. Q(x) /\ R(x) ==> U(x)) ==> (exists x. P(x) /\ R(x))", 41)>]
[<TestCase(@"(exists x. P(x)) /\ (forall x. U(x) ==> ~G(x) /\ R(x)) /\ (forall x. P(x) ==> G(x) /\ U(x)) /\ ((forall x. P(x) ==> Q(x)) \/ (exists x. Q(x) /\ P(x))) ==> (exists x. Q(x) /\ P(x))", 42)>]
[<TestCase(@"((exists x. P(x)) <=> (exists x. Q(x))) /\ (forall x y. P(x) /\ Q(y) ==> (R(x) <=> U(y))) ==> ((forall x. P(x) ==> R(x)) <=> (forall x. Q(x) ==> U(x)))", 43)>]
[<TestCase(@"(exists x. P(x) /\ ~Q(x)) /\ (forall x. P(x) ==> R(x)) /\ (forall x. U(x) /\ V(x) ==> P(x)) /\ (exists x. R(x) /\ ~Q(x)) ==> (forall x. U(x) ==> ~R(x)) ==> (forall x. U(x) ==> ~V(x))", 44)>]
[<TestCase(@"(forall x. P(x) ==> (forall x. Q(x))) /\ ((forall x. Q(x) \/ R(x)) ==> (exists x. Q(x) /\ R(x))) /\ ((exists x. R(x)) ==> (forall x. L(x) ==> M(x))) ==> (forall x. P(x) /\ L(x) ==> M(x))", 45)>]
[<TestCase(@"(exists x. P(x)) /\ (exists x. G(x)) ==> ((forall x. P(x) ==> H(x)) /\ (forall x. G(x) ==> J(x)) <=> (forall x y. P(x) /\ G(y) ==> H(x) /\ J(y)))", 46)>]
[<TestCase(@"(forall x. P(x) \/ G(x) ==> ~H(x)) /\ (forall x. (G(x) ==> ~U(x)) ==> P(x) /\ H(x)) ==> (forall x. U(x))", 47)>]
[<TestCase(@"~(exists x. P(x) /\ (G(x) \/ H(x))) /\ (exists x. Q(x) /\ P(x)) /\ (forall x. ~H(x) ==> J(x)) ==> (exists x. Q(x) /\ J(x))", 48)>]
[<TestCase(@"(forall x. P(x) /\ (G(x) \/ H(x)) ==> Q(x)) /\ (forall x. Q(x) /\ H(x) ==> J(x)) /\ (forall x. R(x) ==> H(x)) ==> (forall x. P(x) /\ R(x) ==> J(x))", 49)>]
[<TestCase(@"(forall x. P(a) /\ (P(x) ==> P(b)) ==> P(c)) <=> (forall x. P(a) ==> P(x) \/ P(c)) /\ (P(a) ==> P(b) ==> P(c))", 50)>]
let ``lcffol all`` (f, idx) =
    lcffol (parse f) 
    |> should equal (parse lcffol_results.[idx])
 
//  ------------------------------------------------------------------------- // 
//  More exhaustive set of tests not in the main text.                        // 
//  ------------------------------------------------------------------------- // 

let private lcffol_gilmore_results = [| 
                                        @"exists x.
                                            forall y z.
                                            ((F(y,z) ==> G(y) ==> H(x)) ==> F(x,x)) /\
                                            ((F(z,x) ==> G(x)) ==> H(z)) /\ F(x,y) ==> F(z,z)"; // 0
                                        @"exists x y.
                                            forall z.
                                            (F(x,y) ==> F(y,z) /\ F(z,z)) /\
                                            (F(x,y) /\ G(x,y) ==> G(x,z) /\ G(z,z))"; // 1
                                        @"(forall x. exists y. F(x,y) \/ F(y,x)) /\
                                            (forall x y. F(y,x) ==> F(y,y)) ==> (exists z. F(z,z))"; // 2
                                        @"forall x.
                                            exists y.
                                            (exists u. forall v. F(u,x) ==> G(v,u) /\ G(u,x)) ==>
                                            (exists u. forall v. F(u,y) ==> G(v,u) /\ G(u,y)) \/
                                            (forall u v. exists w. G(v,u) \/ H(w,y,u) ==> G(u,w))"; // 3
                                        @"(forall x. K(x) ==> (exists y. L(y) /\ (F(x,y) ==> G(x,y)))) /\
                                            (exists z. K(z) /\ (forall u. L(u) ==> F(z,u))) ==>
                                            (exists v w. K(v) /\ L(w) /\ G(v,w))"; // 4
                                        @"exists x.
                                            forall y z.
                                            ((F(y,z) ==> G(y) ==> (forall u. exists v. H(u,v,x))) ==> F(x,x)) /\
                                            ((F(z,x) ==> G(x)) ==> (forall u. exists v. H(u,v,z))) /\ F(x,y) ==>
                                            F(z,z)"; // 5
                                        @"forall x.
                                            exists y.
                                            forall z.
                                            ((forall u. exists v. F(y,u,v) /\ G(y,u) /\ ~H(y,x)) ==>
                                            (forall u. exists v. F(x,u,v) /\ G(z,u) /\ ~H(x,z)) ==>
                                            (forall u. exists v. F(x,u,v) /\ G(y,u) /\ ~H(x,y))) /\
                                            ((forall u. exists v. F(x,u,v) /\ G(y,u) /\ ~H(x,y)) ==>
                                            ~(forall u. exists v. F(x,u,v) /\ G(z,u) /\ ~H(x,z)) ==>
                                            (forall u. exists v. F(y,u,v) /\ G(y,u) /\ ~H(y,x)) /\
                                            (forall u. exists v. F(z,u,v) /\ G(y,u) /\ ~H(z,y)))"; // 6
                                    |]

[<TestCase(@"(forall x. exists y. F(x,y) \/ F(y,x)) /\ (forall x y. F(y,x) ==> F(y,y)) ==> exists z. F(z,z)", 2)>]
[<TestCase(@"forall x. exists y. (exists u. forall v. F(u,x) ==> G(v,u) /\ G(u,x)) ==> (exists u. forall v. F(u,y) ==> G(v,u) /\ G(u,y)) \/ (forall u v. exists w. G(v,u) \/ H(w,y,u) ==> G(u,w))", 3)>]
[<TestCase(@"(forall x. K(x) ==> exists y. L(y) /\ (F(x,y) ==> G(x,y))) /\ (exists z. K(z) /\ forall u. L(u) ==> F(z,u)) ==> exists v w. K(v) /\ L(w) /\ G(v,w)", 4)>]
let ``lcffol gilmore fast`` (f, idx) =
    lcffol (parse f) 
    |> should equal (parse lcffol_gilmore_results.[idx])

[<TestCase(@"exists x. forall y z. ((F(y,z) ==> (G(y) ==> H(x))) ==> F(x,x)) /\ ((F(z,x) ==> G(x)) ==> H(z)) /\ F(x,y) ==> F(z,z)", 0); Category("LongRunning")>]
[<TestCase(@"exists x y. forall z. (F(x,y) ==> F(y,z) /\ F(z,z)) /\ (F(x,y) /\ G(x,y) ==> G(x,z) /\ G(z,z))", 1); Category("LongRunning")>]
[<TestCase(@"exists x. forall y z. ((F(y,z) ==> (G(y) ==> (forall u. exists v. H(u,v,x)))) ==> F(x,x)) /\ ((F(z,x) ==> G(x)) ==> (forall u. exists v. H(u,v,z))) /\ F(x,y) ==> F(z,z)", 5); Category("LongRunning")>]
[<TestCase(@"forall x. exists y. forall z. ((forall u. exists v. F(y,u,v) /\ G(y,u) /\ ~H(y,x)) ==> (forall u. exists v. F(x,u,v) /\ G(z,u) /\ ~H(x,z)) ==> (forall u. exists v. F(x,u,v) /\ G(y,u) /\ ~H(x,y))) /\ ((forall u. exists v. F(x,u,v) /\ G(y,u) /\ ~H(x,y)) ==> ~(forall u. exists v. F(x,u,v) /\ G(z,u) /\ ~H(x,z)) ==> (forall u. exists v. F(y,u,v) /\ G(y,u) /\ ~H(y,x)) /\ (forall u. exists v. F(z,u,v) /\ G(y,u) /\ ~H(z,y)))", 6); Category("LongRunning")>]
let ``lcffol gilmore slow`` (f, idx) =
    lcffol (parse f) 
    |> should equal (parse lcffol_gilmore_results.[idx])
