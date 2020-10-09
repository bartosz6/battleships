namespace Battleships.ConsoleApp.Tests

open System
open System.IO
open System.Text
open Battleships.ConsoleApp
open Xunit
open FsUnit.Xunit

module PlayerInputTests =
    
    [<Fact>]
    let ``readCoords given correct coords writes label and reads user input`` () =
        async {
            use input = new StringReader("A5")
            use output = new StringWriter()
            
            let! result = PlayerInput.readCoords input output
            
            output.ToString() |> should equal "shoot: "
            result |> should equal ('A', 5)
        }
    
    [<Fact>]
    let ``readCoords given incorrect coords writes label and retries until correct coords are given`` () =
        async {
            use input = new StringReader(StringBuilder().AppendLine("5A").AppendLine("A5").ToString())
            use output = new StringWriter()
            
            let! result = PlayerInput.readCoords input output
            
            let expectedOutput = "shoot: coordinates '5A' are invalid, use coordinates from 'A1' to 'J10'" + Environment.NewLine + "shoot: "
            output.ToString() |> should equal expectedOutput
            result |> should equal ('A', 5)
        }

