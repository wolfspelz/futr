FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
COPY . ./
RUN dotnet publish src/futr/futr.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
COPY --from=build /app/out . 
RUN apt-get update && apt-get install -y libfontconfig1
EXPOSE 80
ENTRYPOINT ["dotnet", "futr.dll"]
