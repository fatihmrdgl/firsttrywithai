FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/backend/MimCrm.Api/MimCrm.Api.csproj MimCrm.Api/
RUN dotnet restore MimCrm.Api/MimCrm.Api.csproj
COPY src/backend/MimCrm.Api/ MimCrm.Api/
WORKDIR /src/MimCrm.Api
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "MimCrm.Api.dll"]
