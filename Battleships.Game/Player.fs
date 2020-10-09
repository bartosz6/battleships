namespace Battleships.Game

type Player = {
    Ships: Ship list
}

module Player =
    let createCpu battleFieldSize =
        let cpu = { Ships = [] }
        let generateShipForPlayer kind player =
            ShipFactory.createNewShipInFleet kind battleFieldSize player.Ships
            |> Result.map (fun newShip -> { player with Ships = newShip::player.Ships })
            
        generateShipForPlayer ShipKind.Battleship cpu
        |> Result.bind ( generateShipForPlayer ShipKind.Destroyer )
        |> Result.bind ( generateShipForPlayer ShipKind.Destroyer )
        
        
                    
                    
                
                