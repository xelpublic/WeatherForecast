param(
    [switch]$Build = $false,
    [switch]$NoBrowser = $false,
    [int]$WaitSeconds = 10
)

Write-Host "=========================================" -ForegroundColor Cyan
Write-Host "WeatherForecast Docker Compose Starter" -ForegroundColor Cyan
Write-Host "=========================================" -ForegroundColor Cyan
Write-Host ""

# Check if Docker is running
try {
    $dockerVersion = docker --version
    Write-Host "Docker detected: $dockerVersion" -ForegroundColor Green
} catch {
    Write-Host "ERROR: Docker is not running or not installed!" -ForegroundColor Red
    Write-Host "Please start Docker Desktop and try again." -ForegroundColor Yellow
    exit 1
}

# Check if docker-compose is available
try {
    $composeVersion = docker-compose --version
    Write-Host "Docker Compose detected: $composeVersion" -ForegroundColor Green
} catch {
    Write-Host "WARNING: docker-compose command not found, trying docker compose..." -ForegroundColor Yellow
    try {
        $composeVersion = docker compose version
        Write-Host "Docker Compose (plugin) detected" -ForegroundColor Green
        $composeCommand = "docker compose"
    } catch {
        Write-Host "ERROR: Neither docker-compose nor docker compose are available!" -ForegroundColor Red
        exit 1
    }
}

$ProjectName = "weatherforecast"

if (-not $composeCommand) {
    $composeCommand = "docker-compose"
}

# Add project name to compose command
$composeCommandWithProject = "$composeCommand -p $ProjectName"

Write-Host ""
Write-Host "Starting Docker Compose stack: $ProjectName" -ForegroundColor Yellow

# Build images if requested
if ($Build) {
    Write-Host "Building Docker images (this may take a while)..." -ForegroundColor Yellow
    Invoke-Expression "$composeCommandWithProject build"
    if ($LASTEXITCODE -ne 0) {
        Write-Host "ERROR: Build failed!" -ForegroundColor Red
        exit 1
    }
    Write-Host "Build completed successfully." -ForegroundColor Green
}

# Start services in detached mode
Write-Host "Starting services in detached mode..." -ForegroundColor Yellow
Invoke-Expression "$composeCommandWithProject up -d"
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Failed to start services!" -ForegroundColor Red
    exit 1
}

Write-Host "Services started successfully." -ForegroundColor Green
Write-Host ""

# Show service status
Write-Host "Service status for '$ProjectName' stack:" -ForegroundColor Cyan
Invoke-Expression "$composeCommandWithProject ps"
Write-Host ""

# Wait for services to be ready
if ($WaitSeconds -gt 0) {
    Write-Host "Waiting $WaitSeconds seconds for services to initialize..." -ForegroundColor Yellow
    Start-Sleep -Seconds $WaitSeconds
    
    # Check if frontend is responding
    Write-Host "Checking if frontend is accessible..." -ForegroundColor Yellow
    try {
        $response = Invoke-WebRequest -Uri "http://localhost" -UseBasicParsing -TimeoutSec 5 -ErrorAction SilentlyContinue
        if ($response.StatusCode -eq 200) {
            Write-Host "Frontend is responding (HTTP 200)." -ForegroundColor Green
        } else {
            Write-Host "Frontend responded with HTTP $($response.StatusCode)." -ForegroundColor Yellow
        }
    } catch {
        Write-Host "Frontend not yet ready (will open browser anyway)..." -ForegroundColor Yellow
    }
}

# Open browser if not disabled
if (-not $NoBrowser) {
    Write-Host ""
    Write-Host "Opening browser to http://localhost..." -ForegroundColor Cyan
    
    # Try different methods to open browser
    $url = "http://localhost"
    
    # Method 1: Start-Process (works on Windows)
    try {
        Start-Process $url
        Write-Host "Browser opened successfully." -ForegroundColor Green
    } catch {
        # Method 2: Using default browser registry
        try {
            $browserPath = (Get-ItemProperty 'HKCU:\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice' -ErrorAction SilentlyContinue).ProgId
            if ($browserPath) {
                $browserCommand = (Get-ItemProperty "HKLM:\SOFTWARE\Classes\$browserPath\shell\open\command" -ErrorAction SilentlyContinue).'(default)'
                if ($browserCommand) {
                    $browserCommand = $browserCommand -replace '"', '' -replace '%1', $url
                    Start-Process $browserCommand
                    Write-Host "Browser opened using registry method." -ForegroundColor Green
                }
            }
        } catch {
            Write-Host "Could not automatically open browser. Please open manually: $url" -ForegroundColor Yellow
        }
    }
}

Write-Host ""
Write-Host "=========================================" -ForegroundColor Cyan
Write-Host "Docker Compose stack '$ProjectName' is running in background." -ForegroundColor Cyan
Write-Host "  • Frontend:      http://localhost" -ForegroundColor White
Write-Host "To stop: .\stop.ps1" -ForegroundColor Gray
Write-Host "=========================================" -ForegroundColor Cyan