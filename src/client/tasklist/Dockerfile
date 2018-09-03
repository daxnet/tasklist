FROM node:8 AS build
RUN npm install -g @angular/cli@6.1.5
WORKDIR /src
COPY . .
RUN npm install
RUN ng build --prod

FROM nginx AS final
COPY --from=build /src/dist/tasklist /usr/share/nginx/html
