namespace Battleships.ConsoleApp

type InputErrors =
    | InvalidCoordinates of string

type Errors =
    | ApplicationError of Battleships.Game.Errors
    | InputErrors of InputErrors 
