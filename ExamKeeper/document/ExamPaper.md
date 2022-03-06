# **Exam Paper**
## * <a href="https://onepaper-api-dev.oneclass.com.tw/swagger/index.html">swagger 測試路徑</a> *


### **<font color=#1AFD9C>[POST] /api/Exam/Create </font>**
> 新增試卷

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
    "searchKey": "e1a5fbef41f44271933e5329da67d340",
    "drawUp": "FastPattern",
    "questionGroup": [
      {
        "typeCode": "SS017",
        "scoreType": "PerQuestion",
        "score": 20,
        "questionList": [
          "D3BM8KBnf0yZm5WS1w7DOQiP",
          "MKbpAiXGX0GeBKhVPy0O1QyD",
          "PwNz39ERQEaVnuhsuSIEgAsH",
          "u58x4lazRUuqDxZD1b4ulASY",
          "g7tpeCmYN8kuQxbWM6yiCBwIx"
        ]
      }
    ]
  }
  ```
  > 欄位說明
  > <b><font color=#FF0000>* :Required</font></b><br/>
  > |  | Name        | Meaning |
  > | ---------| ---------   |---------|
  > | <b><font color=#FF0000>*</font></b> | searchKey | 搜尋Key
  > | <b><font color=#FF0000>*</font></b> | drawUp    | 出卷方式
  > | <b><font color=#FF0000>*</font></b> | <font color=#66B3FF> questionGroup 試題資訊 </font>  | 
  > | <b><font color=#FF0000>*</font></b> | typeCode  | 題型代碼
  > | <b><font color=#FF0000>*</font></b> | scoreType | 配分方式
  > | <b><font color=#FF0000>*</font></b> | score     | 配分
  > | <b><font color=#FF0000>*</font></b> | questionList | 試題ID

* ### Response
  > 新增成功:回傳Related資訊
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-07-28T18:30:11.0941047+08:00",
    "message": "",
    "data": {
      "examUID": "109-d251f62042f244a2b876ad879510567f",
      "examType": [
        {
          "code": "General",
          "name": "平時考"
        },
        {
          "code": "Exam",
          "name": "段考"
        }
      ],
      "outputType": [
        {
          "code": "Online",
          "name": "線上測驗"
        },
        {
          "code": "Files",
          "name": "紙本卷類"
        }
      ],
      "defaultName": "109學年度國中公民測驗"
    }
  }
  ```
  > 新增失敗:配分方式有誤
  ```json
  {
    "systemCode": "0400",
    "isSuccess": false,
    "systemNow": "2021-07-28T18:18:01.5982649+08:00",
    "message": "[配分方式資料超出使用範圍]\n",
    "disposal": null,
    "data": null
  }
  ```

  > 新增失敗:試題ID有誤
  ```json
  {
    "systemCode": "0400",
    "isSuccess": false,
    "systemNow": "2021-07-28T18:18:25.9650165+08:00",
    "message": "[試題ID資料超出使用範圍]PwNz39ERQEaVnuhsuSIEgAH",
    "disposal": null,
    "data": null
  }
  ```

  > 欄位說明
  >| Name     | Meaning 
  > --------- | ------- 
  > examUID   | 試卷ID
  > examType  | 試卷考試類別
  > outputType| 輸出方式
  > defaultName| 預設試卷名稱

---

