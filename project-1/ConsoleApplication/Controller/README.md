## Tasks of a Controller's command method handler
- Validate input at Controller level:
    - Does the ammount of arguments make sense?
- If arguments are good, then call the Service layer to let it handle the buisiness logic part of it.
- Return values:
    - 1: All OK, continue program loop.
    - 0: Exit program loop
## Rules
- Each file defines method handlers for the commands speciffic to one command context.
- ANY means no command context, its global. Commands that you want to run from within any context go there.
- ANY commands can set the command context at will
- For not ANY commands, the following appies:
    - Each command context group contains an entry point command: A command to be called if you want to enter that command context.
    - Each entry point command is to be treated as the home window of that command context.
    - If you want to move from one command context to another, you call the entry point command for that target command context.
    - Entry point commands are responsible for making sure that the user can enter that context based on the current state of the application, and also responsible for updating the context (in Session).
- Every context has ONE single entry point command.

### HOME
- REGISTER
- NOTES: calls NOTES:NOTES
- DELETEME
- LOGIN
- HOME: Entry point

### NOTES
- NOTES: Entry point
- NEW
- OPEN
- REMOVE

### ANY
- >
- <
- LOGOUT: calls HOME:HOME
- ERROR
- HELP
- EXIT
- HOME: calls HOME:HOME

