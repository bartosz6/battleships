namespace Battleships.ConsoleApp.Tests

open System
open System.IO
open Battleships.ConsoleApp
open Battleships.Game
open FsCheck.Xunit
open Xunit
open FsUnit.Xunit

module ErrorWriterTests =
            
    [<Property>]
    let ``printError handles every error`` error =
        use stream = new StringWriter()
        
        ErrorWriter.printError error stream |> Async.RunSynchronously
        
        let printed = stream.ToString()
        printed.Length > 0
        
    [<Fact>]
    let ``given IncorrectNumberOfParts error writes generic error message`` () =
        use stream = new StringWriter()
        let inputError = Errors.IncorrectNumberOfParts
        
        ErrorWriter.printError (Errors.ApplicationError inputError) stream |> Async.RunSynchronously
        
        stream.ToString() |> should equal ("something went wrong ;)"+Environment.NewLine)
        
    [<Fact>]
    let ``given PartsNotInInlined error writes generic error message`` () =
        use stream = new StringWriter()
        let inputError = Errors.PartsNotInInlined
        
        ErrorWriter.printError (Errors.ApplicationError inputError) stream |> Async.RunSynchronously
        
        stream.ToString() |> should equal ("something went wrong ;)"+Environment.NewLine)
        
    [<Fact>]
    let ``given ThereIsNoPlaceOnTheOcean error writes generic error message`` () =
        use stream = new StringWriter()
        let inputError = Errors.ThereIsNoPlaceOnTheOcean
        
        ErrorWriter.printError (Errors.ApplicationError inputError) stream |> Async.RunSynchronously
        
        stream.ToString() |> should equal ("something went wrong ;)"+Environment.NewLine)
        
    [<Fact>]
    let ``given InvalidColumn error writes invalid column message`` () =
        use stream = new StringWriter()
        let inputError = Errors.InvalidColumn 3
        
        ErrorWriter.printError (Errors.ApplicationError inputError) stream |> Async.RunSynchronously
        
        stream.ToString() |> should equal ("invalid column '3' minimum is '1' maximum is '10'"+Environment.NewLine)
        
    [<Fact>]
    let ``given InvalidRow error writes invalid row message`` () =
        use stream = new StringWriter()
        let inputError = Errors.InvalidRow 3
        
        ErrorWriter.printError (Errors.ApplicationError inputError) stream |> Async.RunSynchronously
        
        stream.ToString() |> should equal ("invalid row 'C' minimum is 'A' maximum is 'J'"+Environment.NewLine)
        
    [<Fact>]
    let ``given invalid coordinates error writes coordinates are invalid message`` () =
        use stream = new StringWriter()
        let inputError = InputErrors.InvalidCoordinates "wolololo"
        
        ErrorWriter.printError (Errors.InputErrors inputError) stream |> Async.RunSynchronously
        
        stream.ToString() |> should equal ("coordinates 'wolololo' are invalid, use coordinates from 'A1' to 'J10'"+Environment.NewLine)

        

