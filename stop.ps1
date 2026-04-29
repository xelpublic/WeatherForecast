param(
    [int]$Timeout = 10
)

$ProjectName = "weatherforecast"

Write-Host "=========================================" -ForegroundColor Cyan
Write-Host "WeatherForecast Docker Compose Stopper" -ForegroundColor Cyan
Write-Host "=========================================" -ForegroundColor Cyan
Write-Host ""

# Check if Docker is running
try {
    $null = docker --version
    Write-Host "Docker detected." -ForegroundColor Green
} catch {
    Write-Host "WARNING: Docker is not running or not installed!" -ForegroundColor Yellow
    Write-Host "Services may already be stopped." -ForegroundColor Yellow
}

# Determine docker-compose command
$composeCommand = $null
try {
    $null = docker-compose --version
    $composeCommand = "docker-compose"
    Write-Host "Using docker-compose command." -ForegroundColor Green
} catch {
    try {
        $null = docker compose version
        $composeCommand = "docker compose"
        Write-Host "Using docker compose (plugin) command." -ForegroundColor Green
    } catch {
        Write-Host "ERROR: Neither docker-compose nor docker compose are available!" -ForegroundColor Red
        Write-Host "Cannot stop services without Docker Compose." -ForegroundColor Yellow
        exit 1
    }
}

# Add project name to compose command
$composeCommandWithProject = "$composeCommand -p $ProjectName"


try {
    $services = Invoke-Expression "$composeCommandWithProject ps --services --filter status=running" 2>$null
    if ($services -and $services.Count -gt 0) {
        Write-Host "Found $($services.Count) running service(s) in '$ProjectName' stack." -ForegroundColor Green
        
        # Show current running services for this project
        Write-Host "Current running services:" -ForegroundColor Cyan
        Invoke-Expression "$composeCommandWithProject ps"
        
        Write-Host ""
        Write-Host "Stopping services..." -ForegroundColor Yellow
        
        # Stop services with timeout
        Invoke-Expression "$composeCommandWithProject stop -t $Timeout"
        
        if ($LASTEXITCODE -ne 0) {
            Write-Host "WARNING: Some services may not have stopped cleanly." -ForegroundColor Yellow
        } else {
            Write-Host "Services stopped successfully." -ForegroundColor Green
        }
    } else {
        Write-Host "No running services found in '$ProjectName' stack." -ForegroundColor Yellow
        Write-Host "Stack may already be stopped or doesn't exist." -ForegroundColor Gray
    }
} catch {
    Write-Host "Could not check stack status (stack may not exist)." -ForegroundColor Yellow
    Write-Host "Assuming services are already stopped." -ForegroundColor Gray
}

# Final status check

Write-Host ""
Write-Host "=========================================" -ForegroundColor Cyan
Write-Host "Docker Compose '$ProjectName' has been stopped." -ForegroundColor Cyan
Write-Host "=========================================" -ForegroundColor Cyan
