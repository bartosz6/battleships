namespace Battleships.ConsoleApp.Tests

open System
open Battleships.ConsoleApp
open FsCheck.Xunit
open Xunit
open Battleships.TestCommon
open FsUnit.Xunit

module CoordinateParserTests =

    [<Property>]
    let ``given some char and '2' if char is a-z returns success`` (row: char) =
        CoordinateParser.parseUserInput (String([|row ; '2'|])) |> Result.isOk |> should equal (Char.IsLetter row)

    [<Property>]
    let ``given 'a' and some char if char is number returns success`` (col: char) =
        CoordinateParser.parseUserInput (String([|'a' ; col|])) |> Result.isOk |> should equal (Char.IsDigit col)
        
    [<Fact>]
    let ``given lower char returns big upper`` () =
        ['a'..'z'] |> Seq.iter (fun row ->
            CoordinateParser.parseUserInput (String([|row ; '2'|])) |> Result.get |> should equal (row |> Char.ToUpper, 2))