FROM node:22.11-alpine3.19 as builder


WORKDIR /app

COPY ./package.json ./package-lock.json
RUN npm ci

COPY . .


FROM node:22.11-alpine3.19

WORKDIR /app

COPY --from=builder /app/.output ./.output

EXPOSE 3000

CMD [ "npm", "run", "dev" ]
