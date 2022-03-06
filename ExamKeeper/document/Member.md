# **Member**
## * <a href="https://onepaper-api-dev.oneclass.com.tw/swagger/index.html">swagger 測試路徑</a> *
---
### **<font color=#66B3FF>[GET] /api/Member/Login </font>**
> 使用者登入

* ### Header
  > 帶入oneclub token
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
    "systemNow": "2021-10-19T14:55:21.5548771+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "userProfile": {
        "uid": "65472133-afdc-4c92-b586-caff001a5e85",
        "name": "管理者",
        "identity": "Admin",
        "email": "nani.tiku1680@gmail.com",
        "account": "tikuadmin",
        "usetype": "General",
        "organization": {
          "type": "Agency",
          "code": "NanI",
          "name": "南一書局"
        },
        "status": "Active",
        "lastLogin": "2021-07-02T10:12:00.815Z"
      },
      "preference": {
        "education": "J",
        "subject": "MA"
      },
      "examQueryMap": [
        {
          "code": "Name",
          "name": "試卷名稱"
        },
        {
          "code": "Author",
          "name": "命題教師"
        }
      ],
      "token": "809ab3d6-68b8-47b3-bae7-bf0f5a91759f",
      "expireAt": 1634637322
    }
  }
  ```

  > 欄位說明
  > | Name                                    | Meaning                                     |
  > | --------------------------------------- | ------------------------------------------- |
  > | ===== userProfile =====                 | <font color=#84C1FF> 使用者基本資料</font>  |
  > | uid                                     | UID                                         |
  > | name                                    | 姓名                                        |
  > | identity                                | 身分代碼                                    |
  > | email                                   | 電子信箱                                    |
  > | account                                 | 帳號 (沿用使用者來源帳號)                   |
  > | <font color=#84C1FF>organization</font> | 所屬組織/機構                               |
  > | .type                                   | 組織類別                                    |
  > | .code                                   | 組織代碼                                    |
  > | .name                                   | 組織名稱                                    |
  > | education                               | 可用學制 (有資料才回傳)                     |
  > | subject                                 | 可用科目 (有資料才回傳)                     |
  > | status                                  | 帳戶狀態                                    |
  > | lastLogin                               | 最後登入時間                                |
  > | =================                       | =================                           |
  > | <font color=#84C1FF> preference</font>  | <font color=#84C1FF> 個人操作設定</font>    |
  > | .education                              | 學制                                        |
  > | .subject                                | 科目                                        |
  > | examQueryMap                            | 查詢試卷可用欄位選單                        |
  > | token                                   | 一次性token (時限1小時, 系統操作時自動延長) |
  > | expireAt                                | 逾期時間 (timestamp)                        |
---

### **<font color=#66B3FF>[GET] /api/Member/UserProfile</font>**
> 讀取使用者資訊

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
    "systemNow": "2021-10-19T15:01:25.4055374+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "userProfile": {
        "uid": "65472133-afdc-4c92-b586-caff001a5e85",
        "name": "管理者",
        "identity": "Admin",
        "email": "nani.tiku1680@gmail.com",
        "account": "tikuadmin",
        "usetype": "General",
        "organization": {
          "type": "Agency",
          "code": "NanI",
          "name": "南一書局"
        },
        "status": "Active",
        "lastLogin": "2021-07-02T10:12:00.815Z"
      },
      "preference": {
        "education": "J",
        "subject": "MA"
      }
    }
  }
  ```
  > **同<font color=#66B3FF>[GET] /api/Member/Login </font>**，但不回傳token

---

