FROM mcr.microsoft.com/dotnet/core/sdk:3.1

WORKDIR /dotnetapp

# copy project.json and restore as distinct layers
COPY netcore.csproj .
RUN dotnet restore

# copy and build everything else
COPY . .
RUN dotnet publish -c Release -o out
ENTRYPOINT ["dotnet", "out/netcore.dll"]