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
#   docker run -p 8080:8080 -v pciworld-data:/data pciworld

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
# Zero-config boot: if a persistent disk is mounted at /data the database lives there; without
# one, /data is created inside the (ephemeral) container layer so the service still starts —
# data then resets on redeploy, which is exactly the preview posture. Explicit DATABASE_FILE /
# STORAGE_ROOT env vars always win.
ENV PCIWORLD_ONLY=true \
    PORT=8080
VOLUME /data
EXPOSE 8080
ENTRYPOINT ["/bin/sh","-c","mkdir -p /data 2>/dev/null || true; \
  if [ -d /data ] && [ -w /data ]; then export DATABASE_FILE=\"${DATABASE_FILE:-/data/pciworld.db}\" STORAGE_ROOT=\"${STORAGE_ROOT:-/data/storage}\"; \
  else export DATABASE_FILE=\"${DATABASE_FILE:-./pciworld.db}\" STORAGE_ROOT=\"${STORAGE_ROOT:-./storage}\"; fi; \
  exec dotnet PCI.Backend.dll"]
