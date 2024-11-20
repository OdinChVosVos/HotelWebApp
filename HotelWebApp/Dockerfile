# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["HotelWebApp.csproj", "HotelWebApp/"]
RUN dotnet restore "HotelWebApp/HotelWebApp.csproj"

# Copy the rest of the application code
COPY . ./HotelWebApp/

# Set the working directory to the project folder
WORKDIR "/src/HotelWebApp"

# Build the application
RUN dotnet build "HotelWebApp.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "HotelWebApp.csproj" -c Release -o /app/publish --no-restore

# Final stage to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "HotelWebApp.dll"]