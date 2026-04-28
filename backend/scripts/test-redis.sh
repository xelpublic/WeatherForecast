#!/bin/bash

# Bash скрипт для проверки Redis (Linux/Mac)

echo "=== Проверка Redis ==="

# Проверка, запущен ли Docker
if command -v docker &> /dev/null; then
    echo "✓ Docker доступен"
    DOCKER_RUNNING=true
else
    echo "✗ Docker не доступен"
    DOCKER_RUNNING=false
fi

# Проверка порта Redis
echo -e "\nПроверка порта 6379..."
if nc -z localhost 6379 2>/dev/null; then
    echo "✓ Порт 6379 открыт"
    
    # Попытка подключиться через redis-cli
    echo -e "\nПроверка подключения Redis..."
    if command -v redis-cli &> /dev/null; then
        if redis-cli -h localhost -p 6379 ping 2>/dev/null | grep -q "PONG"; then
            echo "✓ Redis отвечает (PONG)"
        else
            echo "✗ Redis не отвечает правильно"
        fi
    else
        echo "ℹ redis-cli не установлен"
        # Альтернативная проверка
        echo "*" | timeout 2 telnet localhost 6379 2>&1 | grep -q "Connected" && \
            echo "✓ Подключение к Redis установлено" || \
            echo "✗ Не удалось подключиться к Redis"
    fi
else
    echo "✗ Порт 6379 закрыт"
    
    # Проверка Docker контейнеров
    if [ "$DOCKER_RUNNING" = true ]; then
        echo -e "\nПроверка Docker контейнеров Redis..."
        REDIS_CONTAINER=$(docker ps --filter "name=redis" --format "{{.Names}}" 2>/dev/null)
        if [ -n "$REDIS_CONTAINER" ]; then
            echo "✓ Найден контейнер Redis: $REDIS_CONTAINER"
            echo "  Запущен, но порт может не быть проброшен"
        else
            echo "✗ Контейнер Redis не найден"
        fi
    fi
fi

# Проверка строки подключения в appsettings
echo -e "\nПроверка конфигурации приложения..."
APPSETTINGS_PATH="Wf.WebApi/appsettings.json"
APPSETTINGS_DEV_PATH="Wf.WebApi/appsettings.Development.json"

if [ -f "$APPSETTINGS_PATH" ]; then
    REDIS_CONNECTION=$(grep -A1 -B1 '"Redis"' "$APPSETTINGS_PATH" | grep ':' | cut -d'"' -f4 | tail -1)
    if [ -n "$REDIS_CONNECTION" ]; then
        echo "✓ Найдена строка подключения Redis в appsettings.json"
        echo "  $REDIS_CONNECTION"
    fi
fi

if [ -f "$APPSETTINGS_DEV_PATH" ]; then
    REDIS_CONNECTION_DEV=$(grep -A1 -B1 '"Redis"' "$APPSETTINGS_DEV_PATH" | grep ':' | cut -d'"' -f4 | tail -1)
    if [ -n "$REDIS_CONNECTION_DEV" ]; then
        echo "✓ Найдена строка подключения Redis в appsettings.Development.json"
        echo "  $REDIS_CONNECTION_DEV"
    fi
fi

# Рекомендации
echo -e "\n=== Рекомендации ==="
if ! nc -z localhost 6379 2>/dev/null; then
    echo "1. Запустите Redis:"
    echo "   docker-compose -f docker-compose.redis.yml up -d"
    echo "2. Или установите Redis локально:"
    echo "   sudo apt install redis-server  # Ubuntu/Debian"
    echo "   brew install redis             # Mac"
fi
echo "3. Проверьте настройки брандмауэра"
echo "4. Убедитесь, что Redis слушает на 0.0.0.0:6379"

echo -e "\nПодробнее: CHECK_REDIS.md"

# Проверка работы приложения с Redis
echo -e "\n=== Дополнительная проверка ==="
echo "Для проверки работы кеширования:"
echo "1. Запустите приложение: dotnet run --project Wf.WebApi"
echo "2. Выполните запрос: curl 'http://localhost:5000/api/weatherforecast?latitude=55.7558&longitude=37.6173&days=3'"
echo "3. Повторите запрос - второй раз должен быть cache hit"