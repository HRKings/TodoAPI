# TODO API

This repository contains a simple TODO API made using ASP.NET Core 5/.NET 5 and PostgreSQL.

# Running

Docker files and docker compose are provided. You should only need to run `docker-compose up -d` inside the root of the repository and a docker stack will be launched with a PostgreSQL database and an auto refreshing ASP.NET API with a swagger index. Everything is running under `https://localhost/7000`. The EF migrations are automatically ran when you start the docker stack.

## Changing ORMs

Two ORMs are provided (Dapper and Entity Framework Core), by default EF is used. To change, just go to the docker compose file and change the ORM environment variable to "Dapper" and rebuild the container. It should be using Dapper then.