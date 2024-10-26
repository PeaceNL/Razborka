# Укажите базовый образ с поддержкой .NET 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Установите рабочую директорию
WORKDIR /app

# Скопируйте файл проекта и восстановите зависимости
COPY *.csproj ./
RUN dotnet restore

# Скопируйте все файлы и соберите приложение
COPY . ./
RUN dotnet publish -c Release -o out

# Укажите образ для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Укажите команду для запуска приложения
ENTRYPOINT ["dotnet", "BackEndForRazborka.dll"]
