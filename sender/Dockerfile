# =======================================================
# Stage 1 - Build/compile app using container
# =======================================================

ARG IMAGE_BASE=5.0-alpine

# Build image has SDK and tools (Linux)
FROM mcr.microsoft.com/dotnet/sdk:$IMAGE_BASE as build
WORKDIR /build

# Copy project source files
COPY . .

# Restore, build & publish
WORKDIR /build
RUN dotnet restore
RUN dotnet publish --no-restore --configuration Release

# =======================================================
# Stage 2 - Assemble runtime image from previous stage
# =======================================================

# Base image is .NET Core runtime only (Linux)
FROM mcr.microsoft.com/dotnet/aspnet:$IMAGE_BASE

# Seems as good a place as any
WORKDIR /app

# Copy already published binaries (from build stage image)
COPY --from=build /build/bin/Release/net5.0/publish/ .

# Run the ASP.NET Core app
ENTRYPOINT dotnet sender.dll