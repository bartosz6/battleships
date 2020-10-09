namespace Battleships.ConsoleApp

open System
open Battleships.Game

module CoordinateParser =
    let parseUserInput userInput =
        let onlyLettersAndDigits = String.filter(fun c -> Char.IsDigit c || Char.IsLetter c)
        let toUpper (str: string) = str.ToUpper()
        
        match userInput |> onlyLettersAndDigits |> toUpper with
        | str when [2..3] |> Seq.contains str.Length && str.[0] |> Char.IsLetter ->
            let row = str.[0]
            let (isParsed, column) = Int32.TryParse(String(str.ToCharArray() |> Array.skip 1))
            if isParsed then Ok (row, column)
            else Error <| Errors.InputErrors (InvalidCoordinates userInput)
        | _ ->
            Error <| Errors.InputErrors (InvalidCoordinates userInput)

