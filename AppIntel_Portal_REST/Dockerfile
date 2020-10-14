FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["AppIntel_Portal_REST/AppIntel_Portal_REST.csproj", "AppIntel_Portal_REST/"]
RUN dotnet restore "AppIntel_Portal_REST/AppIntel_Portal_REST.csproj"
COPY . .
WORKDIR "/src/AppIntel_Portal_REST"
RUN dotnet build "AppIntel_Portal_REST.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AppIntel_Portal_REST.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AppIntel_Portal_REST.dll"]