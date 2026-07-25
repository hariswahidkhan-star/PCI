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

# ---- Stage 1: build the React apps (Stage 3) ----
# Produces /web/dist (student portal → /app/) and /web/dist-admin (admin console → /admin/), dropped
# into the backend's wwwroot in the final image. Kept as its own stage so the Node toolchain never
# ships in the runtime image. The admin entry is emitted as admin.html; it is placed as the /admin/
# index so UseDefaultFiles serves it (asset URLs are absolute /admin/… so the rename is safe).
FROM node:22-slim AS webbuild
WORKDIR /web
COPY frontend/package.json frontend/package-lock.json ./
RUN npm ci
COPY frontend/ ./
RUN npm run build && cp dist-admin/admin.html dist-admin/index.html

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
# The compiled React apps are added on top of the published static site (wwwroot/app and wwwroot/admin
# are gitignored, so never stale in source — assembled here from the freshly-built frontend).
COPY --from=webbuild /web/dist ./wwwroot/app
COPY --from=webbuild /web/dist-admin ./wwwroot/admin
# /data is the persistent mount for evidence/attachment files (and optional local SQLite).
# Production on Render uses MySQL (DB_PROVIDER=mysql in render.yaml) — DATABASE_FILE below is
# only adopted when the provider is SQLite/local; it is NOT a production MySQL fail-open.
# Both paths are overridable; one mount suits hosts that attach a single disk per service.
ENV DATABASE_FILE=/data/pci.db \
    STORAGE_ROOT=/data/storage \
    PORT=8080
VOLUME /data
EXPOSE 8080
ENTRYPOINT ["dotnet", "PCI.Backend.dll"]
