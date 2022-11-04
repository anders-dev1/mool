DEFAULT='\033[0m'
GREEN='\033[0;32m'

echo -e "${GREEN}Building backend (API) image...\n${DEFAULT}"
(cd backend && docker build -t moolapi:latest -f api.dockerfile .)

echo -e "\n${GREEN}Building dataseeder image...\n${DEFAULT}"
(cd backend && docker build -t mooldataseeder:latest -f dataseeder.dockerfile .)

echo -e "\n${GREEN}Building frontend image...\n${DEFAULT}"
(cd frontend && docker build -t moolfrontend:latest .)
