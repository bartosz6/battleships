namespace Battleships.Game.Tests

open Battleships.Game
open Xunit
open FsUnit.Xunit
open Battleships.TestCommon

module GameTests =
    [<Fact>]
    let ``hit every cell in game returns finished game with all ships sunk`` () =
        let boardSize = 5
        let game = Game.start boardSize |> Result.get
        let shootEveryCell =
            let hit row col game = boardSize |> Game.hit row col game |> Result.get
            ['A'..Coordinate.rowNumberToChar boardSize] |> Seq.map(fun row ->
                [1..boardSize]
                |> Seq.map(fun col -> hit row col)
                |> Seq.reduce(fun a b -> a >> b))
            |> Seq.reduce(fun a b -> a >> b)
            
        let result = shootEveryCell game
        
        result.IsGameFinished |> should be True
        result.Hits.Length |> should be (lessThanOrEqualTo 25)
        result.Opponent.Ships |> Seq.forall Ship.isSunk |> should be True
        
    [<Fact>]
    let ``hit cell that is occupied by ship stores Hit on list`` () =
        let boardSize = 5
        let game = Game.start boardSize |> Result.get
        let coord = game.Opponent.Ships.[0].Parts.[0].Coordinate
        let row = Coordinate.rowNumberToChar coord.Row
        let col = coord.Column
        let expectedShip = { game.Opponent.Ships.[0]
                             with Parts = game.Opponent.Ships.[0].Parts
                                          |> List.mapi(fun i p -> if i = 0 then { p with State = State.Sunk } else p) }
        
        let hit = Game.hit row col game boardSize |> Result.get 
        
        hit.Hits.[0]  |> should equal (HitResult.Hit (expectedShip, row, col))
        
    [<Fact>]
    let ``hit every cell that single ship occupies stores Sunk on list`` () =
        let boardSize = 5
        let mutable game = Game.start boardSize |> Result.get
        let ship = game.Opponent.Ships |> Seq.find(fun a -> a.Kind = ShipKind.Destroyer)
        let isSunk a = match a with | HitResult.Sunk _ -> true | _ -> false
        let isHit a = match a with | HitResult.Hit _ -> true | _ -> false
        ship.Parts |> Seq.iter (fun part ->
                                    let row = Coordinate.rowNumberToChar part.Coordinate.Row
                                    let col = part.Coordinate.Column
                                    game <- Game.hit row col game boardSize |> Result.get) 
        
        game.Hits.[0]  |> isSunk |> should be True
        game.Hits.[1]  |> isHit |> should be True
        game.Hits.[2]  |> isHit |> should be True
        game.Hits.[3]  |> isHit |> should be True
        
    [<Fact>]
    let ``hit cell that is empty stores Miss on list`` () =
        let boardSize = 10
        let game = Game.start boardSize |> Result.get
        let row = [1..boardSize]
                  |> Seq.find(fun r -> game.Opponent.Ships |> Seq.forall(fun s -> s.Parts |> Seq.forall(fun p -> p.Coordinate.Row <> r)))
                  |> Coordinate.rowNumberToChar
        let col = [1..boardSize] |> Seq.find(fun c -> game.Opponent.Ships |> Seq.forall(fun s -> s.Parts |> Seq.forall(fun p -> p.Coordinate.Column <> c)))
        
        let hit = Game.hit row col game boardSize |> Result.get 
        
        hit.Hits.[0]  |> should equal (HitResult.Miss (row, col))
        