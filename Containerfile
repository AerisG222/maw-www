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
COPY maw-www.slnx  .
COPY nuget.config  .
COPY src/MawWww/MawWww.csproj                 src/MawWww/
COPY src/MawWww.Blog/MawWww.Blog.csproj       src/MawWww.Blog/
COPY src/MawWww.Captcha/MawWww.Captcha.csproj src/MawWww.Captcha/
COPY src/MawWww.Email/MawWww.Email.csproj     src/MawWww.Email/
RUN dotnet restore --runtime linux-x64

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
