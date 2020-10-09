namespace Battleships.Game.Tests

module UtilsTests =
    open FsCheck.Xunit
    open Battleships.Game

    [<Property>]
    let ``shuffle given collection returns collection of the same length`` (seq: int list) =
        let shuffled = seq |> Seq.shuffle
        seq.Length =  shuffled.Length

    [<Property>]
    let ``shuffle given collection returns collection of the same items`` (seq: int list) =
        let shuffled = seq |> Seq.shuffle |> Seq.toList
        (seq |> List.sort) = (shuffled |> List.sort)
