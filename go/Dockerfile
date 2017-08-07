FROM golang:1.8.3

RUN apt-get update && apt-get install net-tools

WORKDIR /go/src/app
COPY . .

RUN go-wrapper download
RUN go-wrapper install 

CMD ["go-wrapper", "run"]
