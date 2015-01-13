module orchestrator_light

open System
open System.Threading
open Command
open Database

// Helper functions
let getFromCsv (csv: string) column = csv.Split(',').[column]
let removeIdentifier (line: string) = getFromCsv line 1

// Command related functions
let execAndPrint cmd = Command.ExecString cmd (fun data -> Console.WriteLine data.Data)
let execAndReport cmd = Command.ExecString cmd (fun data -> Database.Report data.Data)


// Execute and report procedures
let procedure line clientAction databaseAction = clientAction <| removeIdentifier line; databaseAction line
let executeProcedure line = procedure line execAndPrint Database.RemoveExecutedCommand
let reportProcedure line = procedure line execAndReport Database.RemoveReportedCommand

[<EntryPoint>]
let main argv =
    while (true) do
        Database.QueryExecuteCommands() |> Seq.iter(executeProcedure)
        Database.QueryReportCommands() |> Seq.iter(reportProcedure)
        Thread.Sleep(3000)

    0 // return an integer exit code