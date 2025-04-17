FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["IdentityService.csproj", "."]
RUN dotnet restore "IdentityService.csproj"
COPY . .
RUN dotnet publish "IdentityService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:7051
EXPOSE 7051
ENTRYPOINT ["dotnet", "IdentityService.dll"]