### **<font color=#66B3FF>[GET] /api/Member/EduSubject </font>**
> 取得學制科目列表
> 2021/12/06 回傳內容調整為操作設定資訊

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
      "systemNow": "2022-03-05T17:43:22.1112835+08:00",
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
              },
              {
                  "code": "Quick",
                  "name": "速測"
              }
          ],
          "patternMap": [
              {
                  "code": "FastPattern",
                  "name": "電腦命題"
              },
              {
                  "code": "AutoPattern",
                  "name": "智能命題"
              },
              {
                  "code": "CustomPattern",
                  "name": "手動命題"
              },
              {
                  "code": "KnowledgePattern",
                  "name": "知識向度命題"
              }
          ],
          "publisherMap": {
              "B": "部編",
              "CW": "全華",
              "G": "綜合",
              "H": "翰林",
              "K": "康軒",
              "KX": "康熹",
              "L": "龍騰",
              "N": "南一",
              "S": "三民",
              "T": "泰宇"
          },
          "eduMap": [
              {
                  "code": "E",
                  "name": "國小"
              },
              {
                  "code": "J",
                  "name": "國中"
              },
              {
                  "code": "H",
                  "name": "高中"
              }
          ],
          "eduSubject": {
              "E": [
                  {
                      "code": "CH",
                      "name": "國語"
                  },
                  {
                      "code": "CN",
                      "name": "華語"
                  },
                  {
                      "code": "EN",
                      "name": "英語"
                  },
                  {
                      "code": "MA",
                      "name": "數學"
                  },
                  {
                      "code": "NA",
                      "name": "自然與生活科技"
                  },
                  {
                      "code": "SO",
                      "name": "社會"
                  },
                  {
                      "code": "LI",
                      "name": "生活"
                  },
                  {
                      "code": "PE",
                      "name": "健康與體育"
                  }
              ],
              "J": [
                  {
                      "code": "PC",
                      "name": "國文"
                  },
                  {
                      "code": "CN",
                      "name": "華語"
                  },
                  {
                      "code": "EN",
                      "name": "英語"
                  },
                  {
                      "code": "MA",
                      "name": "數學"
                  },
                  {
                      "code": "BI",
                      "name": "生物"
                  },
                  {
                      "code": "PY",
                      "name": "理化"
                  },
                  {
                      "code": "EA",
                      "name": "地球科學"
                  },
                  {
                      "code": "GE",
                      "name": "地理"
                  },
                  {
                      "code": "HI",
                      "name": "歷史"
                  },
                  {
                      "code": "CT",
                      "name": "公民"
                  },
                  {
                      "code": "PE",
                      "name": "健康與體育"
                  },
                  {
                      "code": "CO",
                      "name": "綜合活動"
                  },
                  {
                      "code": "TC",
                      "name": "科技"
                  }
              ],
              "H": [
                  {
                      "code": "PC",
                      "name": "國文"
                  },
                  {
                      "code": "CN",
                      "name": "華語"
                  },
                  {
                      "code": "EW",
                      "name": "英文"
                  },
                  {
                      "code": "MA",
                      "name": "數學"
                  },
                  {
                      "code": "BI",
                      "name": "生物"
                  },
                  {
                      "code": "PH",
                      "name": "物理"
                  },
                  {
                      "code": "CE",
                      "name": "化學"
                  },
                  {
                      "code": "EA",
                      "name": "地球科學"
                  },
                  {
                      "code": "GE",
                      "name": "地理"
                  },
                  {
                      "code": "HI",
                      "name": "歷史"
                  },
                  {
                      "code": "CS",
                      "name": "公民與社會"
                  }
              ]
          },
          "eduGrade": {
              "E": [
                  {
                      "code": "G01",
                      "name": "一年級"
                  },
                  {
                      "code": "G02",
                      "name": "二年級"
                  },
                  {
                      "code": "G03",
                      "name": "三年級"
                  },
                  {
                      "code": "G04",
                      "name": "四年級"
                  },
                  {
                      "code": "G05",
                      "name": "五年級"
                  },
                  {
                      "code": "G06",
                      "name": "六年級"
                  }
              ],
              "J": [
                  {
                      "code": "G01",
                      "name": "一年級"
                  },
                  {
                      "code": "G02",
                      "name": "二年級"
                  },
                  {
                      "code": "G03",
                      "name": "三年級"
                  }
              ],
              "H": [
                  {
                      "code": "G01",
                      "name": "一年級"
                  },
                  {
                      "code": "G02",
                      "name": "二年級"
                  },
                  {
                      "code": "G03",
                      "name": "三年級"
                  }
              ]
          }
      }
  }
  ```
  
  > 欄位說明
  > | Name         | Meaning  |
  > | ------------ | -------- |
  > | eduMap       | 學制選單 |
  > | eduSubject   | 學制科目 |
  > | outputMap    | 輸出方式 |
  > | patternMap   | 選題方式 |
  > | publisherMap | 出版社   |
  > | eduGrade     | 年級     |

---

### **<font color=#1AFD9C>[POST] /api/Member/Preference </font>**
> 記錄個人操作設定

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
    "education": "J",
    "subject": "MA"
  }
  ```
  > 欄位說明
  > | Name      | Meaning |
  > | --------- | ------- |
  > | education | 學制    |
  > | subject   | 科目    |

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-10-19T14:41:18.2629856+08:00",
    "message": "",
    "disposal": null,
    "data": null
  }
  ```
---

### **<font color=#66B3FF>[GET] /api/Member/Typesetting/Related </font>**
> 取得自主設定相關資訊

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
    "systemNow": "2021-10-20T17:07:50.4220718+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "uid": "65472133-afdc-4c92-b586-caff001a5e85",
      "isTeacher": false,
      "identityName": "系統管理員",
      "account": "tikuadmin",
      "name": "管理者",
      "organizationName": "南一書局",
      "paperSizeMap": [
        {
          "code": "A4",
          "name": "A4"
        },
        {
          "code": "A3",
          "name": "A3"
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
        "paperName": "自訂試卷名稱",
        "teacherSign": "出題教師",
        "grade": "五",
        "room": "二",
        "eduSubject": "國小數學",
        "studentSign": "學生填入座號姓名",
        "paperSize": "A4",
        "wordSetting": "VS",
        "paperContents": [
          "Question",
          "AnswerPaper"
        ],
        "analyzeContent": null,
        "amount": 0,
        "advanced": null
      }
    }
  }
  ```

  > 欄位說明
  > | Name                                     | Meaning                                       |
  > | ---------------------------------------- | --------------------------------------------- |
  > | uid                                      | 使用者UID                                     |
  > | isTeacher                                | 是否為教師                                    |
  > | identityName                             | 身分                                          |
  > | account                                  | 帳號                                          |
  > | name                                     | 姓名                                          |
  > | organizationName                         | 所屬機構名稱                                  |
  > | <font color=#84C1FF> typesetting </font> | <font color=#84C1FF> 個人設定 </font>         |
  > | schoolName                               | 學校名稱                                      |
  > | paperName                                | 試卷名稱                                      |
  > | teacherSign                              | 出題教師                                      |
  > | grade                                    | 年級                                          |
  > | room                                     | 班號                                          |
  > | eduSubject                               | 學制科目                                      |
  > | studentSign                              | 考生姓名                                      |
  > | paperSize                                | 紙張大小                                      |
  > | wordSetting                              | 排版方式                                      |
  > | paperContents                            | 匯出卷別                                      |
  > | analyzeContent                           | 解析卷匯出項目 (設定匯出卷別包含解析卷時必填) |
  > | amount                                   | 輸出卷種數                                    |
  > | advanced                                 | 進階設定 (輸出卷數 > 1 時不可為空)            |
  > | ===============                          | ===============                               |
  > | paperSizeMap                             | 紙張大小選單                                  |
  > | wordSettingMap                           | 排版方式選單                                  |
  > | paperContent                             | 匯出卷別選單 (可複選)                         |
  > | analyzeContent                           | 解析卷輸出項目 (可複選)                       |
  > | advancedSetting                          | 進階設定 (可複選)                             |
