# **Anomaly**
## * <a href="https://onepaper-api-dev.oneclass.com.tw/swagger/index.html">swagger 測試路徑</a> *

### **<font color=#66B3FF>[GET] /api/Anomaly/Related </font>**
> 取得錯題回報相關選單

* ### Header
  > 帶入user token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
  *none*

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-10-28T11:59:44.7268485+08:00",
    "message": "",
    "disposal": null,
    "data": [
      {
        "code": "Content",
        "name": "試題內容有誤"
      },
      {
        "code": "Answer",
        "name": "答案有誤"
      },
      {
        "code": "Range",
        "name": "超出課綱範圍"
      },
      {
        "code": "Suggestion",
        "name": "試題修改建議"
      }
    ]
  }
  ```

   > 欄位說明
  >| Name | Meaning 
  > ------| ------- 
  > data  | 錯題回報選單
  
---

### **<font color=#1AFD9C>[POST] /api/Anomaly/Report </font>**
> 試題回報

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
    "education": "E",
    "subject": "LI",
    "uid": "6f8c7a93fa884848a5383a31977a5a52",
    "anomalyType": "Answer",
    "description": "系統測試"
  }
  ```
  >| Name     | Meaning
  > --------- | ---------------------
  > education | 學制代碼
  > subject   | 科目代碼
  > uid       | 試題ID
  > anomalyType | 回報類別
  > description | 說明

* ### Response
  > 成功
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-11-01T15:45:23.8028456+08:00",
    "message": "",
    "disposal": null,
    "data": null
  }
  ```

  > 失敗
  ```json
  {
    "systemCode": "0400",
    "isSuccess": false,
    "systemNow": "2021-11-01T15:44:46.3098824+08:00",
    "message": "UID資料：6f8c7a93fa884848a5383a31977a5a52不存在, 請確認輸入內容。",
    "disposal": null,
    "data": null
  }
  ```
---