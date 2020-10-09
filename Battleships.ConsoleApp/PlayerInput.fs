namespace Battleships.ConsoleApp

open System.IO

module PlayerInput =
    let rec readCoords (inputStream:TextReader) (outputStream:TextWriter) =
        async {
            do! outputStream.WriteAsync("shoot: ") |> Async.AwaitTask
            
            let! userInput = inputStream.ReadLineAsync () |> Async.AwaitTask
            
            match CoordinateParser.parseUserInput userInput with
            | Ok coordinates -> return coordinates
            | Error error ->
                do! ErrorWriter.printError error outputStream
                return! readCoords inputStream outputStream
        }