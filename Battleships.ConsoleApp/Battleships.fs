namespace Battleships.ConsoleApp

open System.IO
open Battleships.Game

module Battleships =
    let play (inputStream:TextReader) (outputStream:TextWriter) game =        
        let rec loop previousStep gameResult  =
            async {
                match gameResult |> Result.mapError Errors.ApplicationError with
                | Ok game when game.IsGameFinished |> not ->
                    do! MessageWriter.printRoundNumber game outputStream
                    do! MessageWriter.printShotResult game outputStream
                    
                    let! (row, col) = PlayerInput.readCoords inputStream outputStream
                    do! Settings.boardSize |> Game.hit row col game |> loop (Some gameResult)
                    
                | Ok game ->
                    do! MessageWriter.printRoundNumber game outputStream
                    do! MessageWriter.printShotResult game outputStream
                    
                    do! MessageWriter.printGoodbye game outputStream
                    
                | Error e when previousStep |> Option.isSome ->
                    do! ErrorWriter.printError e outputStream
                    
                    do! loop None previousStep.Value
                    
                | Error e ->
                    do! ErrorWriter.printError e outputStream
            }
        game |>  loop (None)            


