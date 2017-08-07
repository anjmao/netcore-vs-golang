package main

import (
	"encoding/json"
	"fmt"
	"html"
	"log"
	"net/http"
	"os"
)

type Response struct {
	Id   string
	Name string
	Time int64
}

func main() {
	url := "http://" + os.Getenv("HOST") + ":5002/data"
	tr := &http.Transport{
		MaxIdleConns:        4000,
		MaxIdleConnsPerHost: 4000,
	}
	client := &http.Client{Transport: tr}

	http.HandleFunc("/test", func(w http.ResponseWriter, r *http.Request) {
		rsp, err := client.Get(url)
		if err != nil {
			serverError(w, err.Error())
			return
		}

		defer rsp.Body.Close()

		// deserialize
		obj := Response{}
		err = json.NewDecoder(rsp.Body).Decode(&obj)
		if err != nil {
			serverError(w, err.Error())
			return
		}

		// serialize
		jsonStr, err := json.Marshal(&obj)
		if err != nil {
			serverError(w, err.Error())
			return
		}

		w.Header().Set("Content-Type", "application/json")
		w.Write(jsonStr)
	})

	http.HandleFunc("/", func(w http.ResponseWriter, r *http.Request) {
		fmt.Fprintf(w, "Hello, %q", html.EscapeString(r.URL.Path))
	})

	addr := ":5001"
	fmt.Println("listening on " + addr)
	log.Fatal(http.ListenAndServe(addr, nil))
}

func serverError(w http.ResponseWriter, msg string) {
	http.Error(w, msg, http.StatusInternalServerError)
}
