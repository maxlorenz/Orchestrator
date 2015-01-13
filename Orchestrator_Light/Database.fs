module Database

open System

let DB = @"\\deployment\orchestrator\db.txt"
let onlyMyComputer (line: string) = line.StartsWith(Environment.MachineName)

let Remove (cmd: string) =
    let linesToWrite = IO.File.ReadAllLines(DB) |> Seq.filter(fun line -> not(String.Equals(line, cmd)))
    IO.File.WriteAllLines(DB, linesToWrite)

let GetMyCommands() = IO.File.ReadAllLines(DB) |> Seq.filter(onlyMyComputer)