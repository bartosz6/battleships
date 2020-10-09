
open System
open Battleships.ConsoleApp
open Battleships.Game

[<EntryPoint>]
let main _ =
    Game.start Settings.boardSize
    |> Battleships.play Console.In Console.Out
    |> Async.RunSynchronously
    0
