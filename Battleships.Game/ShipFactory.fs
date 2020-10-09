namespace Battleships.Game

module ShipFactory =
    let createNewShipInFleet shipType oceanSize existingShips =
        let shipSize = Ship.shipLengthOf shipType
        
        let attachStatusToCoordinate coordinate =
            let isCoordinateOccupied =
                existingShips
                |> Seq.collect Ship.getCoords
                |> Seq.tryFind(fun coords -> coords = coordinate) |> Option.isSome
            (coordinate,isCoordinateOccupied)
            
        let grid = Array2D.init oceanSize oceanSize (fun i j ->
            attachStatusToCoordinate { Row = i+1; Column = j+1 })
        
        let row number = grid.[number, *]
        
        let column number = grid.[*, number]
        
        let onlyUnoccupied = Seq.filter (Seq.forall (fun (_,occupied) -> occupied |> not))
        
        let findShipCandidates = Seq.windowed shipSize >> onlyUnoccupied >> Seq.toList
        
        let buildShip = Seq.map(fun (coordinate, _) -> coordinate) >> Seq.toList >> Ship.createShip     

        let findByRows = [0..oceanSize-1] |> Seq.collect (fun i -> row i |> findShipCandidates)
        let findByCols = [0..oceanSize-1] |> Seq.collect (fun i -> column i |> findShipCandidates)
        
        let possibleShips = (Seq.append findByCols findByRows) |> Seq.shuffle
        
        match possibleShips |> Seq.tryHead with
        | Some shipParts -> buildShip shipParts
        | None -> Error Errors.ThereIsNoPlaceOnTheOcean