### **<font color=#FFA042>[PUT] /api/Exam/Edit </font>**
> 修改試卷屬性

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
    "examUID": "109-44543dc7a55a43deab72295eea465b6d",
    "name": "系統測試-自行輸入試卷名稱",
    "examType": "General"
  }
  ```
  > 欄位說明
  > <b><font color=#FF0000>* :Required</font></b><br/>
  > |  | Name        | Meaning |
  > | ---------| ---------   |---------|
  > | <b><font color=#FF0000>*</font></b> | examUID   | 試卷UID
  > | <b><font color=#FF0000>*</font></b> | name      | 自訂名稱
  > | <b><font color=#FF0000>*</font></b> | examType  | 考試別
  > | <b><font color=#FF0000>*</font></b> | ~~outputType~~| ~~輸出方式~~ 匯出時才決定匯出方式，故移除資料

* ### Response
  > 修改成功
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-07-29T15:50:34.1799852+08:00",
    "message": "",
    "disposal": null,
    "data": null
  }
  ```
  > 修改失敗
  ```json
  {
    "systemCode": "0400",
    "isSuccess": false,
    "systemNow": "2021-07-29T15:49:12.42611+08:00",
    "message": "[試卷資料超出使用範圍]Not Exam Paper Author.",
    "disposal": null,
    "data": null
  }
  ```
---

### **<font color=#66B3FF>[GET] /api/Exam/Query</font>**
> 查詢個人試卷列表
* ### Header
  > 帶入user token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
  > | Name | Meaning |
  > | ---- |---------|
  > | year | 學年度 (未傳值時會預設帶入當學年度)

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-07-30T17:09:08.9886712+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "yearMap": [
        {
          "code": "110",
          "name": "110學年度"
        },
        {
          "code": "109",
          "name": "109學年度"
        },
        {
          "code": "108",
          "name": "108學年度"
        }
      ],
      "eduSubjectMap": [
        {
          "code": "JCT",
          "name": "國中公民"
        }
      ],
      "typeMap": [
        {
          "code": "General",
          "name": "平時考"
        },
        {
          "code": "Exam",
          "name": "段考"
        },
        {
          "code": "Quiz",
          "name": "隨堂測驗"
        },
        {
          "code": "Others",
          "name": "其他"
        }
      ],
      "patternMap": [
        {
          "code": "FastPattern",
          "name": "電腦命題"
        }
      ],
      "examPaper": [
        {
          "uid": "109-44543dc7a55a43deab72295eea465b6d",
          "name": "系統測試-自行輸入試卷名稱",
          "eduSubjectName": "國中公民",
          "examTypeName": "平時考",
          "drawUpName": "電腦命題",
          "attributes": {
            "questionAmount": 5,
            "isPublic": false,
            "name": "系統測試-自行輸入試卷名稱",
            "examType": "General",
            "drawUp": "FastPattern",
            "year": "109",
            "yearName": "109學年度",
            "education": "J",
            "eduName": "國中",
            "subject": "CT",
            "subjectName": "公民",
            "bookIDs": [
              "109N-JCTB03"
            ],
            "bookNames": [
              "第三冊"
            ]
          },
          "maintainer": "[tikuadmin] 管理者",
          "createTime": "2021-07-29T07:45:08.535Z",
          "updateTime": "2021-07-29T07:45:08.535Z",
          "download": "https://firebasestorage.googleapis.com/v0/b/onepaper-1126a.appspot.com/o/examination%2F9bb170198eb54797bbc340ae6b95c48f.zip?alt=media&token=03d1b998-cbec-494d-8476-266e5b0043b7",
          "downloadName": "2022-01-12-18-22-41國中英語試卷.zip"
        }
      ]
    }
  }
  ```

  > | Name | Meaning |
  > | ---- |---------|
  > | ========== | 篩選條件 |
  > | yearMap | 年度列表 
  > | eduSubjectMap |　學制科目列表
  > | typeMap | 考試別列表
  > | outputMap | 輸出類別 
  > | patternMap | 出題方式
  > | ========== | ========== |
  > | <font color=#66B3FF> examPaper </font> | <font color=#66B3FF> 試卷列表 </font>
  > | uid  | 試卷UID
  > | name | 試卷名稱
  > | eduSubjectName | 學制科目
  > | examTypeName   | 考試別
  > | drawUpName     | 出卷方式
  > | isFavorite     | 是否設為收藏
  > | favorites      | 收藏數統計
  > | <font color=#66B3FF> attribute 基本屬性 </font>
  > | .questionAmount  | 試題總數
  > | .isPublic | 是否為公開卷
  > | .score    | 總分
  > | .examType | 考試別代碼
  > | .drawUp     | 出卷方式代碼
  > | .year       | 學年度代碼
  > | .yearName   | 學年度
  > | .education  | 學制代碼
  > | .eduName    | 學制
  > | .subject    | 科目代碼
  > | .subjectName | 科目
  > | .bookIDs    | 課本代碼
  > | .bookNames  | 課本冊次名稱
  > | .version    | 版本代碼
  > | .versionName| 版本名稱
  > | maintainer  | 出卷者
  > | createTime  | 建立時間
  > | updateTime  | 最後維護時間
  > | download    | 下載路徑
  > | downloadName| 下載名稱
---

### **<font color=#FFA042>[PUT] /api/Exam/Public </font>**
> 試卷公開設定

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
    "examUID": "109-7c428ebc498a430f8b4ede1e4fb6afce",
    "isPublic": true
  }
  ```
  > 欄位說明
  > <b><font color=#FF0000>* :Required</font></b><br/>
  > |  | Name        | Meaning |
  > | ---------| ---------   |---------|
  > | <b><font color=#FF0000>*</font></b> | examUID   | 試卷UID
  > | <b><font color=#FF0000>*</font></b> | isPublic  | 是否公開試卷

