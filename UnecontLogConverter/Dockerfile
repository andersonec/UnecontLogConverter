FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
WORKDIR /app

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS runtime
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "UnecontLogConverter.dll"]
