# Stage 1: Build the Angular app
FROM node:22.11 AS build
WORKDIR /app
COPY UrlShortener.Client/. .
RUN npm i -g @angular/cli
RUN npm install
RUN ng build --configuration production

# Stage 2: Serve with NGINX
FROM nginx:latest
COPY --from=build /app/dist/url-shortener-client/browser /usr/share/nginx/html
COPY UrlShortener.Client/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
