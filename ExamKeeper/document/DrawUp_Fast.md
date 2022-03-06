# **Draw Up Exam Paper**
## * <a href="https://onepaper-api-dev.oneclass.com.tw/swagger/index.html">swagger 測試路徑</a> *

### ※ 出卷方式：電腦出卷 (快速出卷) ※
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
    "systemNow": "2021-09-02T19:36:36.2974516+08:00",
    "message": "",
    "disposal": null,
    "data": {
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
      "versionMap": [
        {
          "code": "N",
          "name": "南一"
        }
      ],
      "sourceMap": [
        {
          "code": "NS019",
          "name": "精選試題"
        }
      ],
      "textbookMap": {
        "N": {
          "bookMap": {
            "108": [
              {
                "code": "B01",
                "name": "第一冊"
              },
              {
                "code": "B02",
                "name": "第二冊"
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
    "year": "109",
    "education": "J",
    "subject": "CT",
    "bookIDs": [
      "109N-JCTB03"
    ],
    "keys": [
      "JCT-17-2-1",
      "JCT-16-2-2"
    ],
    "source": ["NS017","NS019"]
  }
  ```
  >| Name     | Meaning
  > --------- | ---------------------
  > pattern   | 出卷方式
  > education | 學制代碼
  > subject   | 科目代碼
  > year      | 學年度 (產生試卷資料使用)
  > bookIDs   | 冊次代碼 (產生試卷資料使用)
  > keys      | 查詢Key (知識向度代碼)
  > source    | 出處代碼

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-07-22T18:13:27.3001868+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "searchKey": "f9ee86270c0642c197aebf1a226a23c1",
      "questionGroup": {
        "單選題": {
          "code": "SS017",
          "name": "單選題",
          "question": [
            {
              "uid": "D3BM8KBnf0yZm5WS1w7DOQiP",
              "difficulty": "BEGIN",
              "quesType": "SS017",
              "quesTypeName": "單選題",
              "answerAmount": 1
            },
            {
              "uid": "jxgMaoqgf0uzbCLjOr2jvA0o",
              "difficulty": "BEGIN",
              "quesType": "SS017",
              "quesTypeName": "單選題",
              "answerAmount": 0
            }
          ],
          "count": {
            "question": 72,
            "answer": 24,
            "difficulty": {
              "BEGIN": {
                "question": 72,
                "answer": 24
              }
            }
          },
        }
      }
    }
  }
  ```

  > 無符合範圍試題
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-08-13T09:34:34.4401147+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "searchKey": "6911b6f5c6964aabbda532be940278ba",
      "questionGroup": {}
    }
  }
  ```

  > 欄位說明
  >| Name        | Meaning 
  > ------------ | ------- 
  > searchKey     | 複查Key
  > | <font color=#66B3FF> questionGroup 試題群組 </font>
  >  .key        | 題型
  > code         | 題型代碼
  > name         | 題型名稱
  > | <font color=#66B3FF> question 試題資訊</font>
  > uid          | UID
  > difficulty   | 難易度代碼
  > quesType     | 題型代碼
  > quesTypeName | 題型名稱
  > answerAmount | 答案數
  > | <font color=#66B3FF> count 資料統計</font>
  > question     | 題數總計
  > answer       | 答案數總計
  > difficulty	 | 難易度題數/答數總計
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
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-09-24T12:33:39.8553332+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "searchKey": "224056d8f76949ae9609d2cf9c762cdf",
      "filter": [
        {
          "code": "KNOWLEDGE",
          "name": "知識向度",
          "map": [
            {
              "code": "JGE-29-1-2",
              "name": "臺灣的相對位置"
            },
            {
              "code": "JGE-29-1-3",
              "name": "臺灣的絕對位置"
            }
          ]
        },
        {
          "code": "LEARN_CONTENT",
          "name": "學習內容",
          "map": [
            {
              "code": "地AA-IV-3",
              "name": "臺灣地理位置的特性及其影響。"
            },
            {
              "code": "地AA-IV-1",
              "name": "全球經緯度座標系統。"
            }
          ]
        },
        {
          "code": "DIFFICULTY",
          "name": "難易度",
          "map": [
            {
              "code": "BASIC",
              "name": "中偏易"
            },
            {
              "code": "BEGIN",
              "name": "易"
            },
            {
              "code": "INTERMEDIATE",
              "name": "中"
            },
            {
              "code": "EXPERT",
              "name": "難"
            }
          ]
        },
        {
          "code": "QUES_TYPE",
          "name": "題型",
          "map": [
            {
              "subMap": [
                {
                  "code": "SS017",
                  "name": "單選題"
                }
              ],
              "code": "SS",
              "name": "單一選擇題"
            }
          ]
        },
        {
          "code": "SOURCE",
          "name": "出處",
          "map": [
            {
              "subMap": [
                {
                  "code": "102",
                  "name": "習作"
                }
              ],
              "code": "NS003",
              "name": "習作"
            },
            {
              "subMap": [
                {
                  "code": "121",
                  "name": "精選試題"
                }
              ],
              "code": "NS019",
              "name": "精選試題"
            },
            {
              "subMap": [
                {
                  "code": "123",
                  "name": "輔材"
                }
              ],
              "code": "NS016",
              "name": "輔助教材"
            }
          ]
        }
      ],
      "questions": [
        {
          "uid": "9db5a68bbe8a462f887cb4fa43ca350d",
          "image": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.  appspot.com/o/  question-bank%2FJGE%2F9db5a68bbe8a462f887cb4fa43ca350d%2F9db5a68bbe8a462  f887cb4fa43ca350d.gif?alt=media&  token=e1a51129-fb9f-46c6-a502-2c6abe996981",
          "content": "過去的歷史中，荷蘭與西班牙都曾先後占領臺灣，請問他們所考量的因素  為何？　(A)臺灣文明先進　(B)臺灣天然資源豐富　(C)臺灣原住民個性溫和　(D)臺灣  位於許多航道的必經之處",
          "answer": "(D)",
          "metadata": [
            {
              "code": "ANSWER_TYPE",
              "name": "作答方式",
              "content": [
                {
                  "code": "SS",
                  "name": "單一選擇題"
                }
              ]
            },
            {
              "code": "QUES_TYPE",
              "name": "題型",
              "content": [
                {
                  "code": "SS017",
                  "name": "單選題"
                }
              ]
            },
            {
              "code": "DIFFICULTY",
              "name": "難易度",
              "content": [
                {
                  "code": "BASIC",
                  "name": "中偏易"
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
              "code": "SUB_SOURCE",
              "name": "來源",
              "content": [
                {
                  "code": "102",
                  "name": "習作"
                }
              ]
            },
            {
              "code": "SOURCE",
              "name": "出處",
              "content": [
                {
                  "code": "NS003",
                  "name": "習作"
                }
              ]
            },
            {
              "code": "LEARN_CONTENT",
              "name": "學習內容",
              "content": [
                {
                  "code": "地AA-IV-3",
                  "name": "臺灣地理位置的特性及其影響。"
                }
              ]
            },
            {
              "code": "KNOWLEDGE",
              "name": "知識向度",
              "content": [
                {
                  "code": "JGE-29-1-2",
                  "name": "臺灣的相對位置"
                }
              ]
            },
            {
              "code": "TOPIC",
              "name": "主題",
              "content": [
                {
                  "code": "JGE-29",
                  "name": "世界中的臺灣"
                }
              ]
            },
            {
              "code": "CHAPTER",
              "name": "章節",
              "content": [
                {
                  "code": "2-1",
                  "name": "如何從世界地圖中找到臺灣？"
                }
              ]
            }
          ]
        },
        {
          "uid": "c61c37cffd954fff82648d67754052da",
          "image": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.  appspot.com/o/  question-bank%2FJGE%2Fc61c37cffd954fff82648d67754052da%2Fc61c37cffd954ff  f82648d67754052da.gif?alt=media&  token=2be66adf-c9e2-49b2-b4fb-94746d5ffd08",
          "content": "若以「東、西、南、北」半球來標示位置，則臺灣位於何處？　(A)東半 球、南半球　(B)東半球、北半球　(C)西半球、北半球　(D)西半球、南半球",
          "answer": "(B)",
          "metadata": [
            {
              "code": "ANSWER_TYPE",
              "name": "作答方式",
              "content": [
                {
                  "code": "SS",
                  "name": "單一選擇題"
                }
              ]
            },
            {
              "code": "QUES_TYPE",
              "name": "題型",
              "content": [
                {
                  "code": "SS017",
                  "name": "單選題"
                }
              ]
            },
            {
              "code": "DIFFICULTY",
              "name": "難易度",
              "content": [
                {
                  "code": "BEGIN",
                  "name": "易"
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
                  "code": "MEMORY",
                  "name": "記憶"
                }
              ]
            },
            {
              "code": "ABILITY",
              "name": "能力指標",
              "content": [
                {
                  "code": "1-3-4",
                  "name": "1-3-4"
                }
              ]
            },
            {
              "code": "SUB_SOURCE",
              "name": "來源",
              "content": [
                {
                  "code": "121",
                  "name": "精選試題"
                }
              ]
            },
            {
              "code": "SOURCE",
              "name": "出處",
              "content": [
                {
                  "code": "NS019",
                  "name": "精選試題"
                }
              ]
            },
            {
              "code": "LEARN_CONTENT",
              "name": "學習內容",
              "content": [
                {
                  "code": "地AA-IV-3",
                  "name": "臺灣地理位置的特性及其影響。"
                }
              ]
            },
            {
              "code": "KNOWLEDGE",
              "name": "知識向度",
              "content": [
                {
                  "code": "JGE-29-1-3",
                  "name": "臺灣的絕對位置"
                }
              ]
            },
            {
              "code": "TOPIC",
              "name": "主題",
              "content": [
                {
                  "code": "JGE-29",
                  "name": "世界中的臺灣"
                }
              ]
            },
            {
              "code": "CHAPTER",
              "name": "章節",
              "content": [
                {
                  "code": "2-1",
                  "name": "如何從世界地圖中找到臺灣？"
                }
              ]
            }
          ]
        }
      ],
      "bookChapters": [
        {
          "bookID": "110N-JGEB01",
          "bookName": "[110]國中地理(1)南一版章節",
          "amount": 69,
          "chapters": [
            {
              "hierarchy": 1,
              "code": "1",
              "name": "地圖與座標系統",
              "parentCode": null,
              "hasKnowledge": false,
              "knowledgeList": null,
              "desc": "[1] 地圖與座標系統",
              "amount": 29
            },
            {
              "hierarchy": 2,
              "code": "1-1",
              "name": "如何使用地圖？",
              "parentCode": "1",
              "hasKnowledge": true,
              "knowledgeList": [
                {
                  "itemCode": "JGE-1-1-2",
                  "itemName": "地圖類型"
                },
                {
                  "itemCode": "JGE-1-1-1",
                  "itemName": "地圖要素"
                },
                {
                  "itemCode": "JGE-1-1-3",
                  "itemName": "比例尺"
                }
              ],
              "desc": "[1-1] 如何使用地圖？",
              "amount": 29
            },
            {
              "hierarchy": 2,
              "code": "1-2",
              "name": "如何表示位置？",
              "parentCode": "1",
              "hasKnowledge": true,
              "knowledgeList": [
                {
                  "itemCode": "JGE-1-2-2",
                  "itemName": "絕對位置"
                },
                {
                  "itemCode": "JGE-1-2-1",
                  "itemName": "相對位置"
                }
              ],
              "desc": "[1-2] 如何表示位置？",
              "amount": 0
            },
            {
              "hierarchy": 2,
              "code": "1-3",
              "name": "如何用全球經緯度座標系統定位？",
              "parentCode": "1",
              "hasKnowledge": true,
              "knowledgeList": [
                {
                  "itemCode": "JGE-1-3-1",
                  "itemName": "經線與緯線"
                }
              ],
              "desc": "[1-3] 如何用全球經緯度座標系統定位？",
              "amount": 0
            },
            {
              "hierarchy": 1,
              "code": "2",
              "name": "世界中的臺灣",
              "parentCode": null,
              "hasKnowledge": false,
              "knowledgeList": null,
              "desc": "[2] 世界中的臺灣",
              "amount": 40
            },
            {
              "hierarchy": 2,
              "code": "2-1",
              "name": "如何從世界地圖中找到臺灣？",
              "parentCode": "2",
              "hasKnowledge": true,
              "knowledgeList": [
                {
                  "itemCode": "JGE-29-1-3",
                  "itemName": "臺灣的絕對位置"
                },
                {
                  "itemCode": "JGE-29-1-1",
                  "itemName": "全球海陸概述"
                },
                {
                  "itemCode": "JGE-29-1-2",
                  "itemName": "臺灣的相對位置"
                }
              ],
              "desc": "[2-1] 如何從世界地圖中找到臺灣？",
              "amount": 40
            }
          ]
        },
        {
          "bookID": "110N-JGEB03",
          "bookName": "[110]國中地理(3)南一版章節",
          "amount": 0,
          "chapters": [
            {
              "hierarchy": 1,
              "code": "1",
              "name": "中國的人口分布與自然環境",
              "parentCode": null,
              "hasKnowledge": false,
              "knowledgeList": null,
              "desc": "[1] 中國的人口分布與自然環境",
              "amount": 0
            }
          ]
        }
      ]
    }
  }
  ```

> 欄位說明
>| Name      | Meaning | Remark
> ---------- | ------- | ------ |
> searchKey  | 複查Key
> filter     | 進階篩選資訊 | 題型、出處為兩層結構
> | <font color=#66B3FF> bookChapters </font> | <font color=#66B3FF>  課本章節 </font>
> bookID  | 課本代碼
> bookName| 課本名稱
> amount  | 試題統計
> chapters| 章節資訊
> | <font color=#66B3FF> .chapters </font> | <font color=#66B3FF>  課本章節 </font>
> hierarchy  | 階層
> code       | 代碼
> name       | 名稱
> desc       | 簡述
> parentCode | 父層代碼
> hasKnowledge  | 是否有知識向度
> knowledgeList | 知識向度列表
> amount  | 試題統計
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