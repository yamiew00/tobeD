# **Question Chart**
## * <a href="https://onepaper-api-dev.oneclass.com.tw/swagger/index.html">swagger 測試路徑</a> *

---
### **<font color=#1AFD9C>[POST] /api/Question/Chart </font>**
> 計算試題分布狀況

* ### Header
  > 帶入user token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
  ```json
  {
    "searchKey": "cf9b031535834f608a1f47e2eb6cb200",
    "questions": [
      "f9ae3dad217f44c799628a7a7e214351",
      "07d26adbb79749608915f0104be35d18",
      "194198a9733e4ded8149a7a0a3b55432",
      "2c09d14293f144c597ce4b2f9bb6e6a2",
      "d2a1499d3d9e42a8a96f08abfa7bc148",
      "74084f7d2bdc41ea9eb88401386f9283",
      "8b7096d306f64d2a967870cfec1f597f",
      "d0cd87a254224293a447175db6d4a3d4",
      "94f7d673e2d54f7e938c944c69af2b3b",
      "1f3c5174aaa84b4a8727e12df2cb96fb",
      "9249f3b980ad4f148f2519e5a1ee1014",
      "805c7a8aa074464195c72d9741caeeee",
      "1e2e3aacd28f41a89838a0be6bd3b167"
    ]
  }
  ```
  >| Name     | Meaning
  > --------- | ---------------------
  > searchKey | 查詢識別Key
  > questions | 試題ID

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-12-01T17:31:22.1232307+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "typeMap": [
        {
          "code": "QUES_TYPE",
          "name": "題型"
        },
        {
          "code": "DIFFICULTY",
          "name": "難易度"
        },
        {
          "code": "KNOWLEDGE",
          "name": "知識向度"
        }
      ],
      "chart": {
        "QUES_TYPE": {
          "code": "QUES_TYPE",
          "name": "題型",
          "total": 13,
          "item": [
            {
              "code": "SS017",
              "name": "單選題",
              "count": 13,
              "percent": 100
            }
          ]
        },
        "DIFFICULTY": {
          "code": "DIFFICULTY",
          "name": "難易度",
          "total": 13,
          "item": [
            {
              "code": "BEGIN",
              "name": "易",
              "count": 8,
              "percent": 61.54
            },
            {
              "code": "INTERMEDIATE",
              "name": "中",
              "count": 5,
              "percent": 38.46
            }
          ]
        }
      }
    }
  }
  ```

  > 欄位說明
  >| Name        | Meaning 
  > ------------ | ------- 
  > typeMap  | 分布類別清單
  > | <font color=#66B3FF> chart </font> | <font color=#66B3FF> 試題分布資訊 </font>
  >  .key    | 類別代碼
  > code     | 類別代碼
  > name     | 類別名稱
  > total    | 總數
  > | <font color=#66B3FF> .item </font> | <font color=#66B3FF> 項目資訊 </font>
  > code     | 項目代碼
  > name     | 項目名稱
  > count    | 項目數量
  > percent  | 項目占比
---
