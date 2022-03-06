# **Practice**
## * <a href="https://onepaper-api-dev.oneclass.com.tw/swagger/index.html">swagger 測試路徑</a> *

### **<font color=#1AFD9C>[POST] /api/Practice/Create </font>**
> 建立自主練習

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
    "searchKey": "fe15d3a1b47f4031a7aefc48f1e9bbbf",
    "questionGroup": [
      {
        "typeCode": "SS017",
        "questionList": [
          "9514fd5371304aecab779d56bd155bde",
          "0a2c4dba5e2e4ad7a854f57ec0fff138",
          "ddb036f989774a02a822cf3a163ff8c1",
          "0e68004054dd442f85c2c56d27eb36cc",
          "2ded8891ed094878ac6924b899088d43",
          "a44a52f9863d4daeab3f1e9e761422e7",
          "84bad31d6c644440b463334a50ec01c3",
          "0921e429776f4684b1d6ee425d946b58",
          "6130074e8d1a43ddab2a6a3f0d9181a6",
          "92b681b96cba429a88d6147d493305a9"
        ]
      }
    ]
  }
  ```
  >|| Name     | Meaning
  >|-| ----------| ---------------------
  >| <b><font color=#FF0000>*</font></b> | searchKey | 複查Key
  >|  <b><font color=#FF0000>*</font></b> |<font color=#66B3FF> questionGroup 試題群組 </font>
  >| | typeCode     | 題型代碼
  >| | questionList | 試題ID


* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-11-03T16:01:29.4886218+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "uid": "110-8a1884be89674dd08e582f8ac13df2d1",
      "name": "110學年度南一第一冊(1-1)"
    }
  }
  ```
  > 身分驗證 (目前只開放學生使用)
  ```json
  {
    "systemCode": "0403",
    "isSuccess": false,
    "systemNow": "2021-10-07T17:57:29.86876+08:00",
    "message": "Identity Not Allowed.",
    "disposal": null,
    "data": null
  }
  ```
  > 建立成功回傳欄位
  >| Name    | Meaning
  >| --------| ---------
  >| uid     | 自主練習UID
  >| name    | 自主練習名稱

---

### **<font color=#66B3FF>[GET] /api/Practice/Query </font>**
> 查詢自主練習

* ### Header
  > 帶入user token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
  >| Name| Meaning
  > ---- | ---------------------
  > year | 查詢年分起始 (預設往回推3年)

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-10-07T17:46:19.2253841+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "eduSubjectMap": [
        {
          "code": "JPC",
          "name": "國中國文"
        }
      ],
      "practice": [
        {
          "uid": "110-fd90505676d84d81b4360c4e3890c5a9",
          "name": "110學年度南一第一冊(3)",
          "eduSubject": "JPC",
          "eduSubjectName": "國中國文",
          "amount": 10,
          "createTime": "2021-10-07T09:43:46.363Z",
          "status": "未測驗",
          "isExam": false,
          "isReport": false,
          "examPath": "",
          "examReport": ""
        }
      ]
    }
  }
  ```

  > | Name | Meaning |
  > | ---- |---------|
  > | eduSubjectMap  | 學制科目選單
  > | <font color=#66B3FF> practice 自主練習 </font>
  > | uid  | UID
  > | name | 名稱
  > | eduSubject | 學制科目
  > | eduSubjectName | 學制科目名稱
  > | amount | 題數
  > | createTime | 建立時間
  > | status | 狀態
  > | isExam | 是否已建立測驗
  > | examPath | 測驗連結
  > | isReport | 是否已完成測驗
  > | examReport | 測驗結果
---

### **<font color=#1AFD9C>[POST] /api/Practice/Start </font>**
> 建立自主練習測驗

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
    "uid": "110-fd90505676d84d81b4360c4e3890c5a9",
    "oneclubToken": "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.  eyJpc3MiOiJodHRwOi8vbXlhY2NvdW50Lm5hbmkuY29vbC8iLCJzdWIiOiJ1c2Vycy90aWt1YWR taW4iLCJmcm9tIjoiTmFuaSIsInVzZXJuYW1lIjoidGlrdWFkbWluIiwiZW1haWx2YWxpZCI6dH  J1ZSwibW9iaWxldmFsaWQiOmZhbHNlLCJlbWFpbCI6Im5hbmkudGlrdTE2ODBAZ21haWwuY29tI iwidWlkIjoiZGVlN2Q5MDAtODE4Mi0xMWViLTgzOTUtZTdjZmJiMTE1MmYxIiwianRpIjoiYjg1  NzRhOGEtOGQ0MS00MjYwLWI3ZmMtMmY1ZGY1Yzk1NmQ5IiwiaWF0IjoxNjMwOTg0OTA4LCJleHA iOjE2MzYxNjg5MDh9.F9kt_eLy4liiSBGuBWD2gVfc861FkBBsAPj6kQBPaXk"
  }
  ```
  >| Name| Meaning
  > ---- | ---------------------
  > uid  | 自主練習UID
  > oneclubToken | oneclubToken

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-11-02T19:51:57.3108925+08:00",
    "message": "",
    "disposal": null,
    "data": "https://oneexam-dev.oneclass.com.tw/user/answer/Y0eRqzKd2pXQ6IgDBnj6/tikuadmin"
  }
  ```

  > | Name | Meaning |
  > | ---- |---------|
  > | data | 測驗路徑
---