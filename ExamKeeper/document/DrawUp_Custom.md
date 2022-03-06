# **Draw Up Exam Paper**
## * <a href="https://onepaper-api-dev.oneclass.com.tw/swagger/index.html">swagger 測試路徑</a> *

### ※ 出卷方式：手動出卷 ※
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
            "108": [
              {
                "code": "B01",
                "name": "第一冊"
              }
            ]
          },
          "chapterMap": {
            "B01": {
              "bookID": "110N-ELIB01",
              "bookName": "[110]國小生活(1)南一版章節",
              "chapters": [
                {
                  "hierarchy": 1,
                  "code": "1",
                  "name": "我上一年級了",
                  "parentCode": null,
                  "hasKnowledge": false,
                  "knowledgeList": null,
                  "desc": "[1] 我上一年級了",
                  "amount": 0
                },
                {
                  "hierarchy": 2,
                  "code": "1-1",
                  "name": "上學了",
                  "parentCode": "1",
                  "hasKnowledge": true,
                  "knowledgeList": [
                    {
                      "itemCode": "ELIA-1-4-1",
                      "itemName": "〈我的朋友在哪裡〉"
                    },
                    {
                      "itemCode": "ELIA-1-3-2",
                      "itemName": "粉蠟筆"
                    }
                  ],
                  "desc": "[1-1] 上學了"
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
  > sourceMap      | 出處選單
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
    "year": "110",
    "education": "E",
    "subject": "LI",
    "bookIDs": [ "110N-ELIB01" ],
    "keys": [ "ELIS-1-1-8", "ELIS-1-1-14"],
    "source": [ "NS016" ]
  }
  ```
  >| Name     | Meaning
  > --------- | ---------------------
  > pattern   | 出卷方式
  > year      | 學年度 (產生試卷資料使用)
  > education | 學制代碼
  > subject   | 科目代碼
  > bookIDs   | 冊次代碼 (產生試卷資料使用)
  > keys      | 查詢Key (知識向度代碼)
  > source    | 出處代碼

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-11-19T16:48:38.4288633+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "searchKey": "7fa5abcea0e64cc281cc0cd6ddb13409",
      "filter": [
        {
          "code": "QUES_TYPE",
          "name": "題型",
          "map": [
            {
              "code": "YN",
              "name": "是非題",
              "subMap": [
                {
                  "code": "YN002",
                  "name": "是非題"
                }
              ]
            }
          ]
        },
        {
          "code": "SOURCE",
          "name": "出處",
          "map": [
            {
              "code": "NS016",
              "name": "輔助教材",
              "subMap": [
                {
                  "code": "123",
                  "name": "輔材"
                }
              ]
            }
          ]
        },
        {
          "code": "KNOWLEDGE",
          "name": "知識向度",
          "map": [
            {
              "code": "ELIS-1-1-14",
              "name": "遊玩的注意事項"
            }
          ]
        },
        {
          "code": "LEARN_CONTENT",
          "name": "學習內容",
          "map": [
            {
              "code": "E-I-3",
              "name": "自我行為的檢視與調整。"
            },
            {
              "code": "E-I-2",
              "name": "生活規範的實踐。"
            }
          ]
        },
        {
          "code": "DIFFICULTY",
          "name": "難易度",
          "map": [
            {
              "code": "BEGIN",
              "name": "易"
            },
            {
              "code": "BASIC",
              "name": "中偏易"
            },
            {
              "code": "INTERMEDIATE",
              "name": "中"
            }
          ]
        }
      ],
      "questions": [
        {
          "uid": "ec3922460fc64382ad8e7f098b021600",
          "image": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FELI%2Fec3922460fc64382ad8e7f098b021600%2Fec3922460fc64382ad8e7f098b021600.gif?alt=media&token=7a631d2d-a133-408d-a7eb-03d459eecd27",
          "content": "溜滑梯時，要坐著溜下來，不可以趴著或躺著溜下來。",
          "answer": "○",
          "metadata": [
            {
              "code": "ANSWER_TYPE",
              "name": "作答方式",
              "content": [
                {
                  "code": "YN",
                  "name": "是非題"
                }
              ]
            },
            {
              "code": "QUES_TYPE",
              "name": "題型",
              "content": [
                {
                  "code": "YN002",
                  "name": "是非題"
                }
              ]
            },
            {
              "code": "DIFFICULTY",
              "name": "難易度",
              "content": [
                {
                  "code": "INTERMEDIATE",
                  "name": "中"
                }
              ]
            },
            {
              "code": "ANSWER",
              "name": "答案數",
              "content": [
                {
                  "code": "1",
                  "name": "1"
                }
              ]
            },
            {
              "code": "COGNITIVE",
              "name": "認知歷程向度",
              "content": [
                {
                  "code": "REALIZE",
                  "name": "了解"
                }
              ]
            },
            {
              "code": "ABILITY",
              "name": "能力指標",
              "content": [
                {
                  "code": "3-3",
                  "name": "3-3"
                }
              ]
            },
            {
              "code": "SUB_SOURCE",
              "name": "來源",
              "content": [
                {
                  "code": "123",
                  "name": "輔材"
                }
              ]
            },
            {
              "code": "SOURCE",
              "name": "出處",
              "content": [
                {
                  "code": "NS016",
                  "name": "輔助教材"
                }
              ]
            },
            {
              "code": "CORE_ACCOMPLISHMENT",
              "name": "核心素養",
              "content": [
                {
                  "code": "生活-E-C1",
                  "name": "覺察自己、他人和環境的關係，體會生活禮儀與團體規範的意義，學習尊重他人、愛護生活環境及關懷生命，並於生活中實踐，同時能省思自己在團體中所應扮演的角色，在能力所及或與他人合作的情況下，為改善事情而努力或採取改進行動。"
                }
              ]
            },
            {
              "code": "PERFORMANCE",
              "name": "學習表現",
              "content": [
                {
                  "code": "6-I-1",
                  "name": "覺察自己可能對生活中的人、事、物產生影響，學習調整情緒與行為。"
                }
              ]
            },
            {
              "code": "LEARN_CONTENT",
              "name": "學習內容",
              "content": [
                {
                  "code": "E-I-3",
                  "name": "自我行為的檢視與調整。"
                }
              ]
            },
            {
              "code": "KNOWLEDGE",
              "name": "知識向度",
              "content": [
                {
                  "code": "ELIS-1-1-14",
                  "name": "遊玩的注意事項"
                }
              ]
            },
            {
              "code": "TOPIC",
              "name": "主題",
              "content": [
                {
                  "code": "ELIS-1",
                  "name": "社會"
                }
              ]
            }
          ]
        }
      ]
    }
  }
  ```

  > 無符合範圍試題
  ```json
   {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-11-19T17:33:45.4068888+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "searchKey": "089ec0d8d8b24d94b56fa1e92e085153",
      "filter": null,
      "questions": []
    }
  }
  ```

  
> 欄位說明
>| Name      | Meaning | Remark
> ---------- | ------- | ------ |
> searchKey  | 複查Key
> filter     | 進階篩選資訊 | 題型、出處為兩層結構
> | <font color=#66B3FF> questions </font> | <font color=#66B3FF>  試題明細 </font>
> uid     | 試題ID
> image   | 完整圖檔
> content | 試題內容(純文字)
> answer  | 答案(純文字)
> | <font color=#66B3FF> metadata </font> | <font color=#66B3FF>  試題屬性 </font>
> code    | 屬性代碼
> name    | 屬性名稱
> content | 屬性內容
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


### **<font color=#66B3FF>[GET] /api/{pattern}/QuestionInfo</font>**
> 取得查詢試題明細

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