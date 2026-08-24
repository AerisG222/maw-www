FROM mcr.microsoft.com/dotnet/sdk:10.0-noble-amd64 AS build
WORKDIR /maw-www

RUN apt update
RUN apt install -y \
    gawk \
    curl \
    unzip

# install nvm
RUN bash -c "touch ~/.bashrc"
RUN curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.40.3/install.sh | bash

# expose NVM_DIR for later RUN steps and ensure it's consistent
ENV NVM_DIR=/root/.nvm

# enable nvm and install lts version of node (install via nvm so build commands that
# source nvm.sh can access node). We use bash -lc so nvm is available in this RUN.
RUN bash -lc "source ${NVM_DIR}/nvm.sh && nvm install --lts && nvm alias default node"

# install bun and add it to global PATH so subsequent RUNs (including non-login shells)
# can invoke `bun` without sourcing shell rc files. Also create a stable symlink.
RUN curl -fsSL https://bun.sh/install | bash && \
    ln -sf /root/.bun/bin/bun /usr/local/bin/bun

# Make bun available in all future layers via PATH
ENV BUN_INSTALL=/root/.bun
ENV PATH="$BUN_INSTALL/bin:$PATH"

# restore
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
RUN bash -c "source ~/.bashrc && \
    dotnet publish \
        --no-restore \
        --no-self-contained \
        -c Release \
        -r linux-x64 \
        -o /build \
        src/MawWww/MawWww.csproj"


# build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled-extra-amd64
WORKDIR /maw-www

COPY --from=build /build .

EXPOSE 8080

ENTRYPOINT [ "/maw-www/MawWww" ]
