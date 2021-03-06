# Sloflix

A place for you and your friends to create individual movie watchlists
and keep track of which movies you've watched and to rate the movies you've
already seen.

## Dependencies

The ASP.NET Core 2.2 is required for this project to run.
Nodejs is also required in order to build this project

## Starting the server

Currently, you must launch the angular server and the asp server seperately.

### Angular SPA Server

First, start the angular server by running the following two commands
to start the angular SPA server

```sh
cd ClientApp
npm start
```

### ASP.NET Server

Then, in a seperate terminal, run

```sh
dotnet run
```

to start the ASP.NET server

## Deployment

The best current deployment method is to use the `dotnet publish` command.

```sh
dotnet publish -c Release -o build
```

Then to start the production server run:

```sh
cd build
dotnet sloflix.dll
```
