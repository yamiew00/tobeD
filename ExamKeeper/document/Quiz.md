# **Quiz**

> Swagger測試路徑
>
> - [dev](https://onepaper-api-dev.oneclass.com.tw/swagger/index.html) 
> - [uat](https://onepaper-api-uat.oneclass.com.tw/swagger/index.html)



### **<font color=#1AFD9C>[POST] /api/Quiz/Create </font>**

> 新增一項測驗。
>
> 1. 同一試卷可以新增多個測驗

* ### Header
  > 帶入onePaper token
  ```json
  {
    "Authorization": "<token-onePaper>"
  }
  ```
  
* ### Payload
  ```json
  {
      "jwtToken":"{{token-jwt}}",
      "quizName":"jerry_test_2",
      "startTime":"2022-02-23T11:12:28.201Z",
      "endTime":"2022-02-24T11:12:28.201Z",
      "examID": "110-9171900eb8e44faba0e3347c2ca3d809",
      "duration":30,
      "isAutoCheck":true,
      "educationCode":"J",
      "gradeCode":"G03"
  }
  ```
  >| 必要欄位                            | Name          | Meaning                      |
  >| ----------------------------------- | ------------- | ---------------------------- |
  >| <b><font color=#FF0000>*</font></b> | jwtToken      | oneClub token                |
  >| <b><font color=#FF0000>*</font></b> | quizName      | 測驗名稱                     |
  >| <b><font color=#FF0000>*</font></b> | startTime     | 測驗開始時間                 |
  >| <b><font color=#FF0000>*</font></b> | endTime       | 測驗結束時間                 |
  >| <b><font color=#FF0000>*</font></b> | examID        | 考卷ID(必須是isPublic的試卷) |
  >| <b><font color=#FF0000>*</font></b> | duration      | 作答時間                     |
  >| <b><font color=#FF0000>*</font></b> | isAutoCheck   | 是否馬上批改                 |
  >| <b><font color=#FF0000>*</font></b> | educationCode | 受測者學制代碼(E.J.H)        |
  >| <b><font color=#FF0000>*</font></b> | gradeCode     | 受測者年級代碼(G01.G02...)   |
  
* ### Response
  
  成功:
  
  ```json
  //回傳新增成功的Quiz
  {
      "systemCode": "0200",
      "isSuccess": true,
      "systemNow": "2022-02-25T16:38:11.3335757+08:00",
      "diposal": "",
      "data": {
          "quizUID": "f64a3a37-e6c7-4f29-b25f-a91c27ba3496",
          "quizCode": 10872067,
          "userUID": "684d9efc-7248-43af-a25e-31929cefbf3d",
          "quizName": "jerry_test_2",
          "startTime": "2022-02-23T11:12:28.201Z",
          "endTime": "2022-02-24T11:12:28.201Z",
          "education": "J",
          "subject": "EN",
          "grade": "G03"
      }
  }
  ```
  
   > 欄位說明
   > | Name | Meaning |
   > | ---- | ------- |
   > |      |         |
  
  失敗:
  
  ```
  //遺漏必填欄位
  {
      "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
      "title": "One or more validation errors occurred.",
      "status": 400,
      "traceId": "|2b34223-4c647e16d0312786.",
      "errors": {
          "ExamID": [
              "The ExamID property must not be empty"
          ],
          "StartTime": [
              "The StartTime property must not be empty"
          ],
          "EducationCode": [
              "The EducationCode property must not be empty",
              "The EducationCode property code not matched."
          ]
      }
  }
  ```
  
  ```
  //代碼不符
  {
      "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
      "title": "One or more validation errors occurred.",
      "status": 400,
      "traceId": "|2b34225-4c647e16d0312786.",
      "errors": {
          "GradeCode": [
              "The GradeCode property code not matched."
          ]
      }
  }
  ```
  
  ```
  //jwtToken不正確
  {
      "systemCode": "0400",
      "isSuccess": false,
      "systemNow": "2022-02-25T09:16:32.2057311+08:00",
      "diposal": "",
      "data": "Error converting value \"Error:token查無使用者Id\" to type 'ExamKeeper.Controllers.ResponseObject.QuizObject.QuizObjectDerivatives.OneExamQuizContent'. Path 'content', line 1, position 48."
  }
  ```
  
  ```
  //試卷的isPublic為false
  {
      "systemCode": "0400",
      "isSuccess": false,
      "systemNow": "2022-02-25T09:21:45.6949181+08:00",
      "diposal": "",
      "data": "Error converting value \"Error:[必須輸入opt code]\r\n\" to type 'ExamKeeper.Controllers.ResponseObject.QuizObject.QuizObjectDerivatives.OneExamQuizContent'. Path 'content', line 1, position 54."
  }
  ```
  
  

- ### Caution

> 1. Response回傳格式尚未統一
> 2. startTime、endTime未做日期檢驗(只要是datetime就允許存入資料庫)
> 3. duration未做範圍檢驗(即使是負值，只要是integer都通過)
> 4. educationCode、gradeCode只吃代碼。無法使用「國中」、「一年級」。
> 4. 





### **<font color=#1AFD9C>[POST] /api/Quiz/List </font>**

> 取得使用者建立過的所有測驗。
>

* ### Header

  > 帶入onePaper token

  ```json
  {
    "Authorization": "<token-onePaper>"
  }
  ```

* ### Payload

  ```json
  {
  }
  ```

  >| 必要欄位 | Name | Meaning |
  >| -------- | ---- | ------- |

* ### Response

  成功:

  ```
  //兩筆資料
  {
      "systemCode": "0200",
      "isSuccess": true,
      "systemNow": "2022-03-03T17:30:05.955807+08:00",
      "diposal": "",
      "data": [
          {
              "quizUID": "61aa4f53-bcc5-458d-8bf4-baeaf2eb7f46",
              "examUID": "110-64d49ceb6a0a44979e339014010521d9",
              "examName": "110學年度國中數學測驗",
              "quizCode": 78665587,
              "userUID": "ae2788c3-8b1f-452e-8110-034cb37cef87",
              "quizName": "jerry_test_20220301",
              "startTime": "2022-02-28T11:12:28.201Z",
              "endTime": "2022-03-02T11:12:28.201Z",
              "duration": 30,
              "education": "J",
              "subject": "MA",
              "grade": "G01",
              "eduSubjectName": "國中數學"
          },
          {
              "quizUID": "17238593-d666-43aa-9548-b042beeb8ed0",
              "examUID": "110-64d49ceb6a0a44979e339014010521d9",
              "examName": "110學年度國中數學測驗",
              "quizCode": 25126619,
              "userUID": "ae2788c3-8b1f-452e-8110-034cb37cef87",
              "quizName": "jerry_test_20220301",
              "startTime": "2022-02-28T11:12:28.201Z",
              "endTime": "2022-03-02T11:12:28.201Z",
              "duration": 30,
              "education": "J",
              "subject": "MA",
              "grade": "G01",
              "eduSubjectName": "國中數學"
          }
      ]
  }
  ```
  
  ```
  //沒有建立過測驗的使用者
  {
      "systemCode": "0200",
      "isSuccess": true,
      "systemNow": "2022-02-25T16:27:48.9821015+08:00",
      "diposal": "",
      "data": []
  }
  ```
  
  | Name           | Meaning      |
  | -------------- | ------------ |
  | quizUID        | 測驗UID      |
  | examUID        | 考卷UID      |
  | examName       | 考卷名稱     |
  | quizCode       | 測驗碼       |
  | userUID        | 使用者UID    |
  | quizName       | 測驗名稱     |
  | startTime      | 測驗開始時間 |
  | endTime        | 測驗結束時間 |
  | duration       | 作答時間     |
  | education      | 學制         |
  | subject        | 科目         |
  | grade          | 年級         |
  | eduSubjectName | 學制科目     |

### **<font color=#1AFD9C>[POST] /api/Quiz/Result </font>**

> 匯出測驗的作答名單

* ### Header

> 帶入onePaper token

```
{
  "Authorization": "<token-onePaper>"
}
```

* ### Payload

  ```json
  {
      "jwtToken": "{{token-jwt}}",
      "quizUID": "{{quizUID}}"
  }
  ```

  >| 必要欄位                            | Name     | Meaning       |
  >| ----------------------------------- | -------- | ------------- |
  >| <b><font color=#FF0000>*</font></b> | jwtToken | oneClub token |
  >| <b><font color=#FF0000>*</font></b> | quizUID  | 測驗的UID     |

* ### Response

成功:

```
//回傳匯出結果(輪詢中結果有變動)
{
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2022-03-02T09:10:42.4337706+08:00",
    "diposal": "",
    "data": {
        "isUpdate": true,
        "csvFileUrl": "https://firebasestorage.googleapis.com/v0/b/onepaper-dev.appspot.com/o/Uncategorized%2Fjerry_test_20220301_%E4%BD%9C%E7%AD%949%E4%BA%BA.csv?alt=media&token=6e0f7fd9-91e2-4a40-8a85-5e99e95b3b6d",
        "completedNumber": 9
    }
}
```

```
//回傳匯出結果(輪詢中結果無變動)
{
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2022-03-02T09:11:08.6696718+08:00",
    "diposal": "",
    "data": {
        "isUpdate": false,
        "csvFileUrl": "https://firebasestorage.googleapis.com/v0/b/onepaper-dev.appspot.com/o/Uncategorized%2Fjerry_test_20220301_%E4%BD%9C%E7%AD%949%E4%BA%BA.csv?alt=media&token=6e0f7fd9-91e2-4a40-8a85-5e99e95b3b6d",
        "completedNumber": 9
    }
}
```

| Name            | Meaning                              |
| --------------- | ------------------------------------ |
| isUpdate        | 以下兩欄資訊，自上次詢問後是否已更新 |
| csvFileUrl      | csv檔路徑                            |
| completedNumber | 已交卷人數                           |

---

```
//失敗
{
    "systemCode": "0402",
    "isSuccess": false,
    "systemNow": "2022-03-02T09:17:04.2996758+08:00",
    "data": "查無此測驗"
}
```