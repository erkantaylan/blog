FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "Directory.Packages.props"]
COPY ["Source/Blog.Web/Blog.Web.csproj", "Source/Blog.Web/"]
COPY ["Source/Core/Core.csproj", "Source/Core/"]
RUN dotnet restore "Source/Blog.Web/Blog.Web.csproj"
COPY . .
WORKDIR "/src/Source/Blog.Web"
RUN dotnet build "Blog.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Blog.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blog.Web.dll"]
