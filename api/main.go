package main

import (
	"encoding/json"
	"fmt"
	"log"
	"math/rand"
	"net/http"
	"strconv"
	"time"
)

type Response struct {
	Id   string
	Name string
	Time int64
}

func main() {
	http.HandleFunc("/data", func(w http.ResponseWriter, r *http.Request) {
		rsp := Response{
			Id:   "id_" + strconv.Itoa(rand.Int()),
			Name: "name_" + strconv.Itoa(rand.Int()),
			Time: time.Now().Unix(),
		}

		js, err := json.Marshal(rsp)
		if err != nil {
			http.Error(w, err.Error(), http.StatusInternalServerError)
			return
		}

		w.Header().Set("Content-Type", "application/json")
		if _, err := w.Write(js); err != nil {
			http.Error(w, err.Error(), http.StatusInternalServerError)
			return
		}
	})

	addr := ":5002"
	fmt.Println("listening on " + addr)
	log.Fatal(http.ListenAndServe(addr, nil))
}
