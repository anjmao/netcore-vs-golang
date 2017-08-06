# netcore-vs-golang

Test http client, object serialize and deserialize times

## Start containers
`docker-compose up -e HOST=192.168.1.145`

Note: change HOST to your local host IP

### .net core api

```
wrk --connections 256 --duration 60s --threads 8 --timeout 5s --latency --script /Users/anma/go/src/github.com/anjmao/netcore-vs-golang/wrk/requests.lua http://localhost:5000
Running 1m test @ http://localhost:5000
  8 threads and 256 connections
  Thread Stats   Avg      Stdev     Max   +/- Stdev
    Latency   135.48ms   36.55ms 307.72ms   73.63%
    Req/Sec   228.45     83.09   555.00     60.63%
  Latency Distribution
     50%  133.44ms
     75%  156.51ms
     90%  181.22ms
     99%  229.40ms
  109006 requests in 1.00m, 23.57MB read
  Socket errors: connect 0, read 415, write 2, timeout 0
  Non-2xx or 3xx responses: 6
Requests/sec:   1815.36
Transfer/sec:    401.99KB
```

### golang api

```
wrk --connections 256 --duration 60s --threads 8 --timeout 5s --latency --script /Users/anma/go/src/github.com/anjmao/netcore-vs-golang/wrk/requests.lua http://localhost:5001
Running 1m test @ http://localhost:5001
  8 threads and 256 connections
  Thread Stats   Avg      Stdev     Max   +/- Stdev
    Latency    82.78ms  135.06ms   1.76s    95.92%
    Req/Sec   234.87    172.69     1.02k    72.98%
  Latency Distribution
     50%   54.63ms
     75%   85.81ms
     90%  134.80ms
     99%  800.28ms
  106498 requests in 1.00m, 19.37MB read
  Socket errors: connect 0, read 32597, write 0, timeout 0
Requests/sec:   1772.37
Transfer/sec:    330.17KB
```

