# mp-webapi
mp-webapi is the persistence and business logic web API for the Merlin: Plan application.

## MacOS Installation

### Requirements
* Postgres 9.5.x
* [Microsoft .Net Core](https://www.microsoft.com/net/core) (dotnet)

### Postgres Setup

#### Install
`brew install postgresql`

#### Start postgres
From the shell, type: `postgres`

#### Create the DB
From the shell, type: `createdb mpdata`

#### Create the DB User
From the shell, type: `psql mpdata`
This will launch the postgres client.  
Run these commands by copying and pasting into the client:  
`CREATE ROLE mpdbuser WITH LOGIN ENCRYPTED PASSWORD 'crooked-serf-radio' CREATEDB;`  
`GRANT ALL PRIVILEGES ON DATABASE mpdata TO mpdbuser;`  
`ALTER DATABASE mpdata OWNER TO mpdbuser;`  
type `\q` to quit the client

#### Database Fixtures
How fixtures will work is currently being worked out. In the future there will likely be a directory of fixture files that can be added when the application is first run. What fixture file will be used will be specified in `appsettings.json`.

### .Net Core Setup
Go [here](https://www.microsoft.com/net/core) to download and install .Net core

### Get the Code
Clone the repo at git@github.com:lemonadelabs/mp-webapi.git

### Install dependencies and perform migrations
Navigate to the `/src/MPWebAPI` directory inside the project.  
Run `dotnet restore` to download the project dependencies.  
If this is an update rather than a first time install, drop the database first before applying migrations with `dotnet ef database drop`  
Run `dotnet ef database update` to apply the current migrations.

### Run the application
Run `dotnet run` from inside the `/src/MPWebAPI` directory. If the project needs to compile it will do that before running. The Kestral server will start on `http://localhost:5000`

## Update procedure
1. Pull from the repo
2. From inside the /src/MPWebAPI folder, run `dotnet ef database update` (This is only necessary if the update has a new migration)
3. From inside the /src/MPWebAPI folder, run `dotnet run`

## API Documentation
[https://documenter.getpostman.com/collection/view/788789-ff23004d-9eee-01d3-fa28-e5110f8b2490](https://documenter.getpostman.com/collection/view/788789-ff23004d-9eee-01d3-fa28-e5110f8b2490K)






