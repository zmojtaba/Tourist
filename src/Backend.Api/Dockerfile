FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        -o Acquire::Retries=5 \
        -o Acquire::http::Timeout=120 \
        -o Acquire::https::Timeout=120 \
        curl \
        ffmpeg \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

WORKDIR /app

EXPOSE 5001

ENV ASPNETCORE_URLS=http://+:5001


# ============================================================
# Build image
# ============================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Debug

WORKDIR /src

# Copy project files first.
# This allows Docker to cache the restore layer.
COPY ["Backend.Api/Backend.Api.csproj", "Backend.Api/"]
COPY ["Backend.Application/Backend.Application.csproj", "Backend.Application/"]
COPY ["Backend.Domain/Backend.Domain.csproj", "Backend.Domain/"]
COPY ["Backend.Infrustructure/Backend.Infrustructure.csproj", "Backend.Infrustructure/"]

# Restore the API project.
# Project references will cause the other projects to be restored too.
RUN dotnet restore "Backend.Api/Backend.Api.csproj"

# Copy the complete source tree.
COPY . .

WORKDIR "/src/Backend.Api"

# Build the API.
RUN dotnet build "Backend.Api.csproj" \
    -c $BUILD_CONFIGURATION \
    --no-restore \
    -o /app/build


# ============================================================
# Publish
# ============================================================
FROM build AS publish

ARG BUILD_CONFIGURATION=Debug

RUN dotnet publish "Backend.Api.csproj" \
    -c $BUILD_CONFIGURATION \
    --no-restore \
    -o /app/publish \
    /p:UseAppHost=false


# ============================================================
# Final image
# ============================================================
FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Backend.Api.dll"]