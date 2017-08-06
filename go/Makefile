# Update to your host
DOCKER_HOST=192.168.1.145

build:
	docker build -t goapp .

run:
	docker run  -it --rm \
		-e DOCKER_HOST=$(DOCKER_HOST) \
		-p 5001:5001 \
		goapp