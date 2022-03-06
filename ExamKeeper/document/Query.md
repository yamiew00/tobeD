# **Query**

> Swagger測試路徑
>
> - [dev](https://onepaper-api-dev.oneclass.com.tw/swagger/index.html) 
> - [uat](https://onepaper-api-uat.oneclass.com.tw/swagger/index.html)



### **<font color=#1AFD9C>[POST] /api/Query/GetBook </font>**

> 查詢藏書。
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
      "edusubject":"ELI",
      "publishers":["N"]
  }
  ```
  >| 必要欄位                            | Name       | Meaning                        |
  >| ----------------------------------- | ---------- | ------------------------------ |
  >| <b><font color=#FF0000>*</font></b> | edusubject | 學制科目代碼                   |
  >| <b><font color=#FF0000>*</font></b> | publishers | 出版社代碼，不填則回傳全部內容 |
  
* ### Response
  
  成功(books以外的內容):
  
  ```json
  {
      "systemCode": "0200",
      "isSuccess": true,
      "systemNow": "2022-03-05T15:02:08.3161559+08:00",
      "data":{
          "sourceTable": [
              "素養類題",
              "輔助教材",
              "精選試題"
          ],
          "volumeTable": {
              "B01": "第一冊",
              "B02": "第二冊",
              "B03": "第三冊",
              "B04": "第四冊",
              "B05": "第五冊",
              "B06": "第六冊"
          },
          "result": [{
              "publisher": "翰林",
              "collections": [
                  {
                      "curriculum": "108",
                      "content": [
                          {
                              "year": 109,
                              "books":[ ...
                              ]
                          },
                          {
                              "year": 110,
                              "books":[ ...
                              ]
                          }
                      ]
                  },
                  {
                      "curriculum": "99",
                      "content": [
                          {
                              "year": 109,
                              "books":[ ...
                              ]
                          }
                      ]
                  }
              ]
          },
          {
              "publisher": "康軒",
              ...
  }
  ```
  
   > 欄位說明
   > | Name        | Meaning          |
   > | ----------- | ---------------- |
   > | volumeTable | 冊次對照表       |
   > | publisher   | 出版社           |
   > | collections | 出版社內藏書分類 |
   > | curriculum  | 課綱             |
   > | content     | 課綱內藏書分類   |
   > | year        | 書學年度         |
   > | books       | 書藉             |
   > | sourceTable | 出處表           |
  
  
  
  成功(books內容):
  
  ```
  "books": [
  {
      "bookId": "109H-EMAB01",
      "data": [
          {
              "name": "10以內的數",
              "code": "1",
              "data": [
                  {
                      "name": "1～5的數",
                      "code": "1-1",
                      "knowledges": [
                          {
                              "name": "認識1～5的數",
                              "code": "EMA-5-3-7"
                          }
                      ]
                  },
                  {
                      "name": "6～10的數",
                      "code": "1-2",
                      "knowledges": [
                          {
                              "name": "認識6～10的數",
                              "code": "EMA-5-3-8"
                          }
                      ]
                  },
                  {
                      "name": "點數與對應",
                      "code": "1-3",
                      "knowledges": [
                          {
                              "name": "1～10的做數",
                              "code": "EMA-5-3-1"
                          }
                      ]
                  }
              ]
          },
          {
              "name": "比長短",
              "code": "2",
              "data": [
                  {
                      "name": "比長短",
                      "code": "2-1",
                      "knowledges": [
                          {
                              "name": "曲線的長度比較",
                              "code": "EMA-1-7-1"
                          },
                          {
                              "name": "直線和曲線",
                              "code": "EMA-1-7-2"
                          },
                          {
                              "name": "認識長度",
                              "code": "EMA-4-2-10"
                          },
                          {
                              "name": "長度的直接比較",
                              "code": "EMA-4-2-3"
                          }
                      ]
                  },
  ```
  
  > 欄位說明
  >
  > | Name       | Meaning              |
  > | ---------- | -------------------- |
  > | bookId     | 書本id               |
  > | data       | 章節內容             |
  > | name       | (章節or知識向度)名稱 |
  > | code       | (章節or知識向度)代碼 |
  > | knowledges | 知識向度             |
  >
  > 





### **<font color=#1AFD9C>[POST] /api/Query/GetQuestionType</font>**

> 查詢題目(按題型)。
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
      "eduSubject":"JMA",
      "knowledges":[
        "JMA-1-1-4",
        "JMA-1-1-5",
        "JMA-1-1-2",
        "JMA-1-1-1",
        "JMA-1-1-3",
        "JMA-1-2-2",
        "JMA-1-2-3",
        "JMA-1-2-1",
        "JMA-1-1-6",
        "JMA-1-1-7",
        "JMA-1-2-6"
      ],
      "bookIDs":[
        "110N-JMAB01",
        "110N-JMAB02",
        "110N-JMAB03",
        "110N-JMAB04"
      ],
      "sources":[
        "NS020",
        "NS003",
        "NS004",
        "NS019
      ]
  }
  ```
  >| 必要欄位                            | Name       | Meaning      |
  >| ----------------------------------- | ---------- | ------------ |
  >| <b><font color=#FF0000>*</font></b> | eduSubject | 學制科目代碼 |
  >| <b><font color=#FF0000>*</font></b> | knowledges | 知識點代碼   |
  >| <b><font color=#FF0000>*</font></b> | bookIDs    | 書本id       |
  >| <b><font color=#FF0000>*</font></b> | sources    | 出處代碼     |
  >
  >若是沒填篩選條件， **將會一筆都找不到**。
  
  
  
* ### Response
  
  成功:
  
  ```json
  {
      "systemCode": "0200",
      "isSuccess": true,
      "systemNow": "2022-03-06T11:06:47.9407219+08:00",
      "data": [
          {
              "questionType": "SS017",
              "questionTypeName": "單選題",
              "questions": [
                  {
                      "uid": "b3101795a391499fa649c23616a8c531",
                      "difficulty": "INTERMEDIATE",
                      "answerAmount": 1
                  },
                  {
                      "uid": "d9be031f16d8469a8440d952a10406f3",
                      "difficulty": "INTERMEDIATE",
                      "answerAmount": 1
                  },
                  {
                      "uid": "4994f99230c24ee79e64bee3f16b5392",
                      "difficulty": "INTERMEDIATE",
                      "answerAmount": 1
                  }
              ],
              "diffcultyAggregate": [
                  {
                      "difficulty": "INTERMEDIATE",
                      "question": 3346,
                      "answer": 3346
                  },
                  {
                      "difficulty": "BEGIN",
                      "question": 3897,
                      "answer": 3897
                  },
                  {
                      "difficulty": "EXPERT",
                      "question": 230,
                      "answer": 230
                  }
              ],
              "sum": {
                  "question": 7473,
                  "answer": 7473
              }
          },
          {
              "questionType": "WR111",
              "questionTypeName": "非選題",
              "questions": [
                  {
                      "uid": "eb54db6331e24b1996d1ca62d0f4a7c3",
                      "difficulty": "INTERMEDIATE",
                      "answerAmount": 1
                  },
                  {
                      "uid": "27bc1db8f42c4eeb80e6f71065f57e42",
                      "difficulty": "INTERMEDIATE",
                      "answerAmount": 1
                  },
                  {
                      "uid": "c874c298acec455b975b0c2fa24e50cd",
                      "difficulty": "INTERMEDIATE",
                      "answerAmount": 1
                  }
              ],
              "diffcultyAggregate": [
                  {
                      "difficulty": "INTERMEDIATE",
                      "question": 2797,
                      "answer": 3680
                  },
                  {
                      "difficulty": "EXPERT",
                      "question": 336,
                      "answer": 401
                  },
                  {
                      "difficulty": "BEGIN",
                      "question": 2302,
                      "answer": 3675
                  }
              ],
              "sum": {
                  "question": 5435,
                  "answer": 7756
              }
          },
      ]
  }
  ```
  
   > 欄位說明
   > | Name                                                    | Meaning                                     |
   > | ------------------------------------------------------- | ------------------------------------------- |
   > | questionType                                            | 題型代碼                                    |
   > | questionTypeName                                        | 題型名稱                                    |
   > | <b><font color=#FF0000>[questions] </font></b>          | <b><font color=#FF0000>[題目] </font></b>   |
   > | uid                                                     | 題目uid                                     |
   > | difficulty                                              | 難易度                                      |
   > | answerAmount                                            | 答案數量                                    |
   > | <b><font color=#FF0000>[diffcultyAggregate] </font></b> | <b><font color=#FF0000>[難易度] </font></b> |
   > | difficulty                                              | 難易度                                      |
   > | question                                                | 總題數                                      |
   > | answer                                                  | 答案數                                      |
   > | <b><font color=#FF0000>[sum] </font></b>                | <b><font color=#FF0000>[加總] </font></b>   |
   > | question                                                | 總題數                                      |
   > | answer                                                  | 答案數                                      |
  
  