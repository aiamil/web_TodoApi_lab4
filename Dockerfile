FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY *.csproj .
RUN dotnet restore

# Копируем всё остальное и собираем
COPY . .
RUN dotnet publish -c Release -o out

# Образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Запускаем приложение
ENTRYPOINT ["dotnet", "TodoApi.dll"]