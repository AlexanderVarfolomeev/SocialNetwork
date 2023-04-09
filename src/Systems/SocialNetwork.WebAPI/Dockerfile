FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Systems/SocialNetwork.WebAPI/SocialNetwork.WebAPI.csproj", "src/Systems/SocialNetwork.WebAPI/"]
COPY ["src/Data/SocialNetwork.Context/SocialNetwork.Context.csproj", "src/Data/SocialNetwork.Context/"]
COPY ["src/Shared/SocialNetwork.Common/SocialNetwork.Common.csproj", "src/Shared/SocialNetwork.Common/"]
COPY ["src/Data/SocialNetwork.Entities/SocialNetwork.Entities.csproj", "src/Data/SocialNetwork.Entities/"]
COPY ["src/Data/SocialNetwork.Repository/SocialNetwork.Repository.csproj", "src/Data/SocialNetwork.Repository/"]
COPY ["src/Shared/SocialNetwork.Constants/SocialNetwork.Constants.csproj", "src/Shared/SocialNetwork.Constants/"]
COPY ["src/Services/SocialNetwork.AccountServices/SocialNetwork.AccountServices.csproj", "src/Services/SocialNetwork.AccountServices/"]
COPY ["src/Shared/SocialNetwork.Settings/SocialNetwork.Settings.csproj", "src/Shared/SocialNetwork.Settings/"]
COPY ["src/Services/SocialNetwork.EmailServices/SocialNetwork.EmailServices.csproj", "src/Services/SocialNetwork.EmailServices/"]
COPY ["src/Services/SocialNetwork.AttachmentServices/SocialNetwork.AttachmentServices.csproj", "src/Services/SocialNetwork.AttachmentServices/"]
COPY ["src/Services/SocialNetwork.CommentService/SocialNetwork.CommentService.csproj", "src/Services/SocialNetwork.CommentService/"]
COPY ["src/Services/SocialNetwork.ComplaintsServices/SocialNetwork.ComplaintsServices.csproj", "src/Services/SocialNetwork.ComplaintsServices/"]
COPY ["src/Services/SocialNetwork.GroupServices/SocialNetwork.GroupServices.csproj", "src/Services/SocialNetwork.GroupServices/"]
COPY ["src/Services/SocialNetwork.MessengerService/SocialNetwork.MessengerService.csproj", "src/Services/SocialNetwork.MessengerService/"]
COPY ["src/Services/SocialNetwork.RelationshipServices/SocialNetwork.RelationshipServices.csproj", "src/Services/SocialNetwork.RelationshipServices/"]
COPY ["src/Services/SocialNetwork.PostServices/SocialNetwork.PostServices.csproj", "src/Services/SocialNetwork.PostServices/"]
COPY ["src/Services/SocialNetwork.Cache/SocialNetwork.Cache.csproj", "src/Services/SocialNetwork.Cache/"]
RUN dotnet restore "src/Systems/SocialNetwork.WebAPI/SocialNetwork.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Systems/SocialNetwork.WebAPI"
RUN dotnet build "SocialNetwork.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SocialNetwork.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SocialNetwork.WebAPI.dll"]
