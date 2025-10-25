@echo off
REM Docker build and deployment script for uServiceDemo (Windows)

setlocal enabledelayedexpansion

:menu
cls
echo ================================
echo uServiceDemo Docker Manager
echo ================================
echo 1. Build images
echo 2. Start services
echo 3. Stop services
echo 4. Restart services
echo 5. View logs (all)
echo 6. View logs (specific service)
echo 7. Show status
echo 8. Clean up (remove volumes)
echo 9. Build and start
echo 0. Exit
echo ================================
echo.

set /p choice="Enter your choice: "

if "%choice%"=="1" goto build
if "%choice%"=="2" goto start
if "%choice%"=="3" goto stop
if "%choice%"=="4" goto restart
if "%choice%"=="5" goto logs_all
if "%choice%"=="6" goto logs_service
if "%choice%"=="7" goto status
if "%choice%"=="8" goto cleanup
if "%choice%"=="9" goto build_and_start
if "%choice%"=="0" goto end
goto menu

:build
echo [INFO] Building Docker images...
docker-compose build --parallel
if errorlevel 1 (
    echo [ERROR] Build failed!
    pause
    goto menu
)
echo [INFO] Build completed successfully.
pause
goto menu

:start
echo [INFO] Starting services...
docker-compose up -d
if errorlevel 1 (
    echo [ERROR] Failed to start services!
    pause
    goto menu
)
echo [INFO] Services started. Waiting for health checks...
timeout /t 10 /nobreak >nul
docker-compose ps
pause
goto menu

:stop
echo [INFO] Stopping services...
docker-compose down
echo [INFO] Services stopped.
pause
goto menu

:restart
call :stop
call :start
goto menu

:logs_all
echo [INFO] Viewing all logs (Ctrl+C to exit)...
docker-compose logs -f
pause
goto menu

:logs_service
set /p service="Enter service name (api/worker/ui): "
echo [INFO] Viewing logs for %service% (Ctrl+C to exit)...
docker-compose logs -f %service%
pause
goto menu

:status
echo [INFO] Service Status:
docker-compose ps
echo.
echo [INFO] Service URLs:
echo   UI (Blazor):        http://localhost:8082
echo   API (Swagger):      http://localhost:8080/swagger
echo   API (Health):       http://localhost:8080/health
echo   PostgreSQL:         localhost:5432
echo   MongoDB:            localhost:27017
echo   Elasticsearch:      http://localhost:9200
pause
goto menu

:cleanup
echo [WARN] This will remove all containers, networks, and volumes.
set /p confirm="Are you sure? (y/N): "
if /i "%confirm%"=="y" (
    echo [INFO] Cleaning up...
    docker-compose down -v --remove-orphans
    echo [INFO] Cleanup completed.
) else (
    echo [INFO] Cleanup cancelled.
)
pause
goto menu

:build_and_start
call :build
call :start
call :status
goto menu

:end
echo [INFO] Exiting...
exit /b 0
