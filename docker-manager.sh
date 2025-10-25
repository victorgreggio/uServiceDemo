#!/bin/bash
# Docker build and deployment script for uServiceDemo

set -e  # Exit on error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Function to print colored messages
print_info() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

print_warn() {
    echo -e "${YELLOW}[WARN]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Function to check if Docker is running
check_docker() {
    print_info "Checking Docker..."
    if ! docker info > /dev/null 2>&1; then
        print_error "Docker is not running. Please start Docker Desktop."
        exit 1
    fi
    print_info "Docker is running."
}

# Function to build images
build_images() {
    print_info "Building Docker images..."
    docker-compose build --parallel
    print_info "Build completed successfully."
}

# Function to start services
start_services() {
    print_info "Starting services..."
    docker-compose up -d
    print_info "Services started. Waiting for health checks..."
    sleep 10
    docker-compose ps
}

# Function to stop services
stop_services() {
    print_info "Stopping services..."
    docker-compose down
    print_info "Services stopped."
}

# Function to restart services
restart_services() {
    stop_services
    start_services
}

# Function to view logs
view_logs() {
    SERVICE=$1
    if [ -z "$SERVICE" ]; then
        docker-compose logs -f
    else
        docker-compose logs -f "$SERVICE"
    fi
}

# Function to clean up everything
cleanup() {
    print_warn "This will remove all containers, networks, and volumes."
    read -p "Are you sure? (y/N): " -n 1 -r
    echo
    if [[ $REPLY =~ ^[Yy]$ ]]; then
        print_info "Cleaning up..."
        docker-compose down -v --remove-orphans
        print_info "Cleanup completed."
    else
        print_info "Cleanup cancelled."
    fi
}

# Function to show status
show_status() {
    print_info "Service Status:"
    docker-compose ps
    echo
    print_info "Service URLs:"
    echo "  UI (Blazor):        http://localhost:8082"
    echo "  API (Swagger):      http://localhost:8080/swagger"
    echo "  API (Health):       http://localhost:8080/health"
    echo "  PostgreSQL:         localhost:5432"
    echo "  MongoDB:            localhost:27017"
    echo "  Elasticsearch:      http://localhost:9200"
}

# Function to run tests
run_tests() {
    print_info "Running tests in containers..."
    docker-compose run --rm api dotnet test
}

# Main menu
show_menu() {
    echo
    echo "================================"
    echo "uServiceDemo Docker Manager"
    echo "================================"
    echo "1. Build images"
    echo "2. Start services"
    echo "3. Stop services"
    echo "4. Restart services"
    echo "5. View logs (all)"
    echo "6. View logs (specific service)"
    echo "7. Show status"
    echo "8. Run tests"
    echo "9. Clean up (remove volumes)"
    echo "10. Build and start"
    echo "0. Exit"
    echo "================================"
}

# Parse command line arguments
if [ $# -gt 0 ]; then
    case "$1" in
        build)
            check_docker
            build_images
            ;;
        start)
            check_docker
            start_services
            ;;
        stop)
            stop_services
            ;;
        restart)
            check_docker
            restart_services
            ;;
        logs)
            view_logs "$2"
            ;;
        status)
            show_status
            ;;
        test)
            check_docker
            run_tests
            ;;
        clean)
            cleanup
            ;;
        up)
            check_docker
            build_images
            start_services
            show_status
            ;;
        *)
            echo "Usage: $0 {build|start|stop|restart|logs [service]|status|test|clean|up}"
            echo
            echo "Commands:"
            echo "  build      - Build Docker images"
            echo "  start      - Start all services"
            echo "  stop       - Stop all services"
            echo "  restart    - Restart all services"
            echo "  logs       - View logs (optionally specify service: api, worker, ui)"
            echo "  status     - Show service status and URLs"
            echo "  test       - Run tests in containers"
            echo "  clean      - Remove all containers, networks, and volumes"
            echo "  up         - Build images and start services (full setup)"
            exit 1
            ;;
    esac
else
    # Interactive menu
    while true; do
        show_menu
        read -p "Enter your choice: " choice
        case $choice in
            1)
                check_docker
                build_images
                ;;
            2)
                check_docker
                start_services
                ;;
            3)
                stop_services
                ;;
            4)
                check_docker
                restart_services
                ;;
            5)
                view_logs
                ;;
            6)
                read -p "Enter service name (api/worker/ui): " service
                view_logs "$service"
                ;;
            7)
                show_status
                ;;
            8)
                check_docker
                run_tests
                ;;
            9)
                cleanup
                ;;
            10)
                check_docker
                build_images
                start_services
                show_status
                ;;
            0)
                print_info "Exiting..."
                exit 0
                ;;
            *)
                print_error "Invalid choice. Please try again."
                ;;
        esac
        echo
        read -p "Press Enter to continue..."
    done
fi
