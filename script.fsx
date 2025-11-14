#r "nuget: Microsoft.Data.Sqlite, 8.0.8"

open System
open Microsoft.Data.Sqlite

let promptForMainAction () : int =
    let mainActions: string[] = [| "Add Task"; "View Tasks"; "Exit" |]

    let mutable actionIndex = 0
    let mutable isOpen = true

    while isOpen do
        Console.Clear()
        printfn "Use ↑ ↓ to choose, Enter to select:\n"

        for i = 0 to mainActions.Length - 1 do
            if i = actionIndex then
                Console.ForegroundColor <- ConsoleColor.Green
                printfn "> %s" mainActions.[i]
                Console.ResetColor()
            else
                printfn "  %s" mainActions.[i]

        let keyPressed = Console.ReadKey true

        match keyPressed.Key with
        | ConsoleKey.UpArrow ->
            if actionIndex > 0 then
                actionIndex <- actionIndex - 1
        | ConsoleKey.DownArrow ->
            if actionIndex < mainActions.Length - 1 then
                actionIndex <- actionIndex + 1
        | ConsoleKey.Enter -> isOpen <- false
        | _ -> ()

    actionIndex

let promptForAddTask () =
    Console.Clear()

    let mutable input = ""
    let mutable isOpen = true

    while isOpen do
        let key = Console.ReadKey true

        match key.Key with
        | ConsoleKey.Enter -> isOpen <- false
        | ConsoleKey.Escape ->
            input <- ""
            isOpen <- false
        | _ ->
            Console.Write key.KeyChar
            input <- input + string key.KeyChar

    Console.WriteLine()
    input

let ensureTableExists () =
    use conn = new SqliteConnection "Data Source=tasks.db"
    conn.Open()
    use cmd = conn.CreateCommand()
    cmd.CommandText <- "CREATE TABLE IF NOT EXISTS tasks (id INTEGER PRIMARY KEY, text TEXT)"
    cmd.ExecuteNonQuery() |> ignore

let saveTask (task: string) =
    if String.IsNullOrWhiteSpace task then
        ()
    else
        use conn = new SqliteConnection "Data Source=tasks.db"
        conn.Open()

        use cmd = conn.CreateCommand()
        cmd.CommandText <- "CREATE TABLE IF NOT EXISTS tasks (id INTEGER PRIMARY KEY, text TEXT)"
        cmd.ExecuteNonQuery() |> ignore

        cmd.CommandText <- "INSERT INTO tasks (text) VALUES ($text)"
        cmd.Parameters.AddWithValue("$text", task) |> ignore
        cmd.ExecuteNonQuery() |> ignore

let deleteTask (id: int) =
    use conn = new SqliteConnection "Data Source=tasks.db"
    conn.Open()
    use cmd = conn.CreateCommand()
    cmd.CommandText <- "DELETE FROM tasks WHERE id = $id"
    cmd.Parameters.AddWithValue("$id", id) |> ignore
    cmd.ExecuteNonQuery() |> ignore

let getSavedTasks () : (int * string) list =
    use conn = new SqliteConnection "Data Source=tasks.db"
    conn.Open()
    use cmd = conn.CreateCommand()
    cmd.CommandText <- "SELECT id, text FROM tasks"
    use reader = cmd.ExecuteReader()

    [ while reader.Read() do
          yield reader.GetInt32 0, reader.GetString 1 ]


let printTasks (tasks: (int * string) list) =
    if tasks.IsEmpty then
        Console.Clear()
        printfn "No tasks found."
        printfn "\nPress any key to return..."
        Console.ReadKey true |> ignore
    else
        let mutable currentTasks = tasks
        let mutable selectedIndex = 0
        let mutable isOpen = true

        while isOpen do
            Console.Clear()
            printfn "Tasks (Enter = delete, Esc = exit):\n"

            for i = 0 to currentTasks.Length - 1 do
                let _, text = currentTasks.[i]

                if i = selectedIndex then
                    Console.ForegroundColor <- ConsoleColor.Green
                    printfn "> %s" text
                    Console.ResetColor()
                else
                    printfn "  %s" text

            let key = Console.ReadKey true

            match key.Key with
            | ConsoleKey.UpArrow ->
                if selectedIndex > 0 then
                    selectedIndex <- selectedIndex - 1

            | ConsoleKey.DownArrow ->
                if selectedIndex < currentTasks.Length - 1 then
                    selectedIndex <- selectedIndex + 1

            | ConsoleKey.Enter ->
                let id, _ = currentTasks.[selectedIndex]
                deleteTask id
                currentTasks <- getSavedTasks ()

                if currentTasks.IsEmpty then
                    isOpen <- false
                else
                    selectedIndex <- min selectedIndex (currentTasks.Length - 1)

            | ConsoleKey.Escape -> isOpen <- false

            | _ -> ()

let main _ =
    ensureTableExists ()
    let mutable isRunning = true

    while isRunning do
        match promptForMainAction () with
        | 0 ->
            let task = promptForAddTask ()
            saveTask task
        | 1 ->
            let tasks = getSavedTasks ()
            printTasks tasks
        | 2 -> isRunning <- false
        | _ -> ()

    0

main [||]
