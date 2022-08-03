# meta-exchange-cli

## Description
The meta-exchange-cli project is a .NET Core command-line interface for interacting with the Meta Exchange Class Library.

## Usage
```shell
.\meta-exchange-cli.exe <orderbooks-data-path> <eur-balance> <btc-balance> <order-type> <order-amount>
```

## Example
### Request
```shell
.\meta-exchange-cli.exe C:\DEV\meta-exchange-cli\order_books_data.json 25000 0 Buy 5
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