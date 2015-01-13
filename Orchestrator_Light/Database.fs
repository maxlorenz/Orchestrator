module Database

open System

let COMMANDS_DB = @"\\deployment\orchestrator\install.txt"
let REPORTING_DB = @"\\deployment\orchestrator\report.txt"

// Timestamp
let timestamp() = DateTime.Now.ToShortTimeString()
let makeReport name data = sprintf "%s : %s, %s" (timestamp()) name data

// Text manipulation
let NotEqual a b = not <| String.Equals(a, b)

// File operations
let ReadLines file = IO.File.ReadAllLines(file)
let WriteLines file lines = IO.File.WriteAllLines(file, Seq.toArray lines)
let AppendLine file line = IO.File.AppendAllText(file, line)

// Sequence operations
let onlyMyComputer (line: string) = line.StartsWith(Environment.MachineName)
let removeFromDB line file = ReadLines file |> Seq.filter(NotEqual line) |> WriteLines file
let getCommandsFromDB file = ReadLines file |> Seq.filter(onlyMyComputer)

// High level reporting
let Report (data: string) =
    if NotEqual data "" && NotEqual data null then
        AppendLine REPORTING_DB (makeReport Environment.MachineName data)

let RemoveExecutedCommand cmd = removeFromDB cmd COMMANDS_DB
let RemoveReportedCommand cmd = removeFromDB cmd REPORTING_DB

// Database querys
let QueryExecuteCommands() = getCommandsFromDB COMMANDS_DB
let QueryReportCommands() = getCommandsFromDB REPORTING_DB