FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY TicketeraLab10.Api/TicketeraLab10.Api.csproj TicketeraLab10.Api/
COPY TicketeraLab10.Application/TicketeraLab10.Application.csproj TicketeraLab10.Application/
COPY TicketeraLab10.Domain/TicketeraLab10.Domain.csproj TicketeraLab10.Domain/
COPY TicketeraLab10.Infrastructure/TicketeraLab10.Infrastructure.csproj TicketeraLab10.Infrastructure/

RUN dotnet restore TicketeraLab10.Api/TicketeraLab10.Api.csproj

COPY . .
WORKDIR /src/TicketeraLab10.Api
RUN dotnet publish TicketeraLab10.Api.csproj --configuration Release --output /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000
ENTRYPOINT ["dotnet", "TicketeraLab10.Api.dll"]