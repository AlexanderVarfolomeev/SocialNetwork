FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Systems/SocialNetwork.Web/SocialNetwork.Web.csproj", "src/Systems/SocialNetwork.Web/SocialNetwork.Web.csproj"]
RUN dotnet restore "src/Systems/SocialNetwork.Web/SocialNetwork.Web.csproj"
COPY . .
WORKDIR "/src/src/Systems/SocialNetwork.Web"
RUN dotnet build "SocialNetwork.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SocialNetwork.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM nginx:alpine
WORKDIR /var/www/web

COPY --from=publish /app/publish/wwwroot .
COPY src/Systems/SocialNetwork.Web/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80