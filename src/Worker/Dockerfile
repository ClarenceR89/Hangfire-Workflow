FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build
ARG BUILDMODE
WORKDIR /app
ADD . /app
RUN dotnet restore
RUN dotnet build --no-restore -c ${BUILDMODE}

FROM build AS publish
ARG BUILDMODE
RUN dotnet publish --no-build -c ${BUILDMODE} -o /app/out

FROM base AS final
WORKDIR /app/out
RUN ls -lah
COPY --from=publish /app/out .
ENTRYPOINT ["dotnet", "Worker.dll"]