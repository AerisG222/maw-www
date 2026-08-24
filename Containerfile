FROM mcr.microsoft.com/dotnet/sdk:10.0-noble-amd64 AS build
WORKDIR /maw-www

# bun, pinned, taken from the official image instead of piped from an installer
# script.  what was here before was apt (gawk, curl, unzip) purely to bootstrap
# two `curl | bash` installers, one for nvm and one for bun, each resolving
# "whatever is current today" on every cache miss - so the toolchain the image
# built with was not a property of this file.  the binary needs nothing the sdk
# image does not already carry, so copying it in is the whole install.
#
# no node: every client build command is a `bun run`, and bun executes the
# node_modules/.bin entry points on its own runtime rather than shelling out.
# /usr/local/bin is already on PATH, so nothing further is needed to expose it.
COPY --from=docker.io/oven/bun:1.4.0 /usr/local/bin/bun /usr/local/bin/bun

# restore
COPY Directory.Build.props .
COPY Directory.Packages.props .
COPY maw-www.slnx  .
COPY nuget.config  .
COPY src/MawWww/MawWww.csproj                 src/MawWww/
COPY src/MawWww.Blog/MawWww.Blog.csproj       src/MawWww.Blog/
COPY src/MawWww.Captcha/MawWww.Captcha.csproj src/MawWww.Captcha/
COPY src/MawWww.Email/MawWww.Email.csproj     src/MawWww.Email/
RUN dotnet restore --runtime linux-x64

# client restore, for the same reason the csproj copy above is separate from the
# source copy: `COPY src/. src/` changes on every source edit, and the csproj
# asset targets run `bun install` for MawWww and for each client app.  installing
# from the manifests alone keeps all of that in a layer that only a dependency
# change invalidates.  the later source copy merges over these node_modules
# rather than replacing them, since **/node_modules is in .dockerignore, so the
# csproj's own `bun install` then finds them in place and is a no-op.
COPY src/MawWww/package.json                  src/MawWww/bun.lockb                  src/MawWww/
COPY src/solid-binary-clock/package.json      src/solid-binary-clock/bun.lockb      src/solid-binary-clock/
COPY src/solid-google-maps/package.json       src/solid-google-maps/bun.lockb       src/solid-google-maps/
COPY src/solid-learning/package.json          src/solid-learning/bun.lockb          src/solid-learning/
COPY src/solid-memory/package.json            src/solid-memory/bun.lockb            src/solid-memory/
COPY src/solid-money-spin/package.json        src/solid-money-spin/bun.lockb        src/solid-money-spin/
COPY src/solid-weekend-countdown/package.json src/solid-weekend-countdown/bun.lockb src/solid-weekend-countdown/
COPY src/webgl-blender-model/package.json     src/webgl-blender-model/bun.lockb     src/webgl-blender-model/
COPY src/webgl-cube/package.json              src/webgl-cube/bun.lockb              src/webgl-cube/
COPY src/webgl-shader/package.json            src/webgl-shader/bun.lockb            src/webgl-shader/
COPY src/webgl-text/package.json              src/webgl-text/bun.lockb              src/webgl-text/
RUN set -e; for pkg in src/*/package.json; do (cd "$(dirname "${pkg}")" && bun install); done

# build (csproj builds client resources)
COPY src/. src/
RUN dotnet publish \
        --no-restore \
        --no-self-contained \
        -c Release \
        -r linux-x64 \
        -o /build \
        src/MawWww/MawWww.csproj


# build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled-extra-amd64
WORKDIR /maw-www

COPY --from=build /build .

EXPOSE 8080

ENTRYPOINT [ "/maw-www/MawWww" ]
