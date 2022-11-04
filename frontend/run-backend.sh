set -e

printf 'Starting services...\n'
cd ../backend
docker-compose up --d
printf 'Services started\n'
printf 'Running API, CTRL+C to stop\n'
dotnet run --project ../backend/API