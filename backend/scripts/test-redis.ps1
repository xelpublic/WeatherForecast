# PowerShell скрипт для проверки Redis
Write-Host "=== Проверка Redis ===" -ForegroundColor Cyan

# Проверка, запущен ли Docker
$dockerRunning = $false
try {
    docker version 2>$null | Out-Null
    $dockerRunning = $true
    Write-Host "✓ Docker доступен" -ForegroundColor Green
} catch {
    Write-Host "✗ Docker не доступен" -ForegroundColor Yellow
}

# Проверка порта Redis
Write-Host "`nПроверка порта 6379..." -ForegroundColor Cyan
$portTest = Test-NetConnection localhost -Port 6379 -WarningAction SilentlyContinue -ErrorAction SilentlyContinue

if ($portTest.TcpTestSucceeded) {
    Write-Host "✓ Порт 6379 открыт" -ForegroundColor Green
    
    # Попытка подключиться через redis-cli
    Write-Host "`nПроверка подключения Redis..." -ForegroundColor Cyan
    try {
        # Проверяем, установлен ли redis-cli
        $redisCli = Get-Command redis-cli -ErrorAction SilentlyContinue
        if ($redisCli) {
            $result = redis-cli -h localhost -p 6379 ping 2>$null
            if ($result -eq "PONG") {
                Write-Host "✓ Redis отвечает (PONG)" -ForegroundColor Green
            } else {
                Write-Host "✗ Redis не отвечает правильно" -ForegroundColor Red
            }
        } else {
            Write-Host "ℹ redis-cli не установлен, используем telnet..." -ForegroundColor Yellow
            # Альтернативная проверка через telnet
            $telnetResult = telnet localhost 6379 2>&1
            if ($telnetResult -like "*Connected*") {
                Write-Host "✓ Подключение к Redis установлено" -ForegroundColor Green
            }
        }
    } catch {
        Write-Host "✗ Ошибка при проверке Redis: $_" -ForegroundColor Red
    }
} else {
    Write-Host "✗ Порт 6379 закрыт" -ForegroundColor Red
    
    # Проверка Docker контейнеров
    if ($dockerRunning) {
        Write-Host "`nПроверка Docker контейнеров Redis..." -ForegroundColor Cyan
        $redisContainer = docker ps --filter "name=redis" --format "{{.Names}}" 2>$null
        if ($redisContainer) {
            Write-Host "✓ Найден контейнер Redis: $redisContainer" -ForegroundColor Green
            Write-Host "  Запущен, но порт может не быть проброшен" -ForegroundColor Yellow
        } else {
            Write-Host "✗ Контейнер Redis не найден" -ForegroundColor Red
        }
    }
}

# Проверка строки подключения в appsettings
Write-Host "`nПроверка конфигурации приложения..." -ForegroundColor Cyan
$appSettingsPath = "Wf.WebApi\appsettings.json"
$appSettingsDevPath = "Wf.WebApi\appsettings.Development.json"

if (Test-Path $appSettingsPath) {
    $appSettings = Get-Content $appSettingsPath -Raw | ConvertFrom-Json
    if ($appSettings.ConnectionStrings.Redis) {
        Write-Host "✓ Найдена строка подключения Redis в appsettings.json" -ForegroundColor Green
        Write-Host "  $($appSettings.ConnectionStrings.Redis)" -ForegroundColor Gray
    }
}

if (Test-Path $appSettingsDevPath) {
    $appSettingsDev = Get-Content $appSettingsDevPath -Raw | ConvertFrom-Json
    if ($appSettingsDev.ConnectionStrings.Redis) {
        Write-Host "✓ Найдена строка подключения Redis в appsettings.Development.json" -ForegroundColor Green
        Write-Host "  $($appSettingsDev.ConnectionStrings.Redis)" -ForegroundColor Gray
    }
}

# Рекомендации
Write-Host "`n=== Рекомендации ===" -ForegroundColor Cyan
if (-not $portTest.TcpTestSucceeded) {
    Write-Host "1. Запустите Redis:" -ForegroundColor Yellow
    Write-Host "   docker-compose -f docker-compose.redis.yml up -d" -ForegroundColor White
    Write-Host "2. Или установите Redis локально" -ForegroundColor Yellow
}
Write-Host "3. Проверьте настройки брандмауэра" -ForegroundColor Yellow
Write-Host "4. Убедитесь, что Redis слушает на 0.0.0.0:6379" -ForegroundColor Yellow

Write-Host "`nПодробнее: CHECK_REDIS.md" -ForegroundColor Gray