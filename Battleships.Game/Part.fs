namespace Battleships.Game

type State =
    | Floating
    | Sunk

type Part = {
    Coordinate: Coordinate
    State: State
    }

module Part =
    let getCoords = List.map (fun part -> part.Coordinate)

