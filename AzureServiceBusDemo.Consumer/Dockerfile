FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AzureServiceBusDemo.Consumer/AzureServiceBusDemo.Consumer.csproj", "AzureServiceBusDemo.Consumer/"]
RUN dotnet restore "AzureServiceBusDemo.Consumer/AzureServiceBusDemo.Consumer.csproj"
COPY . .
WORKDIR "/src/AzureServiceBusDemo.Consumer"
RUN dotnet build "AzureServiceBusDemo.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AzureServiceBusDemo.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AzureServiceBusDemo.Consumer.dll"]
