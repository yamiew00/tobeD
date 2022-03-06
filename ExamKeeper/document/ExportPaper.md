# **Export Exam Paper**
## * <a href="https://onepaper-api-dev.oneclass.com.tw/swagger/index.html">swagger 測試路徑</a> *


### **<font color=#66B3FF>[GET] /Exam/Export/Related </font>**
> 取得排版資訊

* ### Header
  > 帶入user token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
  >| Name     | Meaning 
  > --------- | ------- 
  > examUID   | 可不帶入，帶入時會自動填入前一次輸出設定

  > ※※※ 版面配置預設值: 同一張試卷的最後一次輸出設定 > 使用者常用設定 ※※※

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-12-27T18:52:49.06887+08:00",
    "message": "",
    "disposal": null,
    "data": {
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
      "settings": {
        "Files": {
          "paperSizeMap": [
            {
              "code": "A4",
              "name": "A4"
            },
            {
              "code": "A3",
              "name": "A3"
            },
            {
              "code": "B4",
              "name": "B4"
            },
            {
              "code": "B5",
              "name": "B5"
            }
          ],
          "wordSettingMap": [
            {
              "code": "VS",
              "name": "直書單欄"
            },
            {
              "code": "VD",
              "name": "直書雙欄"
            },
            {
              "code": "HS",
              "name": "橫書單欄"
            },
            {
              "code": "HDA",
              "name": "橫書雙欄A"
            },
            {
              "code": "HDB",
              "name": "橫書雙欄B"
            }
          ],
          "paperContent": [
            {
              "code": "Question",
              "name": "題目卷"
            },
            {
              "code": "Answer",
              "name": "答案卷"
            },
            {
              "code": "Analyze",
              "name": "解析卷"
            },
            {
              "code": "Read",
              "name": "閱卷"
            },
            {
              "code": "AnswerPaper",
              "name": "作答紙"
            }
          ],
          "analyzeContent": [
            {
              "code": "Question",
              "name": "題目"
            },
            {
              "code": "Answer",
              "name": "答案"
            },
            {
              "code": "Analyze",
              "name": "解析"
            },
            {
              "code": "Difficulty",
              "name": "難易度"
            },
            {
              "code": "Source",
              "name": "出處"
            },
            {
              "code": "Knowledge",
              "name": "知識向度"
            }
          ],
          "advancedSetting": [
            {
              "code": "Continuous",
              "name": "題號連續"
            },
            {
              "code": "AddSpace",
              "name": "插入組距"
            },
            {
              "code": "ChangeOrder",
              "name": "相同題目，不同題序"
            },
            {
              "code": "ChangeOption",
              "name": "相同題目，選項隨機排列"
            }
          ],
          "typesetting": {
            "schoolName": "自訂學校",
            "paperName": "11",
            "teacherSign": "出題教師",
            "grade": "五",
            "room": "二",
            "eduSubject": "國中國文",
            "studentSign": "座號：＿＿＿  姓名：＿＿＿＿＿＿",
            "paperSize": "A4",
            "wordSetting": "VS",
            "paperContents": [
              "Question",
              "Analyze"
            ],
            "analyzeContent": [
              "Question",
              "Answer",
              "Analyze"
            ],
            "amount": 2,
            "advanced": [
              "ChangeOption"
            ]
          }
        },
        "Online": {
          "advancedSetting": [
            {
              "code": "ChangeOrder",
              "name": "相同題目，不同題序"
            },
            {
              "code": "ChangeOption",
              "name": "相同題目，選項隨機排列"
            }
          ],
          "advanced": [
            "ChangeOption"
          ]
        }
      }
    }
  }
  ```
  > 欄位說明
  >| Name     | Meaning 
  > --------- | ------- 
  > outputMap | 輸出方式列表
  > settings  | 輸出方式對應設定
  > <font color=#84C1FF> Files </font> | <font color=#84C1FF> 紙本卷設定 </font>
  > paperSizeMap   | 紙張大小選單
  > wordSettingMap | 排版方式選單
  > paperContent   | 匯出卷別清單 (可複選)
  > analyzeContent | 解析卷輸出項目
  > advancedSetting| 進階設定
  > <font color=#84C1FF> typesetting </font> | <font color=#84C1FF> 個人設定 </font>
  > schoolName | 學校名稱
  > paperName  | 試卷名稱
  > teacherSign | 出題教師 
  > grade      | 年級
  > room       | 班號
  > eduSubject | 學制科目
  > studentSign| 考生姓名
  > paperSize  | 紙張大小
  > wordSetting| 排版方式
  > paperContents  | 匯出卷別
  > analyzeContent | 解析卷輸出項
  > amount     | 輸出幾種卷
  > advanced   | 進階設定
  > <font color=#84C1FF> Online </font> | <font color=#84C1FF> 線上測驗卷設定 </font>
  > advancedSetting| 進階設定
  > advanced   | 進階設定
---

### **<font color=#1AFD9C>[POST] /api/Exam/Export </font>**
> 匯出試卷

* ### Header
  > 帶入user token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
  > 匯出紙本卷 
  ```json
  {
    "examUID": "110-33a2574119fd48acb41a0d650c4cbdce",
    "outputType": "Files",
    "typesetting": {
      "schoolName": "學校名字",
      "paperName": "考卷名",
      "teacherSign": "出卷老師",
      "grade": "年級",
      "room": "班級",
      "eduSubject": "國中數學",
      "studentSign": "__學生__",
      "paperSize": "A4",
      "wordSetting": "HDA",
      "paperContents": [
        "Question"
      ],
      "analyzeContent": [
        "Answer",
        "Analyze"
      ],
      "amount": 2,
      "advanced": [
        "ChangeOption"
      ]
    }
  }
  ```

  > 匯出線上測驗卷 
  ```json
  {
    "examUID": "110-33a2574119fd48acb41a0d650c4cbdce",
    "outputType": "Files",
    "onlineSetting": {
      "advanced": [
        "ChangeOrder",
        "ChangeOption"
      ]
    }
  }
  ```

  > 欄位說明
  >| Name     | Meaning 
  > --------- | ------- 
  > examUID   | 試卷UID
  > outputType| 輸出方式
  > <font color=#84C1FF> typesetting </font> | <font color=#84C1FF> 紙本卷設定 </font>
  > schoolName | 學校名稱
  > paperName  | 試卷名稱
  > teacherSign | 出題教師 
  > grade      | 年級
  > room       | 班號
  > eduSubject | 學制科目
  > studentSign| 考生姓名
  > paperSize  | 紙張大小
  > wordSetting| 排版方式
  > paperContents  | 匯出卷別
  > analyzeContent | 解析卷輸出項
  > amount     | 輸出卷種數
  > advanced   | 進階設定
  > <font color=#84C1FF> onlineSetting </font> | <font color=#84C1FF> 線上卷設定 </font>

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2022-01-12T18:34:05.9821739+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "uid": "3c6b1f9c1034490e843434660355130c",
      "examUID": "110-33a2574119fd48acb41a0d650c4cbdce"
    }
  }
  ```
  > 欄位說明
  >| Name  | Meaning 
  > ------ | ------- 
  > uid    | 匯出UID
  > examUID| 試卷UID
