FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ../*.sln .
COPY *.csproj ./HotelWebApp/
RUN dotnet restore "HotelWebApp/HotelWebApp.csproj"
COPY . ./HotelWebApp/

WORKDIR /src/HotelWebApp
RUN dotnet publish -c Release -o /app --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
COPY --from=build /app ./
EXPOSE 80

ENTRYPOINT ["dotnet", "HotelWebApp.dll"]


#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER $APP_UID
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["HotelWebApp.csproj", "HotelWebApp/"]
#RUN dotnet restore "HotelWebApp/HotelWebApp.csproj"
#COPY . .
#WORKDIR "/src/HotelWebApp"
#RUN dotnet build "HotelWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "HotelWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "HotelWebApp.dll"]
