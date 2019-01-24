# https://docs.docker.com/engine/examples/dotnetcore/#prerequisites
FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /src

COPY PeinearyDevelopment.sln .
COPY PeinearyDevelopment/PeinearyDevelopment.csproj /src/PeinearyDevelopment/
COPY DataAccess/DataAccess.csproj /src/DataAccess/
COPY DataAccess.Contracts/DataAccess.Contracts.csproj /src/DataAccess.Contracts/
COPY Contracts/Contracts.csproj /src/Contracts/
COPY DataAccess.GhostDb.Loader/DataAccess.GhostDb.Loader.csproj /src/DataAccess.GhostDb.Loader/

RUN dotnet restore ./PeinearyDevelopment.sln

COPY . ./
RUN dotnet publish -c debug -o ../../dist

FROM microsoft/aspnetcore:2.0
ENV ASPNETCORE_ENVIRONMENT DEVELOPMENT
WORKDIR /app
COPY --from=build-env dist .

CMD ["dotnet", "PeinearyDevelopment.dll"]
