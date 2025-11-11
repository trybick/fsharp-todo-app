open System

let promptForMainAction () : int =
    let mainActions: string[] = [| "Add Task"; "View Tasks" |]

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

let mainAction = promptForMainAction ()

match mainAction with
| 0 ->
    let task = promptForAddTask ()
    // saveTask

| 1 -> printfn "Goodbye!"
| _ -> ()
