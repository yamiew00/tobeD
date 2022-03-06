# **Fix Wrong**
## * <a href="https://onepaper-api-dev.oneclass.com.tw/swagger/index.html">swagger 測試路徑</a> *


### **<font color=#66B3FF>[GET] /api/Fix/Selection </font>**
> 錯題庫學制科目選單

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
  > ※※※ 只會回傳有錯題的範圍 ※※※
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2022-01-21T18:13:08.9403001+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "year": [
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
      "eduMap": [
        {
          "code": "J",
          "name": "國中"
        }
      ],
      "subjectMap": {
        "J": [
          {
            "code": "EN",
            "name": "英語"
          }
        ]
      }
    }
  }
  ```

  > 無資料
  ```json
  {
    "systemCode": "0402",
    "isSuccess": false,
    "systemNow": "2022-01-21T18:16:06.078598+08:00",
    "message": "",
    "disposal": null,
    "data": null
  }
  ```

  > 欄位說明
  >| Name     | Meaning 
  > --------- | ------- 
  > year      | 年度列表
  > eduMap    | 學制列表
  > subjectMap| 科目列表
---

### **<font color=#66B3FF>[GET] /api/Fix/{eduSubject} </font>**
> 取得錯題列表

* ### Header
  > 帶入user token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
    > 欄位說明
  >| Name     | Meaning 
  > --------- | ------- 
  > eduSubject| 年度列表
  > year      | 年度

* ### Response
  > 只回傳有錯題的冊次及章節
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2022-01-21T18:28:05.6263181+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "versionMap": [
        {
          "code": "N",
          "name": "南一"
        },
        {
          "code": "K",
          "name": "適康"
        },
        {
          "code": "H",
          "name": "適翰"
        }
      ],
      "bookMap": {
        "N": {
          "bookList": [
            {
              "code": "B01",
              "name": "第一冊"
            }
          ],
          "chapterMap": {
            "B01": {
              "bookID": "110N-JENB01",
              "bookName": "[110]國中英語(1)南一版章節",
              "bookDesc": null,
              "amount": 7,
              "chapters": [
                {
                  "hierarchy": 1,
                  "code": "0",
                  "name": "Starter",
                  "parentCode": null,
                  "desc": "[0] Starter",
                  "amount": 1,
                  "questions": [
                    {
                      "id": "25944db6b340434682fbe5d87c6dc4fc",
                      "image": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJEN%2F25944db6b340434682fbe5d87c6dc4fc%2F25944db6b340434682fbe5d87c6dc4fc.gif?alt=media&token=695f7fb7-8c14-4138-9398-57d7c672b228",
                      "questionImage": {
                        "question": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJEN%2F25944db6b340434682fbe5d87c6dc4fc%2FQuestionOverall.gif?alt=media&token=fe750a25-a601-464c-9abf-9661dde45014",
                        "content": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJEN%2F25944db6b340434682fbe5d87c6dc4fc%2FQuestionContent.gif?alt=media&token=1cd439b0-a992-4bb1-b774-7f26fe4573bc",
                        "subQuestions": [
                          {
                            "question": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJEN%2F25944db6b340434682fbe5d87c6dc4fc%2FQuestionContent.gif?alt=media&token=1cd439b0-a992-4bb1-b774-7f26fe4573bc",
                            "options": [
                              "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJEN%2F25944db6b340434682fbe5d87c6dc4fc%2Foptions_a.gif?alt=media&token=b36f4003-6487-426d-91ef-a7eea6994744",
                              "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJEN%2F25944db6b340434682fbe5d87c6dc4fc%2Foptions_b.gif?alt=media&token=5550a1a7-2947-4437-a7da-e4829865b3c7",
                              "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJEN%2F25944db6b340434682fbe5d87c6dc4fc%2Foptions_c.gif?alt=media&token=c8635e0f-9000-4faf-8bec-77adbf2ee6d5",
                              "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJEN%2F25944db6b340434682fbe5d87c6dc4fc%2Foptions_d.gif?alt=media&token=4235badc-7257-4cfb-a28d-f7cbb6066ebe"
                            ]
                          }
                        ],
                        "answer": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJEN%2F25944db6b340434682fbe5d87c6dc4fc%2FAnswer.gif?alt=media&token=ae966c2c-65c1-4251-ae53-979ed5d62578",
                        "analyze": null
                      },
                      "htmlParts": {
                        "content": " <div class=\"WordSection1\" style='layout-grid:18.0pt'> <p class=\"MsoNormal\"><a name=\"SQM00B000\"></a><a name=\"SQM00BC00\"><span style='mso-bookmark:SQM00B000'><span lang=\"EN-US\">Ivy: <span style='mso-font-kerning: 0pt'>It is very late. Go to bed now. <br> Kevin: All right. </span></span></span></a><span style='mso-bookmark:SQM00BC00'><span style='mso-bookmark:SQM00B000'><u><span style='font-family:\"新細明體\",\"serif\"; mso-ascii-font-family:Calibri;mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體;mso-fareast-theme-font:minor-fareast;mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin'>　　　</span></u></span></span><a name=\"SQM00O001\"></a><a name=\"SQM00OT01\"><span style='mso-bookmark:SQM00O001'><span style='mso-bookmark:SQM00B000'><u><span lang=\"EN-US\"><br> (A)</span></u></span></span></a><a name=\"SQM00OC01\"><span style='mso-bookmark: SQM00B000'><span style='mso-bookmark:SQM00O001'><span lang=\"EN-US\">Good bye.</span></span></span></a><a name=\"SQM00O002\"></a><a name=\"SQM00OT02\"><span style='mso-bookmark:SQM00O002'><span style='mso-bookmark:SQM00B000'><span lang=\"EN-US\"><br> (B)</span></span></span></a><a name=\"SQM00OC02\"><span style='mso-bookmark:SQM00B000'><span style='mso-bookmark:SQM00O002'><span lang=\"EN-US\">Good night.</span></span></span></a><a name=\"SQM00O003\"></a><a name=\"SQM00OT03\"><span style='mso-bookmark:SQM00O003'><span style='mso-bookmark:SQM00B000'><span lang=\"EN-US\"><br> (C)</span></span></span></a><a name=\"SQM00OC03\"><span style='mso-bookmark:SQM00B000'><span style='mso-bookmark:SQM00O003'><span lang=\"EN-US\">Good evening.</span></span></span></a><a name=\"SQM00O004\"></a><a name=\"SQM00OT04\"><span style='mso-bookmark:SQM00O004'><span style='mso-bookmark:SQM00B000'><span lang=\"EN-US\"><br> (D)</span></span></span></a><a name=\"SQM00OC04\"><span style='mso-bookmark:SQM00B000'><span style='mso-bookmark:SQM00O004'><span lang=\"EN-US\">Good afternoon.</span></span></span></a></p> </div> ",
                        "answer": " <div class=\"WordSection1\" style='layout-grid:18.0pt'> <p class=\"MsoNormal\"><a name=\"SAM00B000\"><span style='font-family:\"新細明體\",\"serif\"; mso-ascii-font-family:Calibri;mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體;mso-fareast-theme-font:minor-fareast;mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin'>答案：</span></a><a name=\"SAM00BC00\"><span style='mso-bookmark:SAM00B000'><span lang=\"EN-US\">(B)</span></span></a></p> </div> ",
                        "analyze": ""
                      },
                      "typeCode": "SS018",
                      "typeName": "對話",
                      "answer": "[[2]]",
                      "userAnswer": "[[1]]"
                    }
                  ]
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
  > | Name  | Meaning 
  > | ------| ------- |
  > versionMap | 版本選單
  > | .key     | 版本代碼
  > | .bookList| 冊次選單
  > | <font color=#66B3FF> .chapterMap 課本章節 </font>
  > bookID	   | 課本ID
  > bookName   | 課本名稱
  > amount     | 課本題數總計
  > | <font color=#66B3FF> chapters 章節資料 </font>
  > hierarchy  | 階層
  > code       | 代碼
  > name       | 名稱
  > desc       | 簡述
  > parentCode | 父層代碼
  > amount     | 章節題數總計
  > | <font color=#66B3FF> questions 錯題資訊 </font>
  > id         | 錯題ID
  > image      | 試題內容圖檔路徑
  > typeDesc   | 題型
  > answer     | 試題答案
  > userAnswer | 使用者作答
  > | <font color=#66B3FF> .questionImage </font> | <font color=#66B3FF> 試題圖檔  </font>
  > question | 完整試題 | 題目+選項
  > content  | 題幹
  > answer   | 答案
  > analyze  | 解析
  > | <font color=#66B3FF> .subQuestions </font> | <font color=#66B3FF> 子題
  > question | 題目
  > options  | 選項
  > | <font color=#66B3FF> htmlParts </font> | <font color=#66B3FF> 拆分後的html </font>
  > content    | 試題內容 | 試題內文 & 選項
  > answer     | 答案
  > analyze    | 解析
  >|
---

### **<font color=#FFA042>[PUT] /api/Fix/{eduSubject}/Understand</font>**
> 標註「我懂了」

* ### Header
  > 帶入user token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
  > 欄位說明
  > | Name      | Meaning 
  > | ----------| ------- |
  > | eduSubject| 學制科目
  > | ID        | 試題ID

* ### Response
  > 更新成功
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2022-01-22T10:37:23.7999506+08:00",
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
    "systemNow": "2022-01-22T10:41:33.3132075+08:00",
    "message": "[錯題紀錄讀取失敗]",
    "disposal": null,
    "data": null
  }
  ```
---


### **<font color=#1AFD9C>[POST] /api/Fix/Practice </font>**
> 建立測驗

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
    "version": "N",
    "bookIDs": [
      "110N-JENB05"
    ],
    "searchKey": "a8bf722a289c4b7b8472148a578375fd",
    "questionGroup": [
      {
        "typeCode": "SS013",
        "typeName": "文法",
        "questionList": [
          "5c340f8bd8a749b4af1833def8ebe7a5", "bcdfa4b016a747299d6a8aa8021062f5"
        ]
      }
    ]
  }
  ```

  > 欄位說明 **所有欄位全部必填**
  >| Name      | Meaning 
  >| ----------| ------- |
  >| version   | 學制科目
  >| bookIDs   | 試題ID
  >| searchKey | 查詢結果代碼
  >| <font color=#66B3FF> questionGroup </font> |  <font color=#66B3FF> 試題群組 </font>
  >| .typeCode  | 題型代碼 
  >| .typeName  | 題型名稱
  >| .questionList | 試題ID

* ### Response
  > 建立成功
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2022-01-22T14:12:47.0068613+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "uid": "110-7b6f6c2f34764178b62ac85e93d245e9",
      "name": "110學年度南一第五冊_錯題練習"
    }
  }
  ```
---