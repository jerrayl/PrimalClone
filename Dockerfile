FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS with-node

RUN curl -sL https://deb.nodesource.com/setup_20.x | bash -
RUN apt-get update && \
    apt-get install -y nodejs && \
    apt-get clean -y && \
    npm install -g npm

FROM with-node AS build

# Define build arguments for environment variables
ARG VITE_GOOGLE_CLIENT_ID
ENV VITE_GOOGLE_CLIENT_ID=$VITE_GOOGLE_CLIENT_ID

WORKDIR /src

COPY *.sln .
COPY Primal.Server/*.csproj Primal.Server/
COPY Primal.Tests/*.csproj Primal.Tests/
COPY Primal.Client/*.esproj Primal.Client/

RUN dotnet restore
COPY . .
WORKDIR "/src/Primal.Server"
RUN dotnet build "./Primal.Server.csproj"  -c Release

FROM build AS publish
RUN dotnet publish "./Primal.Server.csproj" -c Release --no-restore --no-build -o /app/publish

# Pull Request Stage for Testing
FROM build as test
ENTRYPOINT [ "dotnet", "test", ".", "-c", "Release", "--no-restore", "--no-build"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Primal.Server.dll"]
