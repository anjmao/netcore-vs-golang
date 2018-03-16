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
Running 2m test @ http://localhost:5000
  8 threads and 256 connections
  Thread Stats   Avg      Stdev     Max   +/- Stdev
    Latency   104.67ms   33.49ms 593.62ms   74.62%
    Req/Sec   303.83     89.02   670.00     71.21%
  Latency Distribution
     50%  101.26ms
     75%  122.38ms
     90%  144.94ms
     99%  198.08ms
  240902 requests in 1.67m, 52.10MB read
  Socket errors: connect 0, read 373, write 0, timeout 0
Requests/sec:   2407.66
Transfer/sec:    533.17KB
```

Resources used
```
CPU: 100%
MEMORY: 138MB
TIME_WAIT file descriptors: ~4000
```

### golang api (http://localhost:5001)

```
Running 2m test @ http://localhost:5001
  8 threads and 256 connections
  Thread Stats   Avg      Stdev     Max   +/- Stdev
    Latency    43.80ms   48.89ms   1.03s    98.61%
    Req/Sec   788.53    157.21     1.58k    72.13%
  Latency Distribution
     50%   40.18ms
     75%   51.54ms
     90%   63.41ms
     99%  103.43ms
  624213 requests in 1.67m, 113.56MB read
  Socket errors: connect 0, read 297, write 0, timeout 0
Requests/sec:   6236.99
Transfer/sec:      1.13MB
```

Resources used
```
CPU: 100%
MEMORY: 22.57MB
TIME_WAIT file descriptors: ~10
```

## My machine spec

* MacBook Pro (15-inch, 2017)
* Processor 2,9 GHz Intel Core i7
* Memory 16 GB 2133 MHz LPDDR3
* Docker version 17.06.0-ce, build 02c1d87
* Golang 1.9
* Dotnet 2.0.0
