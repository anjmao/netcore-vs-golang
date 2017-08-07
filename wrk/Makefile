URL?=http://localhost:5000

WRK=wrk --connections 256 \
	--duration 100s \
	--threads 8 \
	--timeout 5s \
	--latency \
	--script \
	$(shell pwd)/requests.lua \
	$(URL)

run:
	$(WRK)