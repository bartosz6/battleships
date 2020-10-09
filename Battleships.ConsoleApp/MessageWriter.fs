namespace Battleships.ConsoleApp

open System.IO
open Battleships.Game

module MessageWriter =
    let printRoundNumber game (outputStream: TextWriter) =
        outputStream.WriteLineAsync(sprintf "========== %d ==========" game.Hits.Length) |> Async.AwaitTask
    
    let printGoodbye game (outputStream: TextWriter) =
        outputStream.WriteLineAsync(sprintf "It's done! It took you %d steps to finish your enemy." game.Hits.Length) |> Async.AwaitTask

    let printShotResult game (outputStream: TextWriter) =
        match game.Hits with
        | [] -> async { return () }
        | lastShot::_ ->
            match lastShot with
            | HitResult.Hit (ship, row, col) ->
                outputStream.WriteLineAsync(sprintf "[%c %d] HIT! %s" row col (ship.Kind |> string)) |> Async.AwaitTask
            | HitResult.Sunk (ship, row, col) ->
                outputStream.WriteLineAsync(sprintf "[%c %d] SUNK! %s" row col (ship.Kind |> string)) |> Async.AwaitTask
            | HitResult.Miss (row, col) ->
                outputStream.WriteLineAsync(sprintf "[%c %d] MISS!" row col) |> Async.AwaitTask



