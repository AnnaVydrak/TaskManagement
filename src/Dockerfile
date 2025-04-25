FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY . .
RUN dotnet restore "TaskManagement.csproj"
COPY . .
RUN dotnet publish "TaskManagement.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
COPY . .                        
EXPOSE 5007
ENTRYPOINT ["dotnet", "TaskManagement.dll"]