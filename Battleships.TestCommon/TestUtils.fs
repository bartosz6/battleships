namespace Battleships.TestCommon

open System

[<AutoOpen>]
module Result =
    let isOk result =
        match result with
        | Ok _ -> true
        | Error _ -> false

    let get result =
        match result with
        | Ok a -> a
        | Error error -> raise (InvalidOperationException(error.ToString()))

    let getError result =
        match result with
        | Ok a -> raise (InvalidOperationException(a.ToString()))
        | Error error -> error


        
