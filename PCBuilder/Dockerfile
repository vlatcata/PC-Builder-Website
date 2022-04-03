#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PCBuilder/PCBuilder.csproj", "PCBuilder/"]
RUN dotnet restore "PCBuilder/PCBuilder.csproj"
COPY . .
WORKDIR "/src/PCBuilder"
RUN dotnet build "PCBuilder.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PCBuilder.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PCBuilder.dll"]