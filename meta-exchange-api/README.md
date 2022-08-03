# meta-exchange-api

## Description
The meta-exchange-api project is a ASP.NET Core API endpoint for interacting with the Meta Exchange Class Library, that supports the usage inside a docker container.

## Building the Image
make sure you are in the directory containing the Dockerfile and have Docker installed.
```shell
docker build -t meta-exchange-api .
```

## Usage
run the docker image, mapping port 5000 on host machine to port 80 inside the container.
```shell
docker run -t -p 5000:80 --rm meta-exchange-api
```
Note: "-rm" removes the container after stopping it and is just a personal preference.

When the container is running, you can reach the API endpoint using:
```shell
curl http://127.0.0.1:5000/order-create/<eurBalance>/<btcBalance>/<orderType>/<orderAmount>
```

## Example
### Request
```shell
curl http://127.0.0.1:5000/order-create/25000/0/Buy/5
```
### Response
```json
[
    {
        "Id":null,
        "Time":"2022-08-03T08:44:43.6781377+02:00",
        "Type":"Buy",
        "Kind":"Limit",
        "Amount":0.1,
        "Price":2901.95
    },
    {
        "Id":null,
        "Time":"2022-08-03T08:44:43.6822401+02:00",
        "Type":"Buy",
        "Kind":"Limit",
        "Amount":4.9,
        "Price":2902.0
    }
]
```