namespace Battleships.Game.Tests

open Battleships.Game
open Xunit
open FsUnit.Xunit
open Battleships.TestCommon

module PlayerTests =
    [<Fact>]
    let ``createCpu returns player with 3 ships: 1 battleship and 2 destroyers located inside 'battlefieldSize' boundaries`` () =
        let cpu = Player.createCpu 5 |> Result.get
        
        cpu.Ships |> Seq.forall(fun ship ->
            ship.Parts |> Seq.forall(fun part ->
                part.Coordinate.Column <= 5 && part.Coordinate.Column >= 1
                && part.Coordinate.Row <= 5 && part.Coordinate.Row >= 1)) |> should be True
        cpu.Ships |> Seq.filter(fun ship -> ship.Kind = Battleship) |> Seq.length |> should equal 1
        cpu.Ships |> Seq.filter(fun ship -> ship.Kind = Destroyer) |> Seq.length |> should equal 2