# Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Копируем файлы проектов
COPY Anserem.EmailProcessor.Console/*.csproj Anserem.EmailProcessor.Console/
COPY Anserem.EmailProcessor.Domain/*.csproj Anserem.EmailProcessor.Domain/
COPY Anserem.EmailProcessor.Tests/*.csproj Anserem.EmailProcessor.Tests/

# Восстанавливаем зависимости
RUN dotnet restore Anserem.EmailProcessor.Console/Anserem.EmailProcessor.Console.csproj

# Копируем все исходники
COPY . .

# Запускаем тесты
RUN dotnet test Anserem.EmailProcessor.Tests/Anserem.EmailProcessor.Tests.csproj --logger:trx --results-directory /testresults

# Публикуем приложение
RUN dotnet publish Anserem.EmailProcessor.Console/Anserem.EmailProcessor.Console.csproj -c Release -o /app/publish

# Финальный образ
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime
WORKDIR /app

# Копируем собранное приложение
COPY --from=build /app/publish .

# Копируем файл конфигурации
COPY Anserem.EmailProcessor.Console/appsettings.json .

# Запускаем приложение
ENTRYPOINT ["dotnet", "Anserem.EmailProcessor.Console.dll"]