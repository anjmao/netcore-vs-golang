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
    Latency    39.46ms   61.00ms   1.04s    98.66%
    Req/Sec     0.95k   194.35     2.85k    72.86%
  Latency Distribution
     50%   33.59ms
     75%   39.36ms
     90%   45.37ms
     99%  254.22ms
  745477 requests in 1.67m, 161.21MB read
  Socket errors: connect 0, read 304, write 0, timeout 0
Requests/sec:   7447.74
Transfer/sec:      1.61MB
```

Resources used
```
CPU: 100%
MEMORY: 82MB
TIME_WAIT file descriptors: ~3
```

### golang api (http://localhost:5001)

```
Running 2m test @ http://localhost:5001
  8 threads and 256 connections
  Thread Stats   Avg      Stdev     Max   +/- Stdev
    Latency    61.20ms   79.19ms   1.10s    98.25%
    Req/Sec   605.58    134.67     1.36k    81.43%
  Latency Distribution
     50%   53.42ms
     75%   67.89ms
     90%   82.50ms
     99%  500.11ms
  475287 requests in 1.67m, 86.47MB read
  Socket errors: connect 0, read 363, write 2, timeout 0
Requests/sec:   4751.79
Transfer/sec:      0.86MB
```

Resources used
```
CPU: 100%
MEMORY: 25.57MB
TIME_WAIT file descriptors: ~10
```

## My machine spec

* MacBook Pro (15-inch, 2017)
* Processor 2,9 GHz Intel Core i7
* Memory 16 GB 2133 MHz LPDDR3
* Docker version 18.06.0-ce (4 CPUs, 2 GiB memory)
* Golang 1.11.2
* Dotnet 2.1.0
