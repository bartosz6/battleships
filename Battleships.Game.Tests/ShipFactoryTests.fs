namespace Battleships.Game.Tests

open Battleships.Game
open Xunit
open FsUnit.Xunit
open Battleships.TestCommon

module ShipFactoryTests =
    
    [<Fact>]
    let ``create produces new ship`` () =
        let ship = ShipFactory.createNewShipInFleet ShipKind.Battleship 5 [] |> Result.get
        
        ship.Kind |> should equal ShipKind.Battleship
        ship.Parts |> Seq.length |> should equal 5
        ship |> Ship.isSunk |> should be False
    
    [<Fact>]
    let ``create given board size lower than ship size returns error`` () =
        ShipFactory.createNewShipInFleet ShipKind.Battleship 4 [] |> Result.getError |> should equal ThereIsNoPlaceOnTheOcean
    
    [<Fact>]
    let ``create produces new ship between already existing ones`` () =
        let ship1 = ShipFactory.createNewShipInFleet ShipKind.Battleship 5 [] |> Result.get
        let ship2 = ShipFactory.createNewShipInFleet ShipKind.Battleship 5 [ship1] |> Result.get
        let ship3 = ShipFactory.createNewShipInFleet ShipKind.Battleship 5 [ship1;ship2] |> Result.get
        
        [ship1;ship2;ship3]
            |> Seq.collect(fun ship -> ship.Parts)
            |> Seq.groupBy(fun part -> part.Coordinate)
            |> Seq.forall(fun (_, partsOnCoordinate) -> partsOnCoordinate |> Seq.length = 1) |> should be True

