# PCI Platform — production container (backend + website + student/admin panels in one service)
#
# Build:  docker build -t pci-platform .
# Run:    docker run -p 8080:8080 -v pci-data:/data \
#           -e ASPNETCORE_ENVIRONMENT=Development \        # Development for a quick local look;
#           pci-platform                                   # Production requires the full env (see DEPLOY.md)
#
# The app validates its configuration on boot: in Production it REFUSES to start until
# APP_BASE_URL, ALLOWED_ORIGIN (and STRIPE_WEBHOOK_SECRET when Stripe is enabled) are set.
# That is deliberate — see DEPLOY.md for the exact variables.

# ---- Stage 1: build the React student portal (Stage 3) ----
# Produces /web/dist, which is dropped into the backend's wwwroot/app in the final image and
# served under /app/. Kept as its own stage so the Node toolchain never ships in the runtime image.
FROM node:22-slim AS webbuild
WORKDIR /web
COPY frontend/package.json frontend/package-lock.json ./
RUN npm ci
COPY frontend/ ./
RUN npm run build

# ---- Stage 2: build the .NET backend ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# restore first so dependency layers cache independently of source edits
COPY backend/PCI.Backend.csproj ./
RUN dotnet restore
COPY backend/ ./
RUN dotnet publish -c Release -o /app --no-restore

# ---- Stage 3: runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
# The compiled React portal is added on top of the published static site (wwwroot/app is gitignored,
# so it is never stale in source — it is assembled here from the freshly-built frontend).
COPY --from=webbuild /web/dist ./wwwroot/app
# /data is the single persistent mount: SQLite database + evidence/attachment files.
# Both paths are overridable; keeping them under one mount point suits hosts that
# attach exactly one disk per service (e.g. Render).
ENV DATABASE_FILE=/data/pci.db \
    STORAGE_ROOT=/data/storage \
    PORT=8080
VOLUME /data
EXPOSE 8080
ENTRYPOINT ["dotnet", "PCI.Backend.dll"]
