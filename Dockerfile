FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GeniaWebApp.csproj", "./"]
RUN dotnet restore "GeniaWebApp.csproj"
COPY . .
WORKDIR "/src/"

ENV ASPNETCORE_URLS=http://+:5000

RUN dotnet build "GeniaWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GeniaWebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false


EXPOSE 80:5000

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GeniaWebApp.dll"]
