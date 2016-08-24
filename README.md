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
From the shell, type: `psql`  
This will launch the postgres client.  
Run these commands by copying and pasting into the client:  
`CREATE ROLE mpdbuser WITH LOGIN ENCRYPTED PASSWORD 'crooked-serf-radio' CREATEDB;`  
`GRANT ALL PRIVILEGES ON DATABASE mpdata TO mpdbuser;`  
type `\q` to quit the client

### .Net Core Setup
Go [here](https://www.microsoft.com/net/core) to download and install .Net core

### Get the Code
Clone the repo at git@github.com:lemonadelabs/mp-webapi.git

### Install dependencies and perform migrations
Navigate to the `/src/MPWebAPI` directory inside the project.  
Run `dotnet restore` to download the project dependencies.  
Run `dotnet ef database update` to apply the current migrations.

### Run the application
Run `dotnet run` from inside the `/src/MPWebAPI` directory. If the project needs to compile it will do that before running. The Kestral server will start on `http://localhost:5000`

### View the api docs
With the application running direct your browser to `http://localhost:5000/swagger/ui`

## Update procedure
1. Pull from the repo
2. From inside the /src/MPWebAPI folder, run `dotnet ef database update` (This is only necessary if the update has a new migration)
3. From inside the /src/MPWebAPI folder, run `dotnet run`






