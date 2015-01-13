module orchestrator_light

open System
open System.Threading
open Command
open Database

// Helper functions
let execAndPrint (cmd: string) = Command.Exec "cmd" ("/C " + cmd) (fun data -> Console.WriteLine data.Data)
let removeIdentifier (line: string) = line.Remove(0, line.IndexOf(",") + 1)

[<EntryPoint>]
let main argv = 
    while (true) do
        for command in Database.GetMyCommands() do 
            execAndPrint (removeIdentifier command)
            Database.Remove command
        Thread.Sleep(3000)

    0 // return an integer exit code