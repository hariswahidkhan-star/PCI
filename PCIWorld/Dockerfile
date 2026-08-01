# PCI World — dedicated production container (serves PCI World EXCLUSIVELY)
#
# This image bakes in PCIWORLD_ONLY=true: the service serves /world*, /world-admin* and their
# APIs only — the Institute website and portals are unreachable on it (pages redirect to /world,
# other APIs return 404). Because those portals are blocked, the React SPA build stage is skipped
# entirely: this image is backend-only, so it builds several minutes faster and ships smaller
# than the full-platform image at the repository root.
#
# Render setup (see PCIWorld/README.md for the click-by-click version):
#   New → Web Service → this repo → Language: Docker
#   Root Directory: (leave EMPTY)                  ← the build needs the whole repo as context
#   Dockerfile Path: PCIWorld/Dockerfile
#
# Local build (from the REPOSITORY ROOT — the context must include backend/):
#   docker build -f PCIWorld/Dockerfile -t pciworld .
#   docker run -p 8080:8080 -v pciworld-data:/data \
#     -e MYSQL_HOST=host.docker.internal -e MYSQL_DATABASE=pciworld \
#     -e MYSQL_USER=pci -e MYSQL_PASSWORD=... pciworld

# ---- Stage 1: build the .NET backend ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# restore first so dependency layers cache independently of source edits
COPY backend/PCI.Backend.csproj ./
RUN dotnet restore
COPY backend/ ./
RUN dotnet publish -c Release -o /app --no-restore

# ---- Stage 2: runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
# MySQL 8 is the production destination. When MYSQL_* settings are present the entrypoint runs
# on them, fail-closed exactly as the platform validator demands. Without them, the image uses
# the validator's own deliberate bridge for this deployment shape — PCIWORLD_ALLOW_SQLITE with
# an absolute /data database path — so the documented zero-config preview still boots instead
# of exiting 78 on every deploy. The bridge is never silent: the boot log carries the
# world-only downgrade warnings, and the EPHEMERAL STORAGE banner when no real disk is mounted.
ENV PCIWORLD_ONLY=true \
    ASPNETCORE_ENVIRONMENT=Production \
    STORAGE_ROOT=/data/storage \
    PORT=8080
VOLUME /data
EXPOSE 8080
ENTRYPOINT ["/bin/sh","-c","mkdir -p /data 2>/dev/null || true; \
  if [ -n \"${MYSQL_HOST}${MYSQL_CONNECTION_STRING}\" ]; then \
    export DB_PROVIDER=\"${DB_PROVIDER:-mysql}\"; \
  else \
    export DB_PROVIDER=\"${DB_PROVIDER:-sqlite}\" \
           PCIWORLD_ALLOW_SQLITE=\"${PCIWORLD_ALLOW_SQLITE:-true}\" \
           DATABASE_FILE=\"${DATABASE_FILE:-/data/pciworld.db}\"; \
  fi; \
  exec dotnet PCI.Backend.dll"]
