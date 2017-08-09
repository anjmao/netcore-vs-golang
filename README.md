# .NET Core vs Golang performance

This repository holds code to test http api performance between .NET Core and Golang HTTP.
Each service has `/test` endpoint which calls another api using http client and returns that api response as JSON.

## Start containers

`docker-compose up --build`

docker-compose should start 3 containers
1) Golang api with GET `http://localhost:5002/data` endpoint
2) Golang api with GET `http://localhost:5001/test` endpoint which calls 1 endpoint
3) .NET Core api with GET `http://localhost:5000/test` endpoint which calls 1 endpoint

## Run load tests

```
brew install wrk
cd wrk
// .net core
URL=http://localhost:5000 make run

// golang
URL=http://localhost:5001 make run
```

## Check for file descriptors leaks

Connect to docker container while wrk is running
`docker exec -it <CONTAINER_ID> /bin/bash`

Count TIME_WAIT state
`ss -t4 state time-wait | wc -l`

## Results

### .net core api (http://localhost:5000)

```
wrk --connections 256 --duration 100s --threads 8 --timeout 5s --latency --script /Users/anma/go/src/github.com/anjmao/netcore-vs-golang/wrk/requests.lua http://localhost:5000
Running 2m test @ http://localhost:5000
  8 threads and 256 connections
  Thread Stats   Avg      Stdev     Max   +/- Stdev
    Latency   100.52ms   28.07ms 281.63ms   72.16%
    Req/Sec   316.30     85.83   800.00     72.31%
  Latency Distribution
     50%   98.94ms
     75%  115.71ms
     90%  134.77ms
     99%  186.74ms
  252037 requests in 1.67m, 54.50MB read
  Socket errors: connect 0, read 347, write 1, timeout 0
Requests/sec:   2518.12
Transfer/sec:    557.63KB
```

Resources used
```
CPU: 150%
MEMORY: 168MB
TIME_WAIT file descriptors: ~3000
```

### golang api (http://localhost:5001)

```
wrk --connections 256 --duration 100s --threads 8 --timeout 5s --latency --script /Users/anma/go/src/github.com/anjmao/netcore-vs-golang/wrk/requests.lua http://localhost:5001
Running 2m test @ http://localhost:5001
  8 threads and 256 connections
  Thread Stats   Avg      Stdev     Max   +/- Stdev
    Latency    43.90ms   21.67ms 321.66ms   70.69%
    Req/Sec   733.84    154.79     1.55k    80.85%
  Latency Distribution
     50%   41.75ms
     75%   55.51ms
     90%   71.49ms
     99%  109.41ms
  584625 requests in 1.67m, 106.36MB read
  Socket errors: connect 0, read 284, write 0, timeout 0
Requests/sec:   5842.33
Transfer/sec:      1.06MB
```

Resources used
```
CPU: 130%
MEMORY: 22.57MB
TIME_WAIT file descriptors: 4
```

## My machine spec

* MacBook Pro (15-inch, 2017)
* Processor 2,9 GHz Intel Core i7
* Memory 16 GB 2133 MHz LPDDR3
* Docker version 17.06.0-ce, build 02c1d87
* Golang 1.8.3
* Dotnet 2.0.0-preview2-006497
