FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

COPY ./Luciano.Serafim.Ebanx.Account.sln .
COPY ./Luciano.Serafim.Ebanx.Account.Api ./Luciano.Serafim.Ebanx.Account.Api
COPY ./Luciano.Serafim.Ebanx.Account.Bootstrap ./Luciano.Serafim.Ebanx.Account.Bootstrap
COPY ./Luciano.Serafim.Ebanx.Account.Core ./Luciano.Serafim.Ebanx.Account.Core
COPY ./Luciano.Serafim.Ebanx.Account.Infrastructure ./Luciano.Serafim.Ebanx.Account.Infrastructure
COPY ./Luciano.Serafim.Ebanx.Account.Tests ./Luciano.Serafim.Ebanx.Account.Tests

RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App

COPY --from=build-env /App/out .

ENV ASPNETCORE_URLS=http://*:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Luciano.Serafim.Ebanx.Account.Api.dll"]