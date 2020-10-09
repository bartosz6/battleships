namespace Battleships.Game

type Column = int
type Row = int
type Coordinate = { Column: Column; Row: Row }

module Coordinate =
    let rowCharToNumber char = (char |> int) - 64
    let rowNumberToChar num = (num + 64) |> char

    let create row column battleSize =
        let createRow (row: char) =
            match rowCharToNumber row with
            | i when i > 0 && i <= battleSize -> Ok i
            | i -> InvalidRow i |> Error
        
        let createColumn column =
            match column with
            | i when i > 0 && i <= battleSize -> Ok i
            | i -> InvalidColumn i |> Error
            
        createColumn column |> Result.bind (fun column -> createRow row |> Result.map (fun row -> { Column = column; Row = row }))
        
        

