FROM node:22.11-alpine3.19 as builder

WORKDIR /app

COPY ./package.json ./
RUN npm install

RUN npm run build 

COPY . .

FROM node:22.11-alpine3.19

WORKDIR /app

COPY --from=builder /app/.output ./.output

EXPOSE 3000

CMD [ "node", ".output/server/index.mjs" ]