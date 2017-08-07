# netcore-vs-golang

Test http client, object serialize and deserialize times

## Start containers
`docker-compose up`

## Run test
`cd wrk && make run`

## Results

### .net core api

```
wrk --connections 256 --duration 60s --threads 8 --timeout 5s --latency --script /Users/anma/go/src/github.com/anjmao/netcore-vs-golang/wrk/requests.lua http://localhost:5000
Running 1m test @ http://localhost:5000
  8 threads and 256 connections
  Thread Stats   Avg      Stdev     Max   +/- Stdev
    Latency    26.01ms   48.62ms   1.11s    99.05%
    Req/Sec   216.88    132.43   750.00     64.13%
  Latency Distribution
     50%   20.18ms
     75%   26.21ms
     90%   35.01ms
     99%   72.83ms
  99502 requests in 1.00m, 21.52MB read
  Socket errors: connect 0, read 31940, write 0, timeout 0
Requests/sec:   1656.06
Transfer/sec:    366.72KB
```

```
CPU: 150%
MEM: 90MB
```

### golang api

```
wrk --connections 256 --duration 60s --threads 8 --timeout 5s --latency --script /Users/anma/go/src/github.com/anjmao/netcore-vs-golang/wrk/requests.lua http://localhost:5001
Running 1m test @ http://localhost:5001
  8 threads and 256 connections
  Thread Stats   Avg      Stdev     Max   +/- Stdev
    Latency    10.45ms   40.09ms 957.84ms   99.25%
    Req/Sec   672.53    512.82     2.11k    66.98%
  Latency Distribution
     50%    6.67ms
     75%    9.10ms
     90%   12.31ms
     99%   30.99ms
  309947 requests in 1.00m, 56.39MB read
  Socket errors: connect 0, read 29980, write 0, timeout 0
Requests/sec:   5157.72
Transfer/sec:      0.94MB
```

```
CPU: 130%
MEM: 10MB
```

