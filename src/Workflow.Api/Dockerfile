FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY Workflow.Api/Workflow.Api.csproj Workflow.Api/
RUN dotnet restore "Workflow.Api/Workflow.Api.csproj"
COPY . .
WORKDIR "/src/Workflow.Api"
RUN dotnet build "Workflow.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Workflow.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Workflow.Api.dll"]
