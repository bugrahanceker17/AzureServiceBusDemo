FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AzureServiceBusDemo.ProducerApi/AzureServiceBusDemo.ProducerApi.csproj", "AzureServiceBusDemo.ProducerApi/"]
RUN dotnet restore "AzureServiceBusDemo.ProducerApi/AzureServiceBusDemo.ProducerApi.csproj"
COPY . .
WORKDIR "/src/AzureServiceBusDemo.ProducerApi"
RUN dotnet build "AzureServiceBusDemo.ProducerApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AzureServiceBusDemo.ProducerApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AzureServiceBusDemo.ProducerApi.dll"]
