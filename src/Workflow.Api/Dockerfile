#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS builder
ARG BUILDMODE
WORKDIR /build
COPY . .
RUN pwd && ls -lah
RUN dotnet restore
WORKDIR /build/src/Workflow.Api
RUN dotnet build --no-restore -c ${BUILDMODE}

FROM builder AS publish
ARG BUILDMODE
RUN dotnet publish --no-build -c ${BUILDMODE} -o /app

FROM base AS final
WORKDIR /app
RUN ls -lah
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Workflow.Api.dll"]
