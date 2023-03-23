#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Systems/SocialNetwork.IdentityServer/SocialNetwork.IdentityServer.csproj", "src/Systems/SocialNetwork.IdentityServer/"]
COPY ["src/Data/SocialNetwork.Context/SocialNetwork.Context.csproj", "src/Data/SocialNetwork.Context/"]
COPY ["src/Data/SocialNetwork.Entities/SocialNetwork.Entities.csproj", "src/Data/SocialNetwork.Entities/"]
COPY ["src/Shared/SocialNetwork.Common/SocialNetwork.Common.csproj", "src/Shared/SocialNetwork.Common/"]
COPY ["src/Shared/SocialNetwork.Settings/SocialNetwork.Settings.csproj", "src/Shared/SocialNetwork.Settings/"]
COPY ["src/Data/SocialNetwork.Repository/SocialNetwork.Repository.csproj", "src/Data/SocialNetwork.Repository/"]
COPY ["src/Shared/SocialNetwork.Constants/SocialNetwork.Constants.csproj", "src/Shared/SocialNetwork.Constants/"]
RUN dotnet restore "src/Systems/SocialNetwork.IdentityServer/SocialNetwork.IdentityServer.csproj"
COPY . .
WORKDIR "/src/src/Systems/SocialNetwork.IdentityServer"
RUN dotnet build "SocialNetwork.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SocialNetwork.IdentityServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SocialNetwork.IdentityServer.dll"]