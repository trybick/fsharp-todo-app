# Notes

To run F# scripts, use the following command:

```bash
dotnet fsi script.fsx
```

## User Stories

User can type todo in terminal and be greeted with a prompt to choose:

- Add Task. Then another prompt opens and they type the task. The task gets saved to local memory and added to the list of tasks.
- View Tasks. Will show a list of all tasks. User can scroll through them and mark completed them which will remove the task.

## Steps

- prompt to get user to choose one of two options
- do an action after a prompt choice is made
- add task: prompt for text input, take that input and save it
- view tasks: retrieve saved tasks and display them
- complete task: scroll through a list and do actions on each item
