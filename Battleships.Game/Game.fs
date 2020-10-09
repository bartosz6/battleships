namespace Battleships.Game

type HitResult =
    | Hit of Ship * char * int
    | Miss of char * int
    | Sunk of Ship * char * int

type Game = {
    Opponent: Player
    Hits: HitResult list
    IsGameFinished: bool
}

module Game =
    let private PvE cpu = { Opponent = cpu; Hits = []; IsGameFinished = false }
        
    let start boardSize = Player.createCpu boardSize |> Result.map PvE
    
    let hit row column game boardSize =
        let hitResult ships =
            match ships |> Seq.except game.Opponent.Ships |> Seq.tryExactlyOne with
            | Some hit -> if Ship.isSunk hit then HitResult.Sunk (hit, row, column) else HitResult.Hit (hit, row, column)  
            | None -> HitResult.Miss (row, column) 
            
        let hit coordinate =
            let ships = Ship.hitAtManyShips coordinate game.Opponent.Ships
            {
              Opponent = { game.Opponent with Ships = ships |> Seq.toList }
              Hits = (ships |> hitResult) :: game.Hits 
              IsGameFinished = ships |> Seq.forall Ship.isSunk
            }
        
        Coordinate.create row column boardSize |> Result.map hit
        