namespace Battleships.ConsoleApp

open Battleships.Game
open System.IO

module ErrorWriter =
    let private createErrorMessage error =
        match error with
        | InputErrors inputError ->
            match inputError with
            | InvalidCoordinates usersInput -> sprintf "coordinates '%s' are invalid, use coordinates from 'A1' to '%c%d'" usersInput (Coordinate.rowNumberToChar Settings.boardSize) Settings.boardSize
        | ApplicationError appError ->
            match appError with
            | IncorrectNumberOfParts
            | PartsNotInInlined
            | ThereIsNoPlaceOnTheOcean -> "something went wrong ;)"
            | InvalidColumn col -> sprintf "invalid column '%d' minimum is '1' maximum is '%d'" col Settings.boardSize
            | InvalidRow row -> sprintf "invalid row '%c' minimum is 'A' maximum is '%c'" (Coordinate.rowNumberToChar row) (Coordinate.rowNumberToChar Settings.boardSize)
        
    let printError error (outputStream:TextWriter) = outputStream.WriteLineAsync(createErrorMessage error) |> Async.AwaitTask

