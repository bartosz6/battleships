namespace Battleships.Game.Tests

open FsCheck
open Battleships.Game
open Battleships.TestCommon


module Gen =
    let battleSize = 1000

    let columnCoords () =
        let rows = [ 'A' .. 'Z' ]
        let colGen = Arb.Default.Int32() |> Arb.toGen
        let rowGen = Arb.Default.Int32() |> Arb.toGen
                     |> Gen.map(fun i -> rows |> List.take (i%rows.Length |> abs)) 
        Gen.zip rowGen colGen
        |> Gen.map (fun (row, col) ->
            row
            |> List.map (fun r -> battleSize |> Coordinate.create r col)
            |> List.filter Result.isOk
            |> List.map Result.get)

    let rowCoords () =
        let cols = [1..50]
        let colGen = Arb.Default.Int32() |> Arb.toGen
                     |> Gen.map(fun i -> cols |> List.take (i%cols.Length |> abs)) 
        let rowGen = Arb.Default.Char() |> Arb.toGen
        Gen.zip rowGen colGen
        |> Gen.map (fun (row, col) ->
            col
            |> List.map (fun c -> battleSize |> Coordinate.create row c)
            |> List.filter Result.isOk
            |> List.map Result.get)

    let coord () =
        let colGen = Arb.Default.Int32() |> Arb.toGen
        let rowGen = Arb.Default.Char() |> Arb.toGen
        Gen.zip rowGen colGen
        |> Gen.map (fun (r, c) -> battleSize |> Coordinate.create r c)
        |> Gen.filter Result.isOk
        |> Gen.map Result.get

    let coordsNotOrdered () =
        let colGen = Arb.Default.Int32() |> Arb.toGen
        let rowGen = Arb.Default.Char() |> Arb.toGen |> Gen.listOf
        Gen.zip rowGen colGen
        |> Gen.map (fun (row, col) ->
            row
            |> List.map (fun r -> battleSize |> Coordinate.create r col)
            |> List.filter Result.isOk
            |> List.map Result.get)