# WeatherForecast
Песочно‑демонстрационное ASP.NET приложение прогноза погоды

# Используемые паттерны, концепции, инструменты:

## Архитектурные паттерны и концепции

### Чистая архитектура (Clean Architecture)
- Domain слой в центре, не зависящий от внешних слоев
- Application слой содержит бизнес-правила и use cases
- Infrastructure слой содержит реализации внешних зависимостей
- Web API слой как точка входа

### Многослойная архитектура (Layered Architecture)
- Разделение на слои: Domain, Application, Infrastructure, Web API
- Принцип разделения ответственности (Separation of Concerns)

### CQRS (Command Query Responsibility Segregation)
- Использование MediatR для разделения команд и запросов

### Паттерн Repository/Service
- Сервисы для работы с внешними API (WeatherApiComService)
- Абстракции через интерфейсы (IWeatherForecastService, ICacheService)

### Dependency Injection (Внедрение зависимостей)
- DI контейнер ASP.NET Core

### Паттерн Pipeline Behavior
- Использование MediatR Pipeline Behaviors для перехвата запросов

### Паттерн Mapper
- Использование AutoMapper для преобразования объектов

### Паттерн Decorator
- AuthTokenHandler как DelegatingHandler для добавления токенов к HTTP-запросам

### Кэширование с паттерном Cache-Aside
- RedisCacheService реализует стратегию "кэш-в-стороне"

## Технологии и фреймворки

### Бэкенд (ASP.NET Core)
- **.NET 8.0** - основная платформа
- **ASP.NET Core Web API** - для создания REST API
- **Entity Framework Core** - для работы с базой данных (в Identity сервисе)
- **Redis** - распределенное кэширование
- **MediatR** - реализация паттерна Mediator для CQRS
- **AutoMapper** - маппинг объектов
- **FluentValidation** - валидация запросов
- **Serilog** - структурированное логирование
- **Swagger/OpenAPI** - документация API
- **API Versioning** - версионирование API
- **Health Checks** - мониторинг здоровья сервисов

### Фронтенд (ASP.NET Core Razor Pages)
- **ASP.NET Core Razor Pages** - серверный рендеринг
- **Bootstrap 5** - CSS-фреймворк
- **jQuery** - JavaScript библиотека
- **jQuery Validation** - клиентская валидация
- **OpenID Connect** - аутентификация через IdentityServer

### Identity Service
- **IdentityServer4** - реализация OpenID Connect и OAuth 2.0
- **ASP.NET Core Identity** - управление пользователями и ролями
- **SQLite** - хранилище данных пользователей

### Инфраструктура и инструменты
- **Docker** - контейнеризация приложения
- **Docker Compose** - оркестрация многоконтейнерного приложения
- **Redis** (контейнер) - кэширование
