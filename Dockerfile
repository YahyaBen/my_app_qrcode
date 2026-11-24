FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Template.QrCode.Api/Template.QrCode.Api.csproj Template.QrCode.Api/
RUN dotnet restore Template.QrCode.Api/Template.QrCode.Api.csproj

COPY Template.QrCode.Api/ Template.QrCode.Api/
COPY docker/appsettings.json Template.QrCode.Api/appsettings.json
RUN dotnet publish Template.QrCode.Api/Template.QrCode.Api.csproj -c Release --no-restore -p:UseAppHost=false -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /publish/ ./
USER $APP_UID
ENTRYPOINT ["dotnet", "Template.QrCode.Api.dll"]
