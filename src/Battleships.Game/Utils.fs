namespace Battleships.Game

open System.Security.Cryptography

module Seq =
    let shuffle collection =
        let copyOfArray = Array.copy (collection |> Seq.toArray)
        match copyOfArray with
        | [||] | [|_|] -> copyOfArray
        | _ ->
            let maxIndex = (copyOfArray.Length - 1)
            let randomSwap (arr:_[]) i =
                let index = RandomNumberGenerator.GetInt32(0,  maxIndex)
                let tmp = arr.[index]
                arr.[index] <- arr.[i]
                arr.[i] <- tmp
                arr
            [|0..maxIndex|] |> Array.fold randomSwap copyOfArray
                