---


### **<font color=#1AFD9C>[POST] /api/Member/Typesetting </font>**
> 設定個人常用資訊

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
    "schoolName": "自訂學校",
    "paperName": "自訂試卷名稱",
    "teacherSign": "出題教師",
    "grade": "五",
    "room": "二",
    "eduSubject": "國小數學",
    "studentSign": "學生填入座號姓名",
    "paperSize": "A4",
    "wordSetting": "VS",
    "paperContents": [
      "Question",
      "Analyze"
    ],
    "analyzeContent": [
      "Answer",
      "Analyze"
    ],
    "amount": 2,
    "advanced":["ChangeOption"]
  }
  ```
  > 欄位說明
  > |                                     | Name           | Meaning                                                      |
  > | ----------------------------------- | -------------- | ------------------------------------------------------------ |
  > |                                     | schoolName     | 學校名稱                                                     |
  > | <b><font color=#FF0000>*</font></b> | paperName      | 試卷名稱                                                     |
  > |                                     | teacherSign    | 出題教師                                                     |
  > |                                     | grade          | 年級                                                         |
  > |                                     | room           | 班號                                                         |
  > |                                     | eduSubject     | 學制科目                                                     |
  > |                                     | studentSign    | 考生姓名                                                     |
  > |                                     | paperSize      | 紙張大小                                                     |
  > |                                     | wordSetting    | 排版方式                                                     |
  > |                                     | paperContents  | 匯出卷別                                                     |
  > |                                     | analyzeContent | 解析卷輸出項目                                               |
  > |                                     | amount         | 輸出卷種數 (限 1-5)                                          |
  > |                                     | advanced       | 進階設定 (輸出卷數 > 1 時必須包含 "ChangeOrder" 或 "ChangeOption") |

* ### Response
  > 寫入成功
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow":  "2021-10-20T16:38:18.  0852558+08:00",
    "message": "",
    "disposal": null,
    "data": null
  }
  ```
  > 寫入失敗
  ```json
  {
    "systemCode": "0400",
    "isSuccess": false,
    "systemNow": "2021-10-20T16:33:31.3570201+08:00",
    "message": "[無法識別紙張大小]\n[無法識別排版方式]\n[無法識別輸出卷別]\n",
    "disposal": null,
    "data": null
  }
  ```