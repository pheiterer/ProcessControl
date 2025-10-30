# Stage 1: Build the Angular application
FROM node:20-alpine AS build

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

RUN npm run build -- --configuration production

# Stage 2: Serve the application from Nginx
FROM nginx:1.25-alpine

# Copy the build output to replace the default Nginx content.
COPY --from=build /app/dist/processcontrol.client/browser /usr/share/nginx/html

# Copy our custom nginx config
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