---

### **<font color=#66B3FF>[GET] /api/Service/ExamExport </font>**
> 取得最優先待組卷資料，取得資料同時會將狀態更新為"開始組卷"

* ### Header
  > 帶入service token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
  *none*

* ### Response
  > 組卷資訊，  
  > 回傳資料與 **<font color=#1AFD9C>[POST] /api/Exam/Export </font>** 相同
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-11-29T16:06:53.8559957+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "uid": "21127e085e3e4bfeba5b12b7c7ea93c3",
      "examUID": "110-39393f5d620c40ed8acaa3812f159404",
      "name": "110學年度國中數學測驗",
      "year": "110",
      "typesetting": {
        "schoolName": "學校名字",
        "paperName": "考卷名",
        "teacherSign": "出卷老師",
        "grade": "年級",
        "room": "班級",
        "eduSubject": "國中數學",
        "studentSign": "__學生__",
        "paperSize": "A4",
        "wordSetting": "HDA",
        "paperContents": [
          "Question"
        ]
      },
      "questionGroup": [
        {
          "typeCode": "SS017",
          "typeName": "單選題",
          "printCode": "1",
          "printName": "(　　)1.題目……",
          "scoreType": "PerAnswer",
          "score": 0,
          "questionList": [
            {
              "sequence": 1,
              "id": "277283d3430e4cab94fcb2f2220461cd",
              "url": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJMA%2F277283d3430e4cab94fcb2f2220461cd%2F277283d3430e4cab94fcb2f2220461cd.doc?alt=media&token=7c2e49f4-5abd-4d97-8d60-1f82a8ac93f1"
            },
            {
              "sequence": 2,
              "id": "25db5a6b825c4f83ad2e27b404f9f04e",
              "url": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJMA%2F25db5a6b825c4f83ad2e27b404f9f04e%2F25db5a6b825c4f83ad2e27b404f9f04e.doc?alt=media&token=4aa10fd4-0757-4c6f-908d-37c297b878f9"
            },
            {
              "sequence": 3,
              "id": "227f0e60f2be42e6b4c7adf2110b492f",
              "url": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJMA%2F227f0e60f2be42e6b4c7adf2110b492f%2F227f0e60f2be42e6b4c7adf2110b492f.doc?alt=media&token=a6de8f44-8dc6-4dc7-ae6b-188c0298d099"
            }
          ]
        }
      ]
    }
  }
  ```
  > 無待處理資料
  ```json
  {
    "systemCode": "0402",
    "isSuccess": false,
    "systemNow": "2021-11-29T16:13:11.3786898+08:00",
    "message": "",
    "disposal": null,
    "data": null
  }
  ```
---

### **<font color=#1AFD9C>[POST] /api/Service/ExportStatus </font>**
> 更新組卷狀態

* ### Header
  > 帶入service token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
  ```json
  {
    "uid": "21127e085e3e4bfeba5b12b7c7ea93c3",
    "status": "Convert",
    "downloadUrl": "",
    "message": "測試"
  }
  ```

* ### Response
  > 更新成功
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-11-29T17:03:54.4300271+08:00",
    "message": "",
    "disposal": null,
    "data": null
  }
  ```

  > 更新失敗
  ```json
  {
    "systemCode": "0400",
    "isSuccess": false,
    "systemNow": "2021-11-29T17:10:19.0719488+08:00",
    "message": "Record資料：21127e085e3efeba5b12b7c7ea93c3不存在, 請確認輸入內容。",
    "disposal": null,
    "data": null
  }
  ```
