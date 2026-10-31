FROM mcr.microsoft.com/dotnet/sdk:10.0
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o /app/publish
EXPOSE 8080
ENTRYPOINT ["dotnet", "/app/publish/HttpServerDemo.dll"]

# docker build -t swenserver .