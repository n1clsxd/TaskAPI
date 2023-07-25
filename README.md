## TaskAPI

This simple minimal asp.net API provide a task creating, updating and check system.

It uses Entity Framework in memory.

### Endpoints and Operations

`POST /tasks` Create a new task using JSON payload.

`GET /tasks` Retrieves a list with all tasks.

`GET /tasks/completed` Retrives a list with completed tasks.

`GET /tasks/{id}` Retrives a specific task by it ID.

`PUT /tasks/{id}` Updates a specific task by it ID using JSON payload.

`DELETE /tasks/{id}` Deletes an specific task by it ID.
