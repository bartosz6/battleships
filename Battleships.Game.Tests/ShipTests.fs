namespace Battleships.Game.Tests

open Xunit
open FsCheck.Xunit
open FsUnit.Xunit
open Battleships.Game
open FsCheck
open Battleships.TestCommon

module ShipTests =
    
    type FiveCoordinatesInColumn = static member Coordinate () = Gen.columnCoords() |> Gen.filter(fun x -> x.Length = 5) |> Arb.fromGen
    [<Property(Arbitrary = [| typeof<FiveCoordinatesInColumn> |])>]
    let ``create given 5 coordinates in the same column returns battleship`` coords =
        let ship = Ship.createShip coords |> Result.get
        ship |> Ship.getCoords |> should equal coords
        ship |> Ship.getKind |> should equal ShipKind.Battleship
        ship |> Ship.isSunk |> should be False
    
    type FiveCoordinatesInRow = static member Coordinate () = Gen.rowCoords() |> Gen.filter(fun x -> x.Length = 5) |> Arb.fromGen
    [<Property(Arbitrary = [| typeof<FiveCoordinatesInRow> |])>]
    let ``create given 5 coordinates in the same row returns battleship`` coords =
        let ship = Ship.createShip coords |> Result.get
        ship |> Ship.getCoords |> should equal coords
        ship |> Ship.getKind |> should equal ShipKind.Battleship
        ship |> Ship.isSunk |> should be False
        
    type FourCoordinatesInRow = static member Coordinate () = Gen.rowCoords() |> Gen.filter(fun x -> x.Length = 4) |> Arb.fromGen
    [<Property(Arbitrary = [| typeof<FourCoordinatesInRow> |])>]
    let ``create given 4 coordinates in the same row returns destroyer`` coords =
        let ship = Ship.createShip coords |> Result.get
        ship |> Ship.getCoords |> should equal coords
        ship |> Ship.getKind |> should equal ShipKind.Destroyer
        ship |> Ship.isSunk |> should be False
        
    type FourCoordinatesInColumn = static member Coordinate () = Gen.columnCoords() |> Gen.filter(fun x -> x.Length = 4) |> Arb.fromGen
    [<Property(Arbitrary = [| typeof<FourCoordinatesInColumn> |])>]
    let ``create given 4 coordinates in the same column returns destroyer`` coords =
        let ship = Ship.createShip coords |> Result.get
        ship |> Ship.getCoords |> should equal coords
        ship |> Ship.getKind |> should equal ShipKind.Destroyer
        ship |> Ship.isSunk |> should be False
        
    type Not4Nor5CoordinatesInColumn = static member Coordinate () = Gen.columnCoords() |> Gen.filter(fun x -> x.Length < 4 || x.Length > 5) |> Arb.fromGen
    [<Property(Arbitrary = [| typeof<Not4Nor5CoordinatesInColumn> |])>]
    let ``create given less than 4 or more than 5 coordinates in the same column returns error`` coords =
        Ship.createShip coords |> Result.getError = Errors.IncorrectNumberOfParts
        
    type Not4Nor5CoordinatesInRow = static member Coordinate () = Gen.rowCoords() |> Gen.filter(fun x -> x.Length < 4 || x.Length > 5) |> Arb.fromGen
    [<Property(Arbitrary = [| typeof<Not4Nor5CoordinatesInColumn> |])>]
    let ``create given less than 4 or more than 5 coordinates in the same row returns error`` coords =
        Ship.createShip coords |> Result.getError = Errors.IncorrectNumberOfParts
        
    type CoordinatesNotInARowOrColumn = static member Coordinate () = Gen.coord() |> Gen.listOfLength 4 |> Arb.fromGen
    [<Property(Arbitrary = [| typeof<CoordinatesNotInARowOrColumn> |])>]
    let ``create given coordinates not in the same row or column returns error`` coords =
        Ship.createShip coords |> Result.getError |> should equal Errors.PartsNotInInlined
        
    type CoordinatesNotInOrder = static member Coordinate () = Gen.coordsNotOrdered() |> Gen.filter(fun x->x.Length>=4 && x.Length<=5) |> Arb.fromGen
    [<Property(Arbitrary = [| typeof<CoordinatesNotInOrder> |])>]
    let ``create given coordinates not ordered returns error`` coords =
        Ship.createShip coords |> Result.getError |> should equal Errors.PartsNotInInlined
     
    let battleSize = Gen.battleSize
    let createShip row =
        Ship.createShip
                    [
                        battleSize |> Coordinate.create row 5 |> Result.get
                        battleSize |> Coordinate.create row 6 |> Result.get
                        battleSize |> Coordinate.create row 7 |> Result.get
                        battleSize |> Coordinate.create row 8 |> Result.get
                    ] |> Result.get
        
    [<Fact>]
    let ``hit given coordinates and ship that does contain this coordinate returns ship with part sunk`` () =
        let ship = createShip 'A'
        let hitCoord = battleSize |> Coordinate.create 'A' 6 |> Result.get
        
        let afterHit = Ship.hit hitCoord ship
        
        afterHit.Parts.[0].State |> should equal State.Floating
        afterHit.Parts.[1].State |> should equal State.Sunk
        afterHit.Parts.[2].State |> should equal State.Floating
        afterHit.Parts.[3].State |> should equal State.Floating
        
    [<Fact>]
    let ``hit given coordinates and ship that does not contain this coordinate returns ship with parts unchanged`` () =
        let ship = createShip 'A'
        let hitCoord = battleSize |> Coordinate.create 'B' 6 |> Result.get
        
        let afterHit = Ship.hit hitCoord ship
        
        afterHit.Parts.[0].State |> should equal State.Floating
        afterHit.Parts.[1].State |> should equal State.Floating
        afterHit.Parts.[2].State |> should equal State.Floating
        afterHit.Parts.[3].State |> should equal State.Floating
        
    [<Fact>]
    let ``hitAtManyShips given coordinates and ships that does not contain this coordinate returns ships with parts unchanged`` () =
        let ship1 = createShip 'A'
        let ship2 = createShip 'B'
        let ships = [ship1;ship2]
        
        let hitCoord = battleSize |> Coordinate.create 'C' 6 |> Result.get
        
        let afterHit = Ship.hitAtManyShips hitCoord ships |> Seq.toList
        
        afterHit.[0].Parts.[0].State |> should equal State.Floating
        afterHit.[0].Parts.[1].State |> should equal State.Floating
        afterHit.[0].Parts.[2].State |> should equal State.Floating
        afterHit.[0].Parts.[3].State |> should equal State.Floating
        afterHit.[1].Parts.[0].State |> should equal State.Floating
        afterHit.[1].Parts.[1].State |> should equal State.Floating
        afterHit.[1].Parts.[2].State |> should equal State.Floating
        afterHit.[1].Parts.[3].State |> should equal State.Floating
        
    [<Fact>]
    let ``hitAtManyShips given coordinates and ships that does contain this coordinate returns ships with one having part sunk`` () =
        let ship1 = createShip 'A'
        let ship2 = createShip 'B'
        let ships = [ship1;ship2]
        
        let hitCoord = battleSize |> Coordinate.create 'B' 6 |> Result.get
        
        let afterHit = Ship.hitAtManyShips hitCoord ships |> Seq.toList
        
        afterHit.[0].Parts.[0].State |> should equal State.Floating
        afterHit.[0].Parts.[1].State |> should equal State.Floating
        afterHit.[0].Parts.[2].State |> should equal State.Floating
        afterHit.[0].Parts.[3].State |> should equal State.Floating
        afterHit.[1].Parts.[0].State |> should equal State.Floating
        afterHit.[1].Parts.[1].State |> should equal State.Sunk
        afterHit.[1].Parts.[2].State |> should equal State.Floating
        afterHit.[1].Parts.[3].State |> should equal State.Floating

