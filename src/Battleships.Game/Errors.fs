namespace Battleships.Game

type Errors =
    | IncorrectNumberOfParts
    | PartsNotInInlined
    | ThereIsNoPlaceOnTheOcean
    | InvalidColumn of int
    | InvalidRow of int

