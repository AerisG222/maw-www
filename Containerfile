FROM mcr.microsoft.com/dotnet/sdk:9.0-azurelinux3.0 AS build
WORKDIR /maw-www

RUN tdnf update -y
RUN tdnf install -y \
    awk \
    curl \
    unzip

# install nvm
RUN bash -c "touch ~/.bashrc"
RUN curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.40.1/install.sh | bash

# enable nvm and install lts version of node (currently v22)
RUN source ~/.bashrc && nvm install --lts

# install bun
RUN curl -fsSL https://bun.sh/install | bash

# restore
COPY maw-www.sln   .
COPY nuget.config  .
COPY src/MawWww/MawWww.csproj                 src/MawWww/
COPY src/MawWww.Blog/MawWww.Blog.csproj       src/MawWww.Blog/
COPY src/MawWww.Captcha/MawWww.Captcha.csproj src/MawWww.Captcha/
COPY src/MawWww.Email/MawWww.Email.csproj     src/MawWww.Email/
RUN dotnet restore --runtime linux-x64

# build (csproj builds client resources)
COPY src/. src/
RUN source ~/.bashrc && \
    dotnet publish \
        --no-restore \
        --no-self-contained \
        -c Release \
        -r linux-x64 \
        -o /build \
        src/MawWww/MawWww.csproj


# build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-azurelinux3.0-distroless-extra
WORKDIR /maw-www

COPY --from=build /build .

EXPOSE 8080

ENTRYPOINT [ "/maw-www/MawWww" ]
