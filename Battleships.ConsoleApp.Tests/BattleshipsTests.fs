namespace Battleships.ConsoleApp.Tests

open System
open System.IO
open System.Text
open Battleships.Game
open Xunit
open Battleships.ConsoleApp
open Battleships.TestCommon
open FsUnit.Xunit

module BattleshipsTests =
    let buildOutput lines =
        let builder = StringBuilder()
        lines |> Seq.iter(fun line-> builder.AppendLine(line) |> ignore)
        builder.ToString()
    let startGameWith1Ship () =
        Game.start 5
        |> Result.map (fun g ->
            let destroyer =
                g.Opponent.Ships
                |> Seq.find (fun s -> s.Kind = Destroyer)

            let ship =
                { destroyer with
                      Parts =
                          destroyer.Parts
                          |> List.mapi (fun i part ->
                              { part with
                                    Coordinate = { Column = i + 1; Row = 1 } }) }

            let playerWithOneShip = { Ships = [ ship ] }
            { g with Opponent = playerWithOneShip })

    [<Fact>]
    let ``play when game is finished exit`` () =
        async {
            use input = new StringReader("")
            use output = new StringWriter()

            let game = Game.start 5 |> Result.map (fun g -> { g with IsGameFinished = true })

            do! Battleships.play input output game

            let expectedOutput = buildOutput [
                "========== 0 =========="
                "It's done! It took you 0 steps to finish your enemy."
            ]
            output.ToString() |> should equal expectedOutput
        }

    [<Fact>]
    let ``play when game is not finished move to next step until it is finished`` () =
        async {
            use input =
                new StringReader(StringBuilder().AppendLine("A2").AppendLine("A4").AppendLine("A1").AppendLine("A3").ToString())

            use output = new StringWriter()

            let game = startGameWith1Ship ()

            do! Battleships.play input output game

            let expectedOutput = buildOutput [
                "========== 0 =========="
                "shoot: ========== 1 =========="
                "[A 2] HIT! Destroyer"
                "shoot: ========== 2 =========="
                "[A 4] HIT! Destroyer"
                "shoot: ========== 3 =========="
                "[A 1] HIT! Destroyer"
                "shoot: ========== 4 =========="
                "[A 3] SUNK! Destroyer"
                "It's done! It took you 4 steps to finish your enemy."
            ]
            output.ToString() |> should equal expectedOutput
        }

    [<Fact>]
    let ``play after error can be finished`` () =
        async {
            use input =
                new StringReader(StringBuilder().AppendLine("A2").AppendLine("A4").AppendLine("this wont work").AppendLine("A1").AppendLine("A3").ToString())

            use output = new StringWriter()

            let game = startGameWith1Ship ()

            do! Battleships.play input output game

            let expectedOutput = buildOutput [
                "========== 0 =========="
                "shoot: ========== 1 =========="
                "[A 2] HIT! Destroyer"
                "shoot: ========== 2 =========="
                "[A 4] HIT! Destroyer"
                "shoot: coordinates 'this wont work' are invalid, use coordinates from 'A1' to 'J10'"
                "shoot: ========== 3 =========="
                "[A 1] HIT! Destroyer"
                "shoot: ========== 4 =========="
                "[A 3] SUNK! Destroyer"
                "It's done! It took you 4 steps to finish your enemy."
            ]
            output.ToString() |> should equal expectedOutput
                   
        }


    [<Fact>]
    let ``play when game fails to start quits`` () =
        async {
            use input = new StringReader("")
            use output = new StringWriter()

            let game = Error Errors.ThereIsNoPlaceOnTheOcean

            do! Battleships.play input output game

            output.ToString() |> should equal ("something went wrong ;)" + Environment.NewLine)
        }
