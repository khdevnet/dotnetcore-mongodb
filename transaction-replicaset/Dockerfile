FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Books.WebApi/Books.WebApi.csproj", "Books.WebApi/"]
RUN dotnet restore "Books.WebApi/Books.WebApi.csproj"
COPY . .
WORKDIR "/src/Books.WebApi"
RUN dotnet build "Books.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Books.WebApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Books.WebApi.dll"]