* ### Response
  > 修改成功
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-08-02T09:52:34.6226264+08:00",
    "message": "",
    "disposal": null,
    "data": null
  }
  ```
  > 修改失敗
  ```json
  {
    "systemCode": "0400",
    "isSuccess": false,
    "systemNow": "2021-08-02T09:54:48.9283135+08:00",
    "message": "試卷ID無資料",
    "disposal": null,
    "data": null
  }
  ```
---

### **<font color=#66B3FF>[GET] /api/Exam/Public </font>**
> 查詢公開試卷列表
* ### Header
  > 帶入user token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
  > | Name | Meaning |
  > | ---- |---------|
  > | field   | 篩選欄位
  > | content | 篩選內容

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-11-08T11:27:40.2743002+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "yearMap": [
        {
          "code": "110",
          "name": "110學年度"
        },
        {
          "code": "109",
          "name": "109學年度"
        }
      ],
      "eduSubjectMap": [
        {
          "code": "JGE",
          "name": "國中地理"
        },
        {
          "code": "JPY",
          "name": "國中理化"
        },
        {
          "code": "JPC",
          "name": "國中國文"
        }
      ],
      "typeMap": [
        {
          "code": "General",
          "name": "平時考"
        },
        {
          "code": "Exam",
          "name": "段考"
        }
      ],
      "outputMap": [
        {
          "code": "Online",
          "name": "線上測驗"
        },
        {
          "code": "Files",
          "name": "紙本卷類"
        }
      ],
      "patternMap": [
        {
          "code": "FastPattern",
          "name": "電腦命題"
        }
      ],
      "examPaper": [
        {
          "uid": "110-77336f0a2c0342a6b3306e9243c94d98",
          "name": "110學年度國中地理測驗(圖片)",
          "eduSubjectName": "國中地理",
          "examTypeName": "段考",
          "drawUpName": "電腦命題",
          "isFavorite": true,
          "favorites": 1,
          "attributes": {
            "isPublic": true,
            "questionAmount": 20,
            "score": 100,
            "name": "110學年度國中地理測驗(圖片)",
            "examType": "Exam",
            "drawUp": "FastPattern",
            "year": "110",
            "yearName": "110學年度",
            "education": "J",
            "eduName": "國中",
            "subject": "GE",
            "subjectName": "地理",
            "bookIDs": [
              "110N-JGEB01",
              "110N-JGEB03",
              "110N-JGEB05"
            ],
            "bookNames": [
              "第一冊",
              "第三冊",
              "第五冊"
            ],
            "version": "N",
            "versionName": "南一"
          },
          "tags": [
            "liveoneclass"
          ],
          "maintainer": "[tikuadmin] 管理者",
          "createTime": "2021-10-29T01:36:25.342Z",
          "updateTime": "2021-10-29T01:36:39.839Z"
        }
      ]
    }
  }
  ```

  > | Name | Meaning |
  > | ---- |---------|
  > | ========== | 篩選條件 |
  > | yearMap | 年度列表 
  > | eduSubjectMap |　學制科目列表
  > | typeMap | 考試別列表
  > | outputMap | 輸出類別 
  > | patternMap | 出題方式
  > | ========== | ========== |
  > | <font color=#66B3FF> examPaper </font> | <font color=#66B3FF> 試卷列表 </font>
  > | | 與 **<font color=#66B3FF>[GET] /api/Exam/Query</font>**  完全相同
