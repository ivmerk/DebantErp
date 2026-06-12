FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src
COPY DebantErp/DebantErp.csproj DebantErp/
RUN dotnet restore "DebantErp/DebantErp.csproj"
COPY DebantErp/ DebantErp/
RUN dotnet publish "DebantErp/DebantErp.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final
RUN apk add --no-cache wget
WORKDIR /app
# /app по умолчанию owned by root; чтобы Data Protection мог писать .data-protection/
# из-под `app` (uid 1654, уже есть в образе) — chown'им /app.
RUN chown app:app /app
USER app
COPY --from=build --chown=app:app /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
HEALTHCHECK --interval=30s --timeout=5s --retries=3 --start-period=30s \
  CMD wget -qO- http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "DebantErp.dll"]
