# PCI World — dedicated production container (serves PCI World EXCLUSIVELY)
#
# This image bakes in PCIWORLD_ONLY=true: the service serves /world*, /world-app*,
# /world-admin*, /world-admin-app* and their APIs only — the Institute website and portals
# are unreachable on it (pages 404, other APIs return 404).
#
# Unlike earlier revisions, this image DOES ship the World React SPAs (participant + admin
# share console). Skipping them left every /world-app route broken on a world-only host.
#
# Render setup (see PCIWorld/README.md for the click-by-click version):
#   New → Web Service → this repo → Language: Docker
#   Root Directory: (leave EMPTY)                  ← the build needs the whole repo as context
#   Dockerfile Path: PCIWorld/Dockerfile
#
# Local build (from the REPOSITORY ROOT — the context must include backend/ + frontend/):
#   docker build -f PCIWorld/Dockerfile -t pciworld .
#   docker run -p 8080:8080 -v pciworld-data:/data pciworld

# ---- Stage 1: build the World React SPAs ----
FROM node:22-slim AS webbuild
WORKDIR /web
COPY frontend/package.json frontend/package-lock.json ./
RUN npm ci
COPY frontend/ ./
RUN npm run build:world && npm run build:worldadmin \
 && cp dist-world/world.html dist-world/index.html \
 && cp dist-worldadmin/worldadmin.html dist-worldadmin/index.html

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
COPY --from=webbuild /web/dist-world ./wwwroot/world-app
COPY --from=webbuild /web/dist-worldadmin ./wwwroot/world-admin-app
# MySQL 8 is the production destination when MYSQL_HOST + MYSQL_PASSWORD (or
# MYSQL_CONNECTION_STRING) are actually set. Without complete credentials the entrypoint
# FORCES SQLite on /data — including when the dashboard still has DB_PROVIDER=mysql from an
# earlier attempt (the classic exit-78 loop). $$ escapes so values expand at *runtime*, not
# image-build time. DOTNET_* flags match the full-platform image (Render exit-139 / Starter GC).
ENV PCIWORLD_ONLY=true \
    ASPNETCORE_ENVIRONMENT=Production \
    STORAGE_ROOT=/data/storage \
    PORT=8080 \
    DOTNET_EnableWriteXorExecute=0 \
    DOTNET_gcServer=0
VOLUME /data
EXPOSE 8080
ENTRYPOINT ["/bin/sh","-c","mkdir -p /data /data/storage 2>/dev/null || true; \
  mysql_ready=0; \
  if [ -n \"$$MYSQL_CONNECTION_STRING\" ]; then mysql_ready=1; \
  elif [ -n \"$$MYSQL_HOST\" ] && [ -n \"$$MYSQL_PASSWORD\" ]; then mysql_ready=1; fi; \
  if [ \"$$mysql_ready\" = 1 ]; then \
    export DB_PROVIDER=\"$${DB_PROVIDER:-mysql}\"; \
  else \
    export DB_PROVIDER=sqlite \
           PCIWORLD_ALLOW_SQLITE=true \
           DATABASE_FILE=\"$${DATABASE_FILE:-/data/pciworld.db}\"; \
  fi; \
  exec dotnet PCI.Backend.dll"]
