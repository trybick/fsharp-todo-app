open System

let promptForAction (options: string[]) =
    let mutable actionIndex = 0
    let mutable isPromptOpen = true

    while isPromptOpen do
        Console.Clear()
        printfn "Use ↑ ↓ to choose, Enter to select:\n"

        for i = 0 to options.Length - 1 do
            if i = actionIndex then
                Console.ForegroundColor <- ConsoleColor.Green
                printfn "> %s" options.[i]
                Console.ResetColor()
            else
                printfn "  %s" options.[i]

        let keyPressed = Console.ReadKey true

        match keyPressed.Key with
        | ConsoleKey.UpArrow   -> if actionIndex > 0 then actionIndex <- actionIndex - 1
        | ConsoleKey.DownArrow -> if actionIndex < options.Length - 1 then actionIndex <- actionIndex + 1
        | ConsoleKey.Enter     -> isPromptOpen <- false
        | _ -> ()

    actionIndex

let mainActions: string[] = [| "Add Task"; "View Tasks" |]
let mainAction = promptForAction mainActions

match mainAction with
| 0 -> printfn "Hello!"
| 1 -> printfn "Goodbye!"
| _ -> ()
