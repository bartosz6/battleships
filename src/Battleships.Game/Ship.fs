namespace Battleships.Game

type ShipKind =
    | Battleship
    | Destroyer

type Ship = { Parts: Part list; Kind: ShipKind }

module Ship =
    let shipLengthOf shipKind =
        match shipKind with
        | ShipKind.Battleship -> 5
        | ShipKind.Destroyer -> 4
        
    let getKind ship = ship.Kind

    let getCoords ship = ship.Parts |> Part.getCoords

    let hit coords ship =
        let markHit part =
            match part with
            | part when part.Coordinate = coords -> { part with State = State.Sunk }
            | skip -> skip
        { ship with Parts = ship.Parts |> List.map markHit }

    let hitAtManyShips coords = Seq.map (hit coords)
    
    let isSunk ship = ship.Parts |> Seq.forall(fun part -> part.State = State.Sunk)

    let createShip coords =
        let checkInline coords =
            let areInTheSameColumn targetColumn =
                List.forall (fun coord -> coord.Column = targetColumn)

            let areInTheSameRow targetRow =
                List.forall (fun coord -> coord.Row = targetRow)
                
            let areInOrderForRow coords =
                let ordered = coords |> List.map (fun coord -> coord.Row) |> List.sort
                (ordered |> List.last) - ordered.Head = ordered.Length-1
                
            let areInOrderForColls coords =
                let ordered = coords |> List.map (fun coord -> coord.Column) |> List.sort
                (ordered |> List.last) - ordered.Head = ordered.Length-1
                
            match coords with
            | [] -> Error IncorrectNumberOfParts
            | coords when coords |> areInTheSameColumn coords.Head.Column && areInOrderForRow coords -> Ok coords
            | coords when coords |> areInTheSameRow coords.Head.Row && areInOrderForColls coords -> Ok coords
            | _ -> Error PartsNotInInlined

        let createNewPart coord = { Coordinate = coord; State = State.Floating }
        let createParts = List.map createNewPart

        let createShip parts =
            match parts with
            | list when list |> Seq.length = shipLengthOf Battleship -> { Parts = parts; Kind = Battleship } |> Ok
            | list when list |> Seq.length = shipLengthOf Destroyer -> { Parts = parts; Kind = Destroyer } |> Ok
            | _ -> Error IncorrectNumberOfParts

        checkInline coords
        |> Result.map createParts
        |> Result.bind createShip
