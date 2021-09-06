# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/aspnet:5.0
RUN apt-get update; apt-get install -y less; apt-get install -y curl;
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS="http://+:4000;http://+:3306";
EXPOSE 4000
EXPOSE 3306
COPY bin/Release/net5.0/publish/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "WebAPI.dll", "--server.urls", "http://+:4000"]