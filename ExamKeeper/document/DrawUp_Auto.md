# **Draw Up Exam Paper**
## * <a href="https://onepaper-api-dev.oneclass.com.tw/swagger/index.html">swagger 測試路徑</a> *

### ※ 出卷方式：智能命題 ※
---

### **<font color=#66B3FF>[GET] /api/{pattern}/Selection </font>**
> 取得範圍篩選資料

* ### Header
  > 帶入user token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
  >| Name     | Meaning
  > --------- | ---------------------
  > pattern   | 出卷方式
  > year      | 學年度
  > education | 學制代碼
  > subject   | 科目代碼

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-08-27T17:50:24.8293937+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "versionMap": [
        {
          "code": "N",
          "name": "南一"
        }
      ],
      "curriculumMap": [
        {
          "code": "99",
          "name": "99課綱"
        },
        {
          "code": "108",
          "name": "108課綱"
        }
      ],
      "yearMap": [
        {
          "code": "109",
          "name": "109學年度"
        },
        {
          "code": "110",
          "name": "110學年度"
        }
      ],
      "textbookMap": {
        "N": {
          "bookMap": {
            "99": [
              {
                "code": "B05",
                "name": "第五冊"
              },
              {
                "code": "B06",
                "name": "第六冊"
              }
            ],
            "108": [
              {
                "code": "B01",
                "name": "第一冊"
              }
            ]
          },
          "chapterMap": {
            "B01": {
              "bookID": "109N-JGEB01",
              "bookName": "[109]國中地理(1)南一版章節",
              "chapters": [
                {
                  "hierarchy": 1,
                  "code": "1",
                  "name": "地圖與座標系統",
                  "parentCode": null,
                  "hasKnowledge": false,
                  "knowledgeList": null,
                  "desc": "[1] 地圖與座標系統"
                },
                {
                  "hierarchy": 2,
                  "code": "1-1",
                  "name": "如何使用地圖？",
                  "parentCode": "1",
                  "hasKnowledge": true,
                  "knowledgeList": [
                    {
                      "itemCode": "JGE-1-1-1",
                      "itemName": "地圖要素"
                    },
                    {
                      "itemCode": "JGE-1-1-3",
                      "itemName": "比例尺"
                    },
                    {
                      "itemCode": "JGE-1-1-2",
                      "itemName": "地圖類型"
                    }
                  ],
                  "desc": "[1-1] 如何使用地圖？"
                }
              ]
            }
          }
        }
      }
    }
  }
  ```

  > 欄位說明
  >| Name      | Meaning 
  > ---------- | ------- 
  > | <font color=#66B3FF> selection </font>
  > curriculumMap  | 課綱選單
  > yearMap        | 年度選單
  > versionMap     | 版本選單
  > | <font color=#66B3FF> textbookMap </font> | <font color=#66B3FF> 版本冊次選單 </font>
  > | .key     | 版本代碼
  > | .bookMap | 冊次選單
  > | <font color=#66B3FF> .chapterMap 課本章節 </font>
  > bookID	   | 課本ID
  > bookName   | 課本名稱
  > | <font color=#66B3FF> chapters 章節資料 </font>
  > hierarchy  | 階層
  > code       | 代碼
  > name       | 名稱
  > desc       | 簡述
  > parentCode | 父層代碼
  > hasKnowledge  | 是否有知識向度
  > knowledgeList | 知識向度列表
  >|
---

### **<font color=#1AFD9C>[POST] /api/{pattern}/CacheQuery </font>**
> 查詢試題並寫入暫存

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
    "year": "109",
    "education": "J",
    "subject": "GE",
    "bookIDs": [
      "109N-JGEB01"
    ],
    "keys": [
      "JGE-4-1-1","JGE-4-1-2"
    ],
    "filterUsed": true
  }
  ```
  >| Name     | Meaning
  > --------- | ---------------------
  > pattern   | 出卷方式
  > year      | 學年度
  > education | 學制代碼
  > subject   | 科目代碼
  > bookIDs   | 冊次代碼
  > keys      | 查詢Key (知識向度代碼)
  > filterUsed| 過濾已使用試題

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-08-27T16:45:26.0185732+08:00",
    "message": "",
    "data": {
      "searchKey": "02fc27e7f7a846878a6f39996b0b9386",
      "questionGroup": {
        "BEGIN": {
          "code": "BEGIN",
          "name": "易",
          "question": [
            {
              "uid": "c1126ca392a645d5ba8e2357459181f5",
              "difficulty": "BEGIN",
              "difficultyName": "易",
              "quesType": "SS017",
              "quesTypeName": "單選題",
              "answerAmount": 1,
              "keys": [
                {
                  "code": "JGE-4-1-2",
                  "name": "氣候的定義"
                }
              ]
            },
            {
              "uid": "e626aa3acef848b2a8d38c363bcd137f",
              "difficulty": "BEGIN",
              "difficultyName": "易",
              "quesType": "SS017",
              "quesTypeName": "單選題",
              "answerAmount": 1,
              "keys": [
                {
                  "code": "JGE-4-1-2",
                  "name": "氣候的定義"
                }
              ]
            }
          ]
        },
        "INTERMEDIATE": {
          "code": "INTERMEDIATE",
          "name": "中",
          "question": [
            {
              "uid": "08330324b4ae466eaebb356eadde77d1",
              "difficulty": "INTERMEDIATE",
              "difficultyName": "中",
              "quesType": "SS017",
              "quesTypeName": "單選題",
              "answerAmount": 1,
              "keys": [
                {
                  "code": "JGE-4-1-2",
                  "name": "氣候的定義"
                }
              ]
            },
            {
              "uid": "113292b4258e4923badff7fec4a630d2",
              "difficulty": "BASIC",
              "difficultyName": "中偏易",
              "quesType": "SS017",
              "quesTypeName": "單選題",
              "answerAmount": 1,
              "keys": [
                {
                  "code": "JGE-4-1-1",
                  "name": "天氣的定義"
                }
              ]
            }
          ]
        }
      }
    }
  }
  ```

  > 無符合範圍試題
  ```json
  {
    "systemCode": "0501",
    "isSuccess": false,
    "systemNow": "2021-08-27T16:49:39.01539+08:00",
    "message": "No Questions.",
    "disposal": null,
    "data": null
  }
  ```

  > 欄位說明
  >| Name        | Meaning 
  > ------------ | ------- 
  > searchKey     | 複查Key
  > | <font color=#66B3FF> questionGroup 試題群組 </font>
  >  .key        | 難易度分類
  > code         | 難易度代碼
  > name         | 難易度名稱
  > | <font color=#66B3FF> question 試題資訊</font>
  > uid           | UID
  > difficulty    | 難易度代碼
  > difficultyName| 難易度名稱
  > quesType      | 題型代碼
  > quesTypeName  | 題型名稱
  > answerAmount  | 答案數
  > keys          | 知識向度
  >|
---

### **<font color=#66B3FF>[GET] /api/{pattern}/CacheQuery </font>**
> 重複取得查詢結果

* ### Header
  > 帶入user token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
  >| Name     | Meaning
  > --------- | ---------------------
  > pattern   | 出卷方式
  > searchKey | 複查Key

* ### Response
  *直接回傳* "**<font color=#1AFD9C>[POST] /api/{pattern}/CacheQuery </font>**" *的Response*
---