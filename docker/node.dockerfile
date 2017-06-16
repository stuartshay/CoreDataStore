FROM node:6.10.2
MAINTAINER Stuart Shay

# Create and copy app directory
RUN mkdir -p /app
WORKDIR /app
COPY src/CoreDataStore.Web/wwwroot ./wwwroot
COPY _package.json /app/package.json
COPY server.js /app/

# Install app dependencies
RUN npm i

EXPOSE 5000
CMD [ "npm", "start" ]