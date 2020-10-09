namespace Battleships.ConsoleApp.Tests

open Xunit
open System
open System.IO
open Battleships.ConsoleApp
open Battleships.Game
open FsCheck.Xunit
open Battleships.TestCommon
open FsUnit.Xunit

module MessageWriterTests =
    [<Fact>]
    let ``printRoundNumber prints round number`` () =
        async {
            let game = Game.start Settings.boardSize |> Result.get
            use output = new StringWriter()
            
            do! MessageWriter.printRoundNumber game output
            
            let expectedLine = sprintf "========== %d ==========" game.Hits.Length
            output.ToString() |> should equal (expectedLine+Environment.NewLine)
        }
        
    [<Fact>]
    let ``printGoodbye prints goodbye message`` () =
        async {
            let game = Game.start Settings.boardSize |> Result.get
            use output = new StringWriter()
            
            do! MessageWriter.printGoodbye game output
            
            let expectedLine = sprintf "It's done! It took you %d steps to finish your enemy." game.Hits.Length
            output.ToString() |> should equal (expectedLine+Environment.NewLine)
        }
        
    [<Fact>]
    let ``printShotResult when shot history is empty prints nothing`` () =
        async {
            let game = Game.start Settings.boardSize |> Result.get
            use output = new StringWriter()
            
            do! MessageWriter.printShotResult game output
            
            output.ToString() |> should equal ""
        }
        
    [<Fact>]
    let ``printShotResult when last shot was hit prints hit message with coords and ship type`` () =
        async {
            let game = Game.start Settings.boardSize |> Result.get
            let ship = game.Opponent.Ships.[0]
            let game = { game with Hits = [ HitResult.Hit (ship, 'A', 5) ; HitResult.Miss ('G', 1) ] }
            use output = new StringWriter()
            
            do! MessageWriter.printShotResult game output
            
            let expectedLine = sprintf "[A 5] HIT! %s" (ship.Kind.ToString())
            output.ToString() |> should equal (expectedLine+Environment.NewLine)
        }
        
    [<Fact>]
    let ``printShotResult when last shot was miss prints miss message with coords`` () =
        async {
            let game = Game.start Settings.boardSize |> Result.get
            let ship = game.Opponent.Ships.[0]
            let game = { game with Hits = [ HitResult.Miss ('G', 1) ; HitResult.Hit (ship, 'A', 5) ] }
            use output = new StringWriter()
            
            do! MessageWriter.printShotResult game output
            
            let expectedLine = sprintf "[G 1] MISS!" 
            output.ToString() |> should equal (expectedLine+Environment.NewLine)
        }
        
    [<Fact>]
    let ``printShotResult when last shot was sunk prints sunk message with coords and ship type`` () =
        async {
            let game = Game.start Settings.boardSize |> Result.get
            let ship = game.Opponent.Ships.[0]
            let game = { game with Hits = [ HitResult.Sunk (ship, 'G', 1) ; HitResult.Hit (ship, 'A', 5) ] }
            use output = new StringWriter()
            
            do! MessageWriter.printShotResult game output
            
            let expectedLine = sprintf "[G 1] SUNK! %s" (ship.Kind.ToString())
            output.ToString() |> should equal (expectedLine+Environment.NewLine)
        }