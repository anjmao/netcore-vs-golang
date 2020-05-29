FROM golang:1.14.3

WORKDIR /go/src/app
COPY . .
RUN go install ./...

CMD ["/go/bin/app"]