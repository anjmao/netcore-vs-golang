FROM golang:1.11.2

WORKDIR /go/src/app
COPY . .
RUN go install ./...

CMD ["/go/bin/app"]