---

### **<font color=#FFA042>[PUT] /api/Exam/Favorite </font>**
> 試卷收藏設定

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
    "examUID": "110-77336f0a2c0342a6b3306e9243c94d98",
    "year": "110",
    "education": "J",
    "subject": "GE",
    "isAdd": true
  }
  ```
  > 欄位說明
  > <b><font color=#FF0000>* :Required</font></b><br/>
  > |  | Name        | Meaning |
  > | ---------| ---------   |---------|
  > | <b><font color=#FF0000>*</font></b> | examUID   | 試卷UID
  > | <b><font color=#FF0000>*</font></b> | year      | 試卷學年度
  > | <b><font color=#FF0000>*</font></b> | education | 學制代碼
  > | <b><font color=#FF0000>*</font></b> | subject   | 科目代碼
  > | <b><font color=#FF0000>*</font></b> | isAdd     | 是否收藏

* ### Response
  > 成功
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-11-08T12:01:00.8868048 +08:00",
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
    "systemNow": "2021-11-08T11:58:13.8197018+08:00",
    "message": "[試卷資料超出使用範圍]Not Exam Paper Author.",
    "disposal": null,
    "data": null
  }
  ```
---

### **<font color=#66B3FF>[GET] /api/Exam/Preview </font>**
> 取得預覽連結

