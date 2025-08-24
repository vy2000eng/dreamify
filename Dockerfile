FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7235
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files for restore (assuming they're all in the root level)
COPY ["dreamify.API/dreamify.API.csproj", "dreamify.API/"]
COPY ["dreamify.Application/dreamify.Application.csproj", "dreamify.Application/"]
COPY ["dreamify.Domain/dreamify.Domain.csproj", "dreamify.Domain/"]
COPY ["dreamify.Infrastructure/dreamify.Infrastructure.csproj", "dreamify.Infrastructure/"]

# Restore dependencies starting from the API project
RUN dotnet restore "dreamify.API/dreamify.API.csproj"

# Copy all source code
COPY . .

# Build the API project
WORKDIR "/src/dreamify.API"
RUN dotnet build "dreamify.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "dreamify.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dreamify.API.dll"]