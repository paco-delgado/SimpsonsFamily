# SimpsonsFamily
- Web API in C# using .NET Core with the following methods:
  - **GET /people/{id}** - Returns the detailed information (name, lastname, birthdate) about a person.
  - **GET /people/{id}/family** - Returns the parents, siblings, partners and children of the given person
  - **POST /people/{id}/children** - Add a child to the given person. The person must already have a partner and the partner id must also be supplied. Return the id of the new child.
  - **GET /tree/{id}** - Returns the tree, in a valid json format, for the person {id}
- Swagger documentation
- Uses a Graph DB (Neo4j)