* ### Header
  > 帶入user token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
  > | Name    | Meaning |
  > | ------- |---------|
  > | examUID | 試卷代碼 |

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-11-23T17:13:43.1369885+08:00",
    "message": "",
    "disposal": null,
    "data": "https://oneexam-dev.oneclass.com.tw/paper/preview/110-8b9c6f7af6ab4929806074e592a20952?otp=300fc3ccd5ab4bd48375f2819fec6b21"
  }
  ```
  > 欄位說明
  > | Name     | Meaning |
  > | ---------|---------|
  > | data     | 預覽連結
---

### **<font color=#66B3FF>[GET] /api/Exam/Question </font>**
> 複製試卷

* ### Header
  > 帶入user token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
  > | Name    | Meaning |
  > | ------- |---------|
  > | examUID | 試卷代碼 |
  > | action  | 試卷操作 |

  ### ※ 試卷操作 ※
  >| Name  | Meaning
  > ------ | -------
  > Copy   | 複製
  > Edit   | 編輯

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2022-01-03T16:54:13.5760161+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "examUID": "110-8c235c0970da4eb78738e469c78a474c",
      "pattern": "FastPattern",
      "examPaper": {
        "uid": "110-8c235c0970da4eb78738e469c78a474c",
        "isPublic": false,
        "usetype": "General",
        "attribute": {
          "name": "110學年度國中英語測驗",
          "examType": "General",
          "drawUp": "FastPattern",
          "yearName": "110學年度",
          "eduName": "國中",
          "subjectName": "英語",
          "bookNames": [
            "第一冊",
            "第三冊"
          ],
          "version": "N",
          "versionName": "南一",
          "year": "110",
          "education": "J",
          "subject": "EN",
          "bookIDs": [
            "110N-JENB01",
            "110N-JENB03"
          ]
        },
        "questionGroup": [
          {
            "typeCode": "SS013",
            "typeName": "文法",
            "scoreType": "PerAnswer",
            "score": 0,
            "questionList": [
              {
                "sequence": 1,
                "id": "efe024e795f94f438502ca0971c02da0",
                "answerAmount": 1,
                "score": 0,
                 "difficulty": "BEGIN",
              },
              {
                "sequence": 2,
                "id": "73ae1788c4fc495c9f73b445e56ce03b",
                "answerAmount": 1,
                "score": 0,
                 "difficulty": "BEGIN",
              }
            ]
          },
          {
            "typeCode": "SS026",
            "typeName": "字彙選擇(認識字彙)",
            "scoreType": "PerAnswer",
            "score": 0,
            "questionList": [
              {
                "sequence": 1,
                "id": "1c5f3c8c22db4f3ebcd3722fb1cd96ed",
                "answerAmount": 1,
                "score": 0,
                 "difficulty": "BEGIN",
              },
              {
                "sequence": 2,
                "id": "2171dd4e807c43cb88d0a8f8407c7cbe",
                "answerAmount": 1,
                "score": 0,
                 "difficulty": "BEGIN",
              }
            ]
          }
        ],
        "maintainerUID": "65472133-afdc-4c92-b586-caff001a5e85",
        "tags": [
          "liveoneclass"
        ],
        "favorites": 0,
        "isLock": false,
        "lockMaintainer": null,
        "lockTime": "0001-01-01T00:00:00",
        "maintainer": "[tikuadmin] 管理者",
        "createTime": "2021-10-20T10:52:43.531Z",
        "updateTime": null
      },
      "questionInfo": {
        "searchKey": "7c40aeea461b4dc88fcea14e9fcec3f0",
        "questionGroup": {
          "文法": {
            "question": [
              {
                "uid": "73ae1788c4fc495c9f73b445e56ce03b",
                "difficulty": "BEGIN",
                "quesType": "SS013",
                "quesTypeName": "文法",
                "answerAmount": 1
              },
              {
                "uid": "1db8892c036349c7a6edaab384a3a56c",
                "difficulty": "BEGIN",
                "quesType": "SS013",
                "quesTypeName": "文法",
                "answerAmount": 1
              }
            ],
            "code": "SS013",
            "name": "文法",
            "count": {
              "difficulty": {
                "BEGIN": {
                  "question": 57,
                  "answer": 57
                },
                "INTERMEDIATE": {
                  "question": 93,
                  "answer": 93
                },
                "EXPERT": {
                  "question": 29,
                  "answer": 29
                }
              },
              "question": 179,
              "answer": 179
            }
          },
          "字彙填空(認識字彙)": {
            "question": [
              {
                "uid": "5cb01dd3cf954340b3d8634f4b98942f",
                "difficulty": "EXPERT",
                "quesType": "FL033",
                "quesTypeName": "字彙填空(認識字彙)",
                "answerAmount": 1
              },
              {
                "uid": "fcf1eedea2ca461c8805ae512de17667",
                "difficulty": "INTERMEDIATE",
                "quesType": "FL033",
                "quesTypeName": "字彙填空(認識字彙)",
                "answerAmount": 1
              }
            ],
            "code": "FL034",
            "name": "字彙填空(應用字彙)",
            "count": {
              "difficulty": {
                "BEGIN": {
                  "question": 8,
                  "answer": 8
                },
                "INTERMEDIATE": {
                  "question": 24,
                  "answer": 24
                },
                "EXPERT": {
                  "question": 7,
                  "answer": 7
                }
              },
              "question": 39,
              "answer": 39
            }
          }
        }
      },
      "disabled": [
        {
          "amount": 1,
          "code": "JEN-28",
          "name": "We Visited Our Relatives Yesterday"
        }
      ]
    }
  }
  ```
  > 欄位說明
  > | Name     | Meaning | Description
  > | ---------|---------| ------------ |
  > | examUID  | 試卷UID 
  > | drawUp   | 出卷方式
  > | ======== | ===========| ==========================
  > | <font color=#66B3FF> examPaper </font> | <font color=#66B3FF> 試卷內容 </font> |
  > | .uid      | 試卷UID
  > | .isPublic | 是否為公開卷
  > | .usetype  | 允許使用範圍
  > | <font color=#66B3FF> attribute </font> | <font color=#66B3FF> 試卷屬性 </font>
  > | .name     | 試卷名稱
  > | .examType | 試卷類別
  > | .drawUp     | 出卷方式代碼
  > | .year       | 學年度代碼
  > | .yearName   | 學年度
  > | .education  | 學制代碼
  > | .eduName    | 學制
  > | .subject    | 科目代碼
  > | .subjectName | 科目
  > | .bookIDs    | 課本代碼
  > | .bookNames  | 課本冊次名稱
  > | .version    | 版本代碼
  > | .versionName| 版本名稱
  > | <font color=#66B3FF> questionGroup </font> | <font color=#66B3FF> 試題組 </font> | 依題型分類
  > typeCode   | 題型代碼
  > typeName   | 題型名稱
  > scoreType  | 配分方式 | PerQuestion:"每題配分", PerAnswer:"每答配分"
  > score      | 配分分數
  > | <font color=#66B3FF> 試題清單 </font> | <font color=#66B3FF> 試題清單 </font>
  > sequence     | 題號
  > id           | 試題ID
  > answerAmount | 答案數
  > score        | 該題配分 | 不區分每題或每答, 為該題總分
  > difficulty   | 難易度代碼 | 僅分為易中難
  > | ========== | ===========| ==========================
  > | maintainer | 出卷者
  > | tags       | 試卷標籤 
  > | favorites  | 收藏數統計
  > | isLock     | 試卷是否封存
  > | lockMaintainer | 封存者
  > | lockTime   | 封存時間
  > | maintainer  | 出卷者
  > | createTime  | 建立時間
  > | updateTime  | 最後維護時間
  > | <font color=#66B3FF> disabled </font> | <font color=#66B3FF> 停用試題統計 </font> 
  > .code   | 知識向度代碼
  > .name   | 知識向度
  > .amount | 題數
  > | <font color=#66B3FF> questionInfo </font> | <font color=#66B3FF> 範圍試題查詢結果 </font> | 依照所屬pattern回傳相應格式

---

### **<font color=#FFA042>[PUT] /api/Exam/Edit/{examUID} </font>**
> 修改試卷

* ### Header
  > 帶入user token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
  > 格式同 **<font color=#1AFD9C>[POST] /api/Exam/Create </font>** Request
  ```json
  {
    "searchKey": "e1a5fbef41f44271933e5329da67d340",
    "drawUp": "FastPattern",
    "questionGroup": [
      {
        "typeCode": "SS017",
        "scoreType": "PerQuestion",
        "score": 20,
        "questionList": [
          "D3BM8KBnf0yZm5WS1w7DOQiP",
          "MKbpAiXGX0GeBKhVPy0O1QyD"
        ]
      }
    ]
  }
  ```
  > 欄位說明
  > |    | Name  | Meaning |
  > |-|-------| ------- |
  > | | examUID | 試卷代碼 |
  > | <b><font color=#FF0000>*</font></b> | searchKey | 搜尋Key
  > | <b><font color=#FF0000>*</font></b> | drawUp    | 出卷方式
  > | <b><font color=#FF0000>*</font></b> | <font color=#66B3FF> questionGroup </font>  |  <font color=#66B3FF> 試題資訊 </font>  | 
  > | <b><font color=#FF0000>*</font></b> | typeCode  | 題型代碼
  > | <b><font color=#FF0000>*</font></b> | scoreType | 配分方式
  > | <b><font color=#FF0000>*</font></b> | score     | 配分
  > | <b><font color=#FF0000>*</font></b> | questionList | 試題ID

* ### Response
  同 **<font color=#1AFD9C>[POST] /api/Exam/Create </font>**
---