---

### **<font color=#66B3FF>[GET] /api/Exam/Export </font>**
> 取得個人組卷紀錄

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
    "systemNow": "2021-11-29T18:18:11.029856+08:00",
    "message": "",
    "disposal": null,
    "data": [
      {
        "uid": "5ff457e684aa4a7098c263c998345505",
        "examUID": "110-39393f5d620c40ed8acaa3812f159404",
        "examName": "110學年度國中數學測驗",
        "paperName": "考卷名",
        "amount": 67,
        "eduSubjectName": "國中數學",
        "status": "組卷中",
        "downloadUrl": "",
        "message": "測試",
        "createTime": "2021-11-29T09:38:59.146Z",
        "lastUpdateTime": "2021-11-29T09:40:09.733Z"
      }
    ]
  }
  ```
  > 欄位說明
  >| Name     | Meaning 
  > --------- | ------- 
  > uid       | 組卷紀錄UID
  > examUID   | 試卷UID
  > examName  | 試卷名稱
  > paperName | 組卷名稱
  > amount    | 總題數
  > eduSubjectName |  學制科目 (使用者組卷時自行填入的，非資源庫定義資料)
  > status    | 目前狀態
  > downloadUrl | 下載路徑
  > message   | 系統訊息
  > createTime| 開始時間 
  > lastUpdateTime | 最後更新時間
---

### **<font color=#66B3FF>[GET] /api/Exam/Export/{UID} </font>**
> 查詢組卷進度
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
  > | UID  | 匯出UID

* ### Response
  > 等待中
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2022-01-12T18:30:19.7161403+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "uid": "3889b4b1f39a41548388074e0d992dc7",
      "examUID": "110-33a2574119fd48acb41a0d650c4cbdce",
      "status": "Waiting",
      "statusDesc": "等待中",
      "waitingPaper": 2,
      "minutes": 1,
      "seconds": 0,
      "download": null,
      "downloadName": null,
      "message": ""
    }
  }
  ```
  > 組卷完成
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2022-01-12T18:29:57.6256185+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "uid": "9bb170198eb54797bbc340ae6b95c48f",
      "examUID": "110-b5d3278d4f1b466bbd40278d2bf68f7d",
      "status": "Finished",
      "statusDesc": "組卷完成",
      "waitingPaper": 0,
      "minutes": 0,
      "seconds": 0,
      "download": "https://firebasestorage.googleapis.com/v0/b/onepaper-1126a.appspot.com/o/examination%2F9bb170198eb54797bbc340ae6b95c48f.zip?alt=media&token=03d1b998-cbec-494d-8476-266e5b0043b7",
      "downloadName": "2022-01-12-06-16-55國中英語試卷.zip",
      "message": ""
    }
  }
  ```

  > | Name      | Meaning |
  > | --------- |---------|
  > | uid       | 匯出UID 
  > | examUID   | 試卷UID
  > | status    | 狀態代碼
  > | statusDesc| 狀態說明 
  > | waitingPaper | 待組試卷
  > | minutes   | 預估時間 (分鐘)
  > | seconds   | 預估時間 (秒)
  > | download  | 下載路徑
  > | downloadName | 下載名稱
  > | message   | 系統訊息