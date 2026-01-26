FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . ./
RUN dotnet publish src/futr/futr.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
COPY data /data
WORKDIR /app
COPY --from=build /app/out .
RUN apt-get update && apt-get install -y libfontconfig1
ENV ASPNETCORE_HTTP_PORTS=80
EXPOSE 80
ENTRYPOINT ["dotnet", "futr.dll"]
