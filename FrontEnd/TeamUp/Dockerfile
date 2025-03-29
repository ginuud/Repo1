# Frontend Dockerfile
FROM node:20-bullseye AS builder

# 1. Installeri süsteemsed sõltuvused
RUN npm install -g npm@latest && \
    apt-get update && \
    apt-get install -y python3 make g++

WORKDIR /app

# 2. Kopeeri package.json
COPY package.json .

# 3. Installeri sõltuvused (npm install asemel npm ci)
RUN npm install --legacy-peer-deps && \
    npm install rollup@4.17.2 @rollup/rollup-linux-x64-gnu -D

# 4. Kopeeri kogu kood
COPY . .

RUN npm run build

# Lõplik pilt
FROM nginx:alpine
COPY --from=builder /app/.output/public /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]