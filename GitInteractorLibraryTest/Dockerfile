FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GitInteractorLibraryTest/GitInteractorLibraryTest.csproj", "GitInteractorLibraryTest/"]
RUN dotnet restore "GitInteractorLibraryTest/GitInteractorLibraryTest.csproj"
COPY . .
WORKDIR "/src/GitInteractorLibraryTest"
RUN dotnet build "GitInteractorLibraryTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GitInteractorLibraryTest.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GitInteractorLibraryTest.dll"]
