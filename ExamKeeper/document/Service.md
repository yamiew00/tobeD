# **Service**
## * <a href="https://onepaper-api-dev.oneclass.com.tw/swagger/index.html">swagger 測試路徑</a> *

### **<font color=#1AFD9C>[POST] /api/Service/CacheQuery </font>**
> ID查詢試題並寫入Cache

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
    "education": "J",
    "subject": "GE",
    "keys": [
      "04bc774c86354bd7846d931e15dae30a",
      "08330324b4ae466eaebb356eadde77d1",
      "113292b4258e4923badff7fec4a630d2"
    ]
  }
  ```
  >|| Name     | Meaning
  >|-| ----------| ---------------------
  >| <b><font color=#FF0000>*</font></b> | education | 學制代碼
  >| <b><font color=#FF0000>*</font></b> | subject   | 科目代碼
  >| <b><font color=#FF0000>*</font></b> | keys      | 試題ID

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-08-27T12:26:38.0559725+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "searchKey": "783faeb7bfd2497d83f83c42852f9006",
      "question": [
        {
          "uid": "04bc774c86354bd7846d931e15dae30a",
          "image": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.  appspot.com/o/  question-bank%2FJGE%2F04bc774c86354bd7846d931e15dae30a%2F04bc774c86354bd7 846d931e15dae30a.gif?alt=media&  token=b3a9e8ef-f723-4382-ba3a-0ec8c47cd277",
          "questionImage": "https://firebasestorage.googleapis.com/v0/b/  question-bank-dev.appspot.com/o/  question-bank%2FJGE%2F04bc774c86354bd7846d931e15dae30a%2FQuestionOverall. gif?alt=media&token=1fb79773-e92b-4f21-b5df-976d0265c84c",
          "metadata": [
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
                  "code": "1-4-7",
                  "name": "1-4-7"
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
                  "code": "地BE-IV-1",
                  "name": "自然環境背景。"
                }
              ]
            },
            {
              "code": "KNOWLEDGE",
              "name": "知識向度",
              "content": [
                {
                  "code": "JGE-18-1-2",
                  "name": "東南亞氣候"
                }
              ]
            },
            {
              "code": "TOPIC",
              "name": "主題",
              "content": [
                {
                  "code": "JGE-18",
                  "name": "東南亞與南亞的自然環境及多元文化"
                }
              ]
            }
          ],
          "answer": [
            "(B)"
          ],
          "answerPosition": [
            2
          ],
          "htmlParts": {
            "content": " <div class=\"WordSection1\" style='layout-grid:18.0pt'>  <p class=\"MsoNormal\"><a name=\"SQM00B000\"></a><a  name=\"SQM00BC00\"><span style='mso-bookmark:SQM00B000'><span  style='font-family:\"新細明體\",\"serif\";   mso-ascii-font-family:Calibri;mso-ascii-theme-font:minor-latin; mso-fareast-font-family: 新細明體;mso-fareast-theme-font:minor-fareast;  mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin;  color:black'>附圖的情景應屬於哪種降雨型態？</span><span lang=\"EN-US\"  style='color:black'><!--[if gte vml 1]><v:shapetype id=\"_x0000_t75\"  coordsize=\"21600,21600\" o:spt=\"75\" o:preferrelative=\"t\"  path=\"m@4@5l@4@11@9@11@9@5xe\" filled=\"f\" stroked=\"f\"> <v:stroke  joinstyle=\"miter\"/> <v:formulas>  <v:f eqn=\"if lineDrawn  pixelLineWidth 0\"/>  <v:f eqn=\"sum @0 1 0\"/>  <v:f eqn=\"sum 0 0  @1\"/>  <v:f eqn=\"prod @2 1 2\"/>  <v:f eqn=\"prod @3 21600   pixelWidth\"/>  <v:f eqn=\"prod @3 21600 pixelHeight\"/>  <v:f  eqn=\"sum @0 0 1\"/>  <v:f eqn=\"prod @6 1 2\"/>  <v:f eqn=\"prod @7   21600 pixelWidth\"/>  <v:f eqn=\"sum @8 21600 0\"/>  <v:f eqn=\"prod  @7 21600 pixelHeight\"/>  <v:f eqn=\"sum @10 21600 0\"/> </v:formulas>   <v:path o:extrusionok=\"f\" gradientshapeok=\"t\"   o:connecttype=\"rect\"/> <o:lock v:ext=\"edit\" aspectratio=\"t\"/> </  v:shapetype><v:shape id=\"_x005f_x0000_i1026\" o:spid=\"_x0000_i1025\"  type=\"#_x0000_t75\" style='width:252.75pt;height:110.25pt'  o:ole=\"\"> <v:imagedata src=\"content.files/image001.emz\"  o:title=\"\"/> </v:shape><![endif]--><![if !vml]><img width=\"337\"  height=\"147\" src=\"https://firebasestorage.googleapis.com/v0/b/  question-bank-dev.appspot.com/o/  question-bank%2FJGE%2F04bc774c86354bd7846d931e15dae30a%2FHtmlImage%2Fim g001.png?alt=media&token=596ea8df-85fa-4aea-b793-077e30108e6f\"  v:shapes=\"_x005f_x0000_i1026\"><![endif]><!--[if gte mso 9]><xml>   <o:OLEObject Type=\"Embed\" ProgID=\"Word.Document.8\"  ShapeID=\"_x005f_x0000_i1026\"  DrawAspect=\"Content\"   ObjectID=\"_1691496910\"> </o:OLEObject> </xml><![endif]--></span></  span></a><span style='mso-bookmark:SQM00BC00'></span><span  style='mso-bookmark:SQM00B000'><span style='font-family:\"新細明體\",  \"serif\"; mso-ascii-font-family:Calibri; mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體;  mso-fareast-theme-font:minor-fareast;mso-hansi-font-family:Calibri;   mso-hansi-theme-font:minor-latin;color:black'>　</span><a   name=\"SQM00O001\"></a><a name=\"SQM00OT01\"><span  style='mso-bookmark:SQM00O001'><span lang=\"EN-US\"  style='color:black'>(A)</span></span></a></span><span  style='mso-bookmark:SQM00OT01'></span><a name=\"SQM00OC01\"></a><span  style='mso-bookmark:SQM00OC01'><span style='mso-bookmark:  SQM00O001'><span style='mso-bookmark:SQM00B000'><span  style='font-family:\"新細明體\",\"serif\";   mso-ascii-font-family:Calibri;mso-ascii-theme-font:minor-latin; mso-fareast-font-family: 新細明體;mso-fareast-theme-font:minor-fareast;  mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin;  color:black'>地形雨</span></span></span></span><span  style='mso-bookmark:SQM00B000'><span style='font-family:\"新細明體\",  \"serif\"; mso-ascii-font-family:Calibri; mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體;  mso-fareast-theme-font:minor-fareast;mso-hansi-font-family:Calibri;   mso-hansi-theme-font:minor-latin;color:black'>　</span><a   name=\"SQM00O002\"></a><a name=\"SQM00OT02\"><span  style='mso-bookmark:SQM00O002'><span lang=\"EN-US\"  style='color:black'>(B)</span></span></a></span><span  style='mso-bookmark:SQM00OT02'></span><a name=\"SQM00OC02\"></a><span  style='mso-bookmark:SQM00OC02'><span style='mso-bookmark:  SQM00O002'><span style='mso-bookmark:SQM00B000'><span  style='font-family:\"新細明體\",\"serif\";   mso-ascii-font-family:Calibri;mso-ascii-theme-font:minor-latin; mso-fareast-font-family: 新細明體;mso-fareast-theme-font:minor-fareast;  mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin;  color:black'>對流雨</span></span></span></span><span  style='mso-bookmark:SQM00B000'><span style='font-family:\"新細明體\",  \"serif\"; mso-ascii-font-family:Calibri; mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體;  mso-fareast-theme-font:minor-fareast;mso-hansi-font-family:Calibri;   mso-hansi-theme-font:minor-latin;color:black'>　</span><a   name=\"SQM00O003\"></a><a name=\"SQM00OT03\"><span  style='mso-bookmark:SQM00O003'><span lang=\"EN-US\"  style='color:black'>(C)</span></span></a></span><span  style='mso-bookmark:SQM00OT03'></span><a name=\"SQM00OC03\"></a><span  style='mso-bookmark:SQM00OC03'><span style='mso-bookmark:  SQM00O003'><span style='mso-bookmark:SQM00B000'><span  style='font-family:\"新細明體\",\"serif\";   mso-ascii-font-family:Calibri;mso-ascii-theme-font:minor-latin; mso-fareast-font-family: 新細明體;mso-fareast-theme-font:minor-fareast;  mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin;  color:black'>鋒面雨</span></span></span></span><span  style='mso-bookmark:SQM00B000'><span style='font-family:\"新細明體\",  \"serif\"; mso-ascii-font-family:Calibri; mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體;  mso-fareast-theme-font:minor-fareast;mso-hansi-font-family:Calibri;   mso-hansi-theme-font:minor-latin;color:black'>　</span><a   name=\"SQM00O004\"></a><a name=\"SQM00OT04\"><span  style='mso-bookmark:SQM00O004'><span lang=\"EN-US\"  style='color:black'>(D)</span></span></a></span><span  style='mso-bookmark:SQM00OT04'></span><a name=\"SQM00OC04\"></a><span  style='mso-bookmark:SQM00OC04'><span style='mso-bookmark:  SQM00O004'><span style='mso-bookmark:SQM00B000'><span  style='font-family:\"新細明體\",\"serif\";   mso-ascii-font-family:Calibri;mso-ascii-theme-font:minor-latin; mso-fareast-font-family: 新細明體;mso-fareast-theme-font:minor-fareast;  mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin;  color:black'>颱風雨</span></span></span></span></p> </div> ",
            "answer": " <div class=\"WordSection1\" style='layout-grid:18.0pt'> <p  class=\"MsoNormal\"><a name=\"SAM00B000\"><span style='font-family:\"新  細明體\",\"serif\"; mso-ascii-font-family:Calibri;  mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體; mso-fareast-theme-font:minor-fareast;mso-hansi-font-family:Calibri;  mso-hansi-theme-font:minor-latin;color:black'>答案：</span></a><a  name=\"SAM00BC00\"><span style='mso-bookmark:SAM00B000'><span  lang=\"EN-US\" style='color:black'>(B)</span></span></a></p> </div> ",
            "analyze": ""
          },
          "updateTime": "2021-08-26T15:29:28.74Z"
        }
      ]
    }
  }
  ```

   > 欄位說明
  >| Name      | Meaning 
  > ---------- | ------- 
  > searchKey  | 複查Key
  > | <font color=#66B3FF> question </font>| <font color=#66B3FF>  試題資料 </font>
  > uid	       | 試題UID
  > image      | 完整試題資訊圖檔 (包含試題 + 答案 + 解析)
  > questionImage | 試題圖檔 (只有試題內文 & 選項)
  > answer  | 答案文字內容
  > answerPosition | 答案位置 (只有選擇題有值，含單選、多選、題組)
  > | <font color=#66B3FF> metadata </font> | <font color=#66B3FF>  試題屬性 </font>
  > code    | 屬性代碼
  > name    | 屬性名稱
  > content | 屬性內容
  > | <font color=#66B3FF> htmlParts </font> | <font color=#66B3FF> 拆分後的html </font>
  > content    | 試題內容 (試題內文 & 選項)
  > answer     | 答案
  > analyze    | 解析
  > updateTime | 最後維護時間
  >|
---

### **<font color=#66B3FF>[GET] /api/Service/CacheQuery </font>**
> 重複取得查詢結果

* ### Header
  > 帶入service token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
  >| Name     | Meaning
  > --------- | ---------------------
  > searchKey | 複查Key

* ### Response
  *直接回傳* "**<font color=#1AFD9C>[POST] /api/Service/CacheQuery </font>**" *Response 的 question*
---

### **<font color=#1AFD9C>[POST] /api/Service/AutoPattern/Query </font>**
> 查詢試題並寫入暫存

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
    "identity": "Student",
    "education": "J",
    "subject": "GE",
    "keys": [
      "JGE-18-1-2","JGE-3-4-1"
    ]
  }
  ```
  >|| Name     | Meaning
  >|-| --------- | ---------------------
  >| <b><font color=#FF0000>*</font></b> | identity  | 使用身分
  >| <b><font color=#FF0000>*</font></b> | education | 學制代碼
  >| <b><font color=#FF0000>*</font></b> | subject   | 科目代碼
  >| <b><font color=#FF0000>*</font></b> | keys      | 查詢Key (知識向度代碼)

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-08-30T16:02:34.7793815+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "searchKey": "82d59b2052a0403db88d036eda6737e6",
      "questionGroup": {
        "BEGIN": {
          "code": "BEGIN",
          "name": "易",
          "question": [
            {
              "uid": "04bc774c86354bd7846d931e15dae30a",
              "difficulty": "BEGIN",
              "difficultyName": "易",
              "quesType": "SS017",
              "quesTypeName": "單選題",
              "answerAmount": 1,
              "keys": [
                {
                  "code": "JGE-18-1-2",
                  "name": "東南亞氣候"
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
              "uid": "ed136a40203448fabed9f4cf4cde7c94",
              "difficulty": "BASIC",
              "difficultyName": "中偏易",
              "quesType": "SS017",
              "quesTypeName": "單選題",
              "answerAmount": 1,
              "keys": [
                {
                  "code": "JGE-3-4-1",
                  "name": "地形災害與環境倫理"
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
    "systemNow": "2021-08-30T16:10:44.1822854+08:00",
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

### **<font color=#66B3FF>[GET] /Service/AutoPattern/Cache </font>**
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
  > searchKey | 複查Key

* ### Response
  *查詢成功直接回傳* "**<font color=#1AFD9C>[POST] /api/Service/AutoPattern/Query
 </font>**" *的Response*

  > 暫存逾期
  ```json
  {
    "systemCode": "0501",
    "isSuccess": false,
    "systemNow": "2021-08-30T16:26:43.8236454+08:00",
    "message": "Cache Expired.",
    "disposal": null,
    "data": null
  }
  ```
---


### **<font color=#1AFD9C>[POST] /api/Service/ExamPaper/OTP </font>**
> 非公開試卷授權

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
    "expireAt": 1631710634,
    "examUID": "110-68faa117a2bb403884606d050f1b566e",
    "optCode": "test",
    "userUID": [ "testUser" ]
  }
  ```

  >|  | Name     | Meaning
  >| ---------| --------- | ---------------------
  >| <b><font color=#FF0000>*</font></b> |  expireAt  | 逾期timestamp
  >| <b><font color=#FF0000>*</font></b> |  examUID   | 試卷UID
  >| <b><font color=#FF0000>*</font></b> |  optCode   | 授權碼
  >|| userUID   | 有帶入才檢核指定使用者

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-09-15T19:18:19.450274+08:00",
    "message": "",
    "disposal": null,
    "data": null
  }
  ```

  > 試卷不存在
  ```json
  {
    "systemCode": "0400",
    "isSuccess": false,
    "systemNow": "2021-09-15T19:18:47.2087976+08:00",
    "message": "exam-paper資料：不存在, 請確認輸入內容。",
    "disposal": null,
    "data": null
  }
  ```
---

### **<font color=#66B3FF>[GET] /api/Service/ExamPaper </font>**
> 查詢系統預設卷

* ### Header
  > 帶入service token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
  >| Name  | Meaning
  > -------| ---------------------
  > year   | 查詢起始年 (預設查詢三年內資料)
  > tag    | 篩選標籤代碼

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-09-30T19:26:42.8501478+08:00",
    "message": "",
    "disposal": null,
    "data": [
      {
        "uid": "110-01f890bfac06440395e99f454362ccc1",
        "name": "110學年度國中地理測驗(答數 25 ，總分 100 分)",
        "eduSubjectName": "國中地理",
        "examTypeName": "段考",
        "drawUpName": "電腦命題",
        "isPublic": true,
        "questionAmount": 25,
        "score": 2500,
        "examType": "Exam",
        "drawUp": "FastPattern",
        "year": "110",
        "yearName": "110學年度",
        "education": "J",
        "educationName": "國中",
        "subject": "GE",
        "subjectName": "地理",
        "tags": [
          "liveoneclass"
        ],
        "maintainer": "[liveedu006] 11",
        "createTime": "2021-10-01T04:04:54.023Z",
        "lastUpdateTime": "2021-10-01T04:05:26.302Z"
      }
    ]
  }
  ```

  > 欄位說明
  >| Name        | Meaning 
  > ------------ | ------- 
  > uid          | 試卷UID
  > name         | 試卷名稱
  > eduSubjectName | 學制科目
  > examTypeName   | 考試別
  > drawUpName     | 出卷方式
  > isPublic       | 是否為公開卷
  > questionAmount | 試題總數
  > score       | 試卷總分
  > examType    | 考試別代碼
  > drawUp      | 出卷方式代碼
  > year        | 學年度代碼
  > yearName    | 學年度
  > education   | 學制代碼
  > eduName     | 學制
  > subject     | 科目代碼
  > subjectName | 科目
  > tags        | 試卷標籤
  > maintainer  | 出卷者
  > createTime  | 建立時間
  > updateTime  | 最後維護時間
---

### **<font color=#66B3FF>[GET] /api/Service/ExamPaper/{UID}</font>**
> 讀取試卷明細

* ### Header
  > 帶入service token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
  >|| Name | Meaning
  >|--| ------| -------------
  >|<b><font color=#FF0000>*</font></b>|UID   | 試卷UID
  >||opt   | 授權碼
  >||user  | 使用者識別碼
  >||authorUID | 出卷者UID (有傳入才能讀取進階設定)

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-12-30T10:22:45.4277897+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "uid": "110-56c8a17a39e34e76840c9f993e7f4c2d",
      "examPaperInfo": {
        "examTypeName": "其他",
        "drawUpName": "電腦命題",
        "isPublic": true,
        "score": 5000,
        "examType": "Others",
        "drawUp": "FastPattern",
        "tags": [
          "liveoneclass"
        ],
        "uid": "110-56c8a17a39e34e76840c9f993e7f4c2d",
        "name": "110學年度國中地理測驗(答數50,100分)",
        "eduSubjectName": "國中地理",
        "questionAmount": 50,
        "year": "110",
        "yearName": "110學年度",
        "education": "J",
        "educationName": "國中",
        "subject": "GE",
        "subjectName": "地理",
        "bookNames": [
          "第一冊",
          "第二冊",
          "第三冊",
          "第四冊"
        ],
        "maintainer": "[liveedu006] 11",
        "createTime": "2021-10-01T03:11:44.995Z",
        "lastUpdateTime": "2021-10-01T03:12:36.329Z"
      },
      "questionGroup": [
        {
          "typeCode": "SS017",
          "typeName": "單選題",
          "scoreType": "PerAnswer",
          "score": 100,
          "questionList": [
            {
              "sequence": 1,
              "id": "7fde043603824d3c8adf02c07d55f9eb",
              "answerAmount": 1,
              "score": 100
            },
            {
              "sequence": 2,
              "id": "c5fe57d03bbe4409b632aa099f624a10",
              "answerAmount": 1,
              "score": 100
            }
          ]
        }
      ],
      "questionInfo": [
        {
          "uid": "004de72dba2845fb9a7035c15a4cc478",
          "image": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJGE%2F004de72dba2845fb9a7035c15a4cc478%2F004de72dba2845fb9a7035c15a4cc478.gif?alt=media&token=9f8507fe-55a3-418d-bc10-95b9ecfb9d09",
          "questionImage": {
            "question": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJGE%2F004de72dba2845fb9a7035c15a4cc478%2FQuestionOverall.gif?alt=media&token=65f31ac6-06bd-449a-a695-468027b0bc81",
            "content": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJGE%2F004de72dba2845fb9a7035c15a4cc478%2FQuestionContent.gif?alt=media&token=80a62fc6-593e-4e29-a625-9f3a0cacd48d",
            "subQuestions": [
              {
                "question": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJGE%2F004de72dba2845fb9a7035c15a4cc478%2FQuestionContent.gif?alt=media&token=80a62fc6-593e-4e29-a625-9f3a0cacd48d",
                "options": [
                  "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJGE%2F004de72dba2845fb9a7035c15a4cc478%2Foptions_a.gif?alt=media&token=f74ca8b6-f61d-4ea8-b927-3df00f8b8821",
                  "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJGE%2F004de72dba2845fb9a7035c15a4cc478%2Foptions_b.gif?alt=media&token=1a41ee32-c904-4531-8e2e-67d259fd3737",
                  "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJGE%2F004de72dba2845fb9a7035c15a4cc478%2Foptions_c.gif?alt=media&token=3abf9904-d350-49e0-bc21-f250c6970e71",
                  "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJGE%2F004de72dba2845fb9a7035c15a4cc478%2Foptions_d.gif?alt=media&token=35462fc4-1ea4-4ebc-a114-e8cc8cf0ef98"
                ]
              }
            ],
            "answer": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJGE%2F004de72dba2845fb9a7035c15a4cc478%2FAnswer.gif?alt=media&token=288cb3eb-ed5f-41b7-886f-0a172faa8d09",
            "analyze": null
          },
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
                  "code": "1-2-4",
                  "name": "1-2-4"
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
                  "code": "地AA-IV-1",
                  "name": "全球經緯度座標系統。"
                }
              ]
            },
            {
              "code": "KNOWLEDGE",
              "name": "知識向度",
              "content": [
                {
                  "code": "JGE-1-1-3",
                  "name": "比例尺"
                }
              ]
            },
            {
              "code": "TOPIC",
              "name": "主題",
              "content": [
                {
                  "code": "JGE-1",
                  "name": "地圖與座標系統"
                }
              ]
            }
          ],
          "answerInfos": [
            {
              "index": 1,
              "answerType": "SS",
              "answerTypeName": "單一選擇題",
              "answerAmount": 1,
              "answer": [
                "(D)"
              ],
              "position": [
                4
              ]
            }
          ],
          "htmlParts": {
            "content": " <div class=\"WordSection1\" style='layout-grid:18.0pt'> <p class=\"MsoNormal\"><a name=\"SQM00B000\"></a><a name=\"SQM00BC00\"><span style='mso-bookmark:SQM00B000'><span style='font-family:\"新細明體\",\"serif\"; mso-ascii-font-family:Calibri;mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體;mso-fareast-theme-font:minor-fareast;mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin'>若地圖中沒有標出方位，也沒有經緯度可供判斷時，通常圖的上方是指哪個方向？</span></span></a><a name=\"SQM00O001\"></a><a name=\"SQM00OT01\"><span style='mso-bookmark:SQM00O001'><span style='mso-bookmark:SQM00B000'><span lang=\"EN-US\"><br> (A)</span></span></span></a><span style='mso-bookmark:SQM00OT01'></span><a name=\"SQM00OC01\"></a><span style='mso-bookmark:SQM00OC01'><span style='mso-bookmark: SQM00B000'><span style='mso-bookmark:SQM00O001'><span style='font-family:\"新細明體\",\"serif\"; mso-ascii-font-family:Calibri;mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體;mso-fareast-theme-font:minor-fareast;mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin'>東方</span></span></span></span><a name=\"SQM00O002\"></a><a name=\"SQM00OT02\"><span style='mso-bookmark:SQM00O002'><span style='mso-bookmark:SQM00B000'><span lang=\"EN-US\"><br> (B)</span></span></span></a><span style='mso-bookmark:SQM00OT02'></span><a name=\"SQM00OC02\"></a><span style='mso-bookmark:SQM00OC02'><span style='mso-bookmark: SQM00B000'><span style='mso-bookmark:SQM00O002'><span style='font-family:\"新細明體\",\"serif\"; mso-ascii-font-family:Calibri;mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體;mso-fareast-theme-font:minor-fareast;mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin'>西方</span></span></span></span><a name=\"SQM00O003\"></a><a name=\"SQM00OT03\"><span style='mso-bookmark:SQM00O003'><span style='mso-bookmark:SQM00B000'><span lang=\"EN-US\"><br> (C)</span></span></span></a><span style='mso-bookmark:SQM00OT03'></span><a name=\"SQM00OC03\"></a><span style='mso-bookmark:SQM00OC03'><span style='mso-bookmark: SQM00B000'><span style='mso-bookmark:SQM00O003'><span style='font-family:\"新細明體\",\"serif\"; mso-ascii-font-family:Calibri;mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體;mso-fareast-theme-font:minor-fareast;mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin'>南方</span></span></span></span><a name=\"SQM00O004\"></a><a name=\"SQM00OT04\"><span style='mso-bookmark:SQM00O004'><span style='mso-bookmark:SQM00B000'><span lang=\"EN-US\"><br> (D)</span></span></span></a><span style='mso-bookmark:SQM00OT04'></span><a name=\"SQM00OC04\"></a><span style='mso-bookmark:SQM00OC04'><span style='mso-bookmark: SQM00B000'><span style='mso-bookmark:SQM00O004'><span style='font-family:\"新細明體\",\"serif\"; mso-ascii-font-family:Calibri;mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體;mso-fareast-theme-font:minor-fareast;mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin'>北方</span></span></span></span></p> </div> ",
            "answer": " <div class=\"WordSection1\" style='layout-grid:18.0pt'> <p class=\"MsoNormal\"><a name=\"SAM00B000\"><span style='font-family:\"新細明體\",\"serif\"; mso-ascii-font-family:Calibri;mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體;mso-fareast-theme-font:minor-fareast;mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin'>答案：</span></a><a name=\"SAM00BC00\"><span style='mso-bookmark:SAM00B000'><span lang=\"EN-US\">(D)</span></span></a></p> </div> ",
            "analyze": ""
          },
          "updateTime": "2021-09-10T15:41:43.984Z"
        }
      ],
      "setting": {
        "advanced": [
          "ChangeOrder",
          "ChangeOption"
        ]
      }
    }
  }
  ```

  > 欄位說明
  >| Name        | Meaning  | Remark
  > ------------ | -------  | -----
  > uid          | 試卷UID
  > | <font color=#66B3FF> examPaperInfo </font> | <font color=#66B3FF> 試卷資訊 </font>
  > uid          | 試卷UID
  > name         | 試卷名稱
  > eduSubjectName | 學制科目
  > examTypeName   | 考試別
  > drawUpName     | 出卷方式
  > isPublic       | 是否為公開卷
  > questionAmount | 試題總數
  > score       | 試卷總分
  > examType    | 考試別代碼
  > drawUp      | 出卷方式代碼
  > year        | 學年度代碼
  > yearName    | 學年度
  > education   | 學制代碼
  > eduName     | 學制
  > subject     | 科目代碼
  > subjectName | 科目
  > maintainer  | 出卷者
  > createTime  | 建立時間
  > updateTime  | 最後維護時間
  > | <font color=#66B3FF> questionGroup </font> | <font color=#66B3FF> 題型群組 </font>
  > typeCode   | 題型代碼
  > typeName   | 題型名稱
  > scoreType  | 配分方式 | PerQuestion:"每題配分", PerAnswer:"每答配分"
  > score      | 配分分數
  > | <font color=#66B3FF> .questionList </font> | <font color=#66B3FF> 試題清單 </font>
  > sequence     | 題號
  > id           | 試題ID
  > answerAmount | 答案數
  > score        | 該題配分 | 不區分每題或每答, 為該題總分 
  > | <font color=#66B3FF> questionInfo </font> | <font color=#66B3FF> 試題資訊</font>
  > uid	       | 試題UID
  > image      | 完整試題資訊圖檔 | 包含試題 + 答案 + 解析
  > | <font color=#66B3FF> .questionImage </font> | <font color=#66B3FF> 試題圖檔  </font>
  > question | 完整試題 | 題目+選項
  > content  | 題幹
  > answer   | 答案
  > analyze  | 解析
  > | <font color=#66B3FF> .subQuestions </font> | <font color=#66B3FF> 子題
  > question | 題目
  > options  | 選項
  > | <font color=#66B3FF> .answerInfos </font> | <font color=#66B3FF> 答案資訊 </font>
  > index       | 題號/子題號
  > answerType  | 作答方式代碼       
  > answerTypeName | 作答方式 |  **<font color=#FF0000> 2022/01/18 新增 </font>**
  > answerAmount | 答案數
  > answer   | 答案文字內容
  > position | 選項位置
  > answerPosition | 答案位置 | 只有選擇題有值，含單選、多選、題組
  > | <font color=#66B3FF> .metadata </font> | <font color=#66B3FF>  試題屬性 </font>
  > code    | 屬性代碼
  > name    | 屬性名稱
  > content | 屬性內容
  > | <font color=#66B3FF> htmlParts </font> | <font color=#66B3FF> 拆分後的html </font>
  > content    | 試題內容 | 試題內文 & 選項
  > answer     | 答案
  > analyze    | 解析
  > updateTime | 最後維護時間
  > | <font color=#66B3FF> setting </font> | <font color=#66B3FF> 進階設定 </font>
  > advanced   | 設定選項
  >|

  ### ※ 進階設定 ※
  >| Name       | Meaning
  > ----------- | -----------
  > ChangeOrder | 變換題序
  > ChangeOption| 變換選項序

* ### **<font color=#FF0000> QuestionImage 範例 </font>**
  > 單選題
  ```json
  
  {
    "question": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJGE%2F2c1d1c4e522e4f8298de8b1039399131%2FQuestionOverall.gif?alt=media&token=da8e4b71-ee39-4575-a0b1-d75e1d6b86c4",
    "content": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJGE%2F2c1d1c4e522e4f8298de8b1039399131%2FQuestionContent.gif?alt=media&token=a9832da5-87f5-409d-80ce-f73775c424ae",
    "subQuestions": [
      {
        "question": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJGE%2F2c1d1c4e522e4f8298de8b1039399131%2FQuestionContent.gif?alt=media&token=a9832da5-87f5-409d-80ce-f73775c424ae",
        "options": [
          "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJGE%2F2c1d1c4e522e4f8298de8b1039399131%2Foptions_a.gif?alt=media&token=4fd7239f-80ac-49bb-99a5-5dcc4bee2670",
          "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJGE%2F2c1d1c4e522e4f8298de8b1039399131%2Foptions_b.gif?alt=media&token=f50936fd-c54b-47ab-bba7-860130cf16f9",
          "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJGE%2F2c1d1c4e522e4f8298de8b1039399131%2Foptions_c.gif?alt=media&token=d84ac8ad-6893-44f4-9731-a94e50d6599c",
          "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJGE%2F2c1d1c4e522e4f8298de8b1039399131%2Foptions_d.gif?alt=media&token=ee142b21-da55-4fec-b572-1e208b613554"
        ]
      }
    ],
    "answer": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJGE%2F2c1d1c4e522e4f8298de8b1039399131%2FAnswer.gif?alt=media&token=c2f0867c-2c89-4f3e-bfcd-7dff370a11fc",
    "analyze": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJGE%2F2c1d1c4e522e4f8298de8b1039399131%2FAnalyze.gif?alt=media&token=018d5f71-f2df-46bb-9559-55c2522be85d"
  }
  ```

  > 題組
  ```json
  {
    "question": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJPC%2F4631216ffb804b48a3a119b980df53b0%2FQuestionOverall.gif?alt=media&token=bb939e2e-c544-4444-9eac-5c4f008e2a2b",
    "content": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJPC%2F4631216ffb804b48a3a119b980df53b0%2FQuestionContent.gif?alt=media&token=d0aec1a6-29c9-4876-a84c-d2afead15b32",
    "subQuestions": [
      {
        "question": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJPC%2F4631216ffb804b48a3a119b980df53b0%2FSubQuestion1.gif?alt=media&token=8877f006-0bee-4488-8fbd-4e18b85b9f08",
        "options": [
          "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJPC%2F4631216ffb804b48a3a119b980df53b0%2Foptions_q1_a.gif?alt=media&token=d679f2d5-35a2-4350-a118-18a8e1338b3d",
          "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJPC%2F4631216ffb804b48a3a119b980df53b0%2Foptions_q1_b.gif?alt=media&token=0d23eff1-ac6c-4f9b-acdc-47f403ccb6b1",
          "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJPC%2F4631216ffb804b48a3a119b980df53b0%2Foptions_q1_c.gif?alt=media&token=19e6e9e3-908d-44d7-bda4-3e5077aa5731",
          "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJPC%2F4631216ffb804b48a3a119b980df53b0%2Foptions_q1_d.gif?alt=media&token=5a1d9c31-b26f-4fa4-9be1-7b68282611a8"
        ]
      },
      {
        "question": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJPC%2F4631216ffb804b48a3a119b980df53b0%2FSubQuestion2.gif?alt=media&token=6a567b5d-8427-45ce-b053-f7ec0acedd4e",
        "options": [
          "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJPC%2F4631216ffb804b48a3a119b980df53b0%2Foptions_q2_a.gif?alt=media&token=fad966e1-c8cf-431c-86ce-5a40f6be58b6",
          "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJPC%2F4631216ffb804b48a3a119b980df53b0%2Foptions_q2_b.gif?alt=media&token=4e24792c-524d-4c6f-8800-66ac9e8ad34e",
          "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJPC%2F4631216ffb804b48a3a119b980df53b0%2Foptions_q2_c.gif?alt=media&token=8de7ec5f-7560-4d6a-bb75-372f039aee2a",
          "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJPC%2F4631216ffb804b48a3a119b980df53b0%2Foptions_q2_d.gif?alt=media&token=4b36b3f3-bf1c-4c9c-9071-847d4bfe3b19"
        ]
      }
    ],
    "answer": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJPC%2F4631216ffb804b48a3a119b980df53b0%2FAnswer.gif?alt=media&token=c815b2a0-a095-4ab9-8578-4581156bfdd8",
    "analyze": "https://firebasestorage.googleapis.com/v0/b/question-bank-dev.appspot.com/o/question-bank%2FJPC%2F4631216ffb804b48a3a119b980df53b0%2FAnalyze.gif?alt=media&token=81ecfb5f-e890-489e-b2ad-f3594108d77e"
  }
   ```
---


### **<font color=#1AFD9C>[POST] /api/Service/Practice/Create </font>**
> 對接服務建立自主練習

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
    "searchKey": "1fdfe09ea0ed4e22a10cf6982270dac9",
    "year":"110",
    "bookID":"110N-JMAB01",
    "name": "對接服務建立自主練習測試",
    "questionGroup": [
      {
        "typeCode": "SS017",
        "questionList": [
          "7451af22d0a84fc4a8a43372241a3591",
          "238a6a3a4f234c33ba8a991911129595",
          "a7397d861a6b4298bc0afb97778e7760"
        ]
      }
    ]
  }
  ```
  >|| Name     | Meaning
  >|-| ----------| ---------------------
  >| <b><font color=#FF0000>*</font></b> | searchKey | 複查Key
  >| <b><font color=#FF0000>*</font></b> | year      | 學年度
  >| <b><font color=#FF0000>*</font></b> | bookID    | 課本代碼
  >| <b><font color=#FF0000>*</font></b> | name      | 練習名稱
  >| <b><font color=#FF0000>*</font></b> | <font color=#66B3FF> questionGroup 試題群組 </font>
  >| | typeCode     | 題型代碼 (quesType)
  >| | questionList | 試題ID

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-10-12T15:28:38.5801878+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "type": "Practice",
      "uid": "110-34d4cc630fd846b99531fdeac57a095a"
    }
  }
  ```
  >| Name      | Meaning
  >| ----------| ---------------------
  >| systemCode | 系統狀態代碼
  >| isSuccess  | 操作是否成功
  >| systemNow  | 系統時間
  > | <font color=#66B3FF> data </font> | <font color=#66B3FF> 回傳資訊 </font>
  >| type       | 類別代碼 (自主練習固定為 "Practice")
  >| uid        | 自主練習UID
---

### **<font color=#66B3FF>[GET] /api/Service/Practice/Query </font>**
> 查詢自主練習 (預設查回三年內資料)

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
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-10-08T22:21:56.1319593+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "eduSubjectMap": [
        {
          "code": "JMA",
          "name": "國中數學"
        }
      ],
      "practice": [
        {
          "uid": "110-64108d6055b445e18c3901c5440b9f3d",
          "name": "對接服務建立自主練習測試",
          "eduSubject": "JMA",
          "eduSubjectName": "國中數學",
          "amount": 3,
          "createTime": "2021-10-08T14:20:52.837Z"
        }
      ]
    }
  }
  ```
  > | Name | Meaning |
  > | ---- |---------|
  > | eduSubjectMap  | 學制科目選單
  > | <font color=#66B3FF> practice </font> | <font color=#66B3FF> 自主練習 </font>
  > | uid  | UID
  > | name | 名稱
  > | eduSubject | 學制科目
  > | eduSubjectName | 學制科目名稱
  > | amount | 題數
  > | createTime | 建立時間
---

### **<font color=#66B3FF>[GET] /api/Service/Practice/Info </font>**
> 查詢自主練習試題資訊

* ### Header
  > 帶入service token
  ```json
  {
    "Authorization": "<token>"
  }
  ```
* ### Payload
  > | Name | Meaning |
  > | ---- |---------|
  > | UID  | 自主練習代碼
  

* ### Response
  ```json
  {
    "systemCode": "0200",
    "isSuccess": true,
    "systemNow": "2021-10-08T22:52:52.4460692+08:00",
    "message": "",
    "disposal": null,
    "data": {
      "uid": "110-64108d6055b445e18c3901c5440b9f3d",
      "practiceInfo": {
        "uid": "110-64108d6055b445e18c3901c5440b9f3d",
        "name": "對接服務建立自主練習測試",
        "eduSubjectName": "國中數學",
        "questionAmount": 3,
        "year": "110",
        "yearName": "110學年度",
        "education": "J",
        "educationName": "國中",
        "subject": "MA",
        "subjectName": "數學",
        "maintainer": "[OneLive] OneLive",
        "createTime": "2021-10-08T14:20:52.837Z",
        "lastUpdateTime": "2021-10-08T14:20:52.837Z"
      },
      "questionGroup": [
        {
          "typeCode": "SS017",
          "typeName": "單選題",
          "scoreType": "PerQuestion",
          "score": 33,
          "questionList": [
            {
              "sequence": 1,
              "id": "7451af22d0a84fc4a8a43372241a3591",
              "answerAmount": 1,
              "score": 33
            },
            {
              "sequence": 2,
              "id": "238a6a3a4f234c33ba8a991911129595",
              "answerAmount": 1,
              "score": 33
            },
            {
              "sequence": 3,
              "id": "a7397d861a6b4298bc0afb97778e7760",
              "answerAmount": 1,
              "score": 33
            }
          ]
        }
      ],
      "questionInfo": [
        {
          "uid": "a7397d861a6b4298bc0afb97778e7760",
          "image": "https://firebasestorage.googleapis.com/v0/b/  question-bank-dev.appspot.com/o/  question-bank%2FJMA%2Fa7397d861a6b4298bc0afb97778e7760%2Fa7397d861a6b 4298bc0afb97778e7760.gif?alt=media&  token=8bc35137-3b86-4755-9967-58faaf834375",
          "questionImage": {
            "question": "https://firebasestorage.googleapis.com/v0/b/ question-bank-dev.appspot.com/o/ question-bank%2FJMA%2Fa7397d861a6b4298bc0afb97778e7760%2FQuestionOv  erall.gif?alt=media&token=2c8c47b1-67a6-48eb-b41f-a222e84de078",
            "answer": "https://firebasestorage.googleapis.com/v0/b/ question-bank-dev.appspot.com/o/ question-bank%2FJMA%2Fa7397d861a6b4298bc0afb97778e7760%2FAnswer. gif?alt=media&token=c7e2df3e-12e7-48aa-a22b-7ef834c961db",
            "analyze": "https://firebasestorage.googleapis.com/v0/b/  question-bank-dev.appspot.com/o/  question-bank%2FJMA%2Fa7397d861a6b4298bc0afb97778e7760%2FAnalyze. gif?alt=media&token=24c24116-bc88-4ca6-9c4e-c5b9a93fc6a0"
          },
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
                  "code": "N-4-10",
                  "name": "N-4-10"
                }
              ]
            },
            {
              "code": "SUB_SOURCE",
              "name": "來源",
              "content": [
                {
                  "code": "119",
                  "name": "補充試題"
                }
              ]
            },
            {
              "code": "SOURCE",
              "name": "出處",
              "content": [
                {
                  "code": "NS017",
                  "name": "補充試題"
                }
              ]
            },
            {
              "code": "LEARN_CONTENT",
              "name": "學習內容",
              "content": [
                {
                  "code": "N-7-7",
                  "name": "指數律：以數字例表示「同 底數的乘法指數律」（am ×an =  am+n、(am)n=amn、(a×b)n=an×bn，其中 m, n為非負整數）；以數字 例  表示「同底數的除法指數律」（am÷an= am-n，其中 m≥n 且m,n 為非負整  數）。"
                }
              ]
            },
            {
              "code": "KNOWLEDGE",
              "name": "知識向度",
              "content": [
                {
                  "code": "JMA-1-4-3",
                  "name": "比較科學記號的大小"
                }
              ]
            },
            {
              "code": "TOPIC",
              "name": "主題",
              "content": [
                {
                  "code": "JMA-1",
                  "name": "數的運算"
                }
              ]
            }
          ],
          "answerInfos": [
            {
              "index": 1,
              "answerType": "SS",
              "answerTypeName": "單一選擇題",
              "answerAmount": 1,
              "answer": [
                "(C)"
              ],
              "position": [
                3
              ]
            }
          ],
          "htmlParts": {
            "content": " <div class=\"WordSection1\" style='layout-grid:18. 0pt'> <p class=\"MsoNormal\"><a name=\"SQM00B000\"></a><a  name=\"SQM00BC00\"><span style='mso-bookmark:SQM00B000'><span  style='font-family:\"新細明體\",\"serif\";   mso-ascii-font-family:Calibri;mso-ascii-theme-font:minor-latin; mso-fareast-font-family: 新細明體; mso-fareast-theme-font:minor-fareast;  mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin'>下 列哪一個數值最大？</span></span></a><a name=\"SQM00O001\"></a><a   name=\"SQM00OT01\"><span style='mso-bookmark:SQM00O001'><span   style='mso-bookmark:SQM00B000'><span lang=\"EN-US\"><br> (A)</  span></span></span></a><a name=\"SQM00OC01\"><span  style='mso-bookmark:SQM00B000'><span   style='mso-bookmark:SQM00O001'><span lang=\"EN-US\">9.5</span></  span></span></a><span style='mso-bookmark:SQM00OC01'><span  style='mso-bookmark:SQM00B000'><span   style='mso-bookmark:SQM00O001'><span style='font-family:華康中明體; mso-ascii-font-family: Calibri;  mso-ascii-theme-font:minor-latin'>×</span><span lang=\"EN-US\">10</ span></span></span></span><span  style='mso-bookmark:SQM00OC01'><span   style='mso-bookmark:SQM00B000'><span  style='mso-bookmark:SQM00O001'><sup><span style='font-family:\"新細  明體\",\"serif\"; mso-ascii-font-family:Calibri;  mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體; mso-fareast-theme-font:minor-fareast;  mso-hansi-font-family:Calibri;  mso-hansi-theme-font:minor-latin'>－</span><span lang=\"EN-US\">8</  span></sup></span></span></span><a name=\"SQM00O002\"></a><a  name=\"SQM00OT02\"><span style='mso-bookmark:SQM00O002'><span  style='mso-bookmark:SQM00B000'><sup><span lang=\"EN-US\"><br> (B)</  span></sup></span></span></a><a name=\"SQM00OC02\"><span  style='mso-bookmark: SQM00B000'><span  style='mso-bookmark:SQM00O002'><span lang=\"EN-US\">2.5</span></ span></span></a><span style='mso-bookmark:SQM00OC02'><span   style='mso-bookmark:SQM00B000'><span  style='mso-bookmark:SQM00O002'><span style='font-family:華康中明體;  mso-ascii-font-family: Calibri; mso-ascii-theme-font:minor-latin'>×</span><span lang=\"EN-US\">10</  span></span></span></span><span   style='mso-bookmark:SQM00OC02'><span  style='mso-bookmark:SQM00B000'><span   style='mso-bookmark:SQM00O002'><sup><span style='font-family:\"新細 明體\",\"serif\"; mso-ascii-font-family:Calibri; mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體;  mso-fareast-theme-font:minor-fareast; mso-hansi-font-family:Calibri;   mso-hansi-theme-font:minor-latin'>－</span><span lang=\"EN-US\">8</ span></sup></span></span></span><a name=\"SQM00O003\"></a><a   name=\"SQM00OT03\"><span style='mso-bookmark:SQM00O003'><span   style='mso-bookmark:SQM00B000'><sup><span lang=\"EN-US\"><br> (C)</ span></sup></span></span></a><a name=\"SQM00OC03\"><span   style='mso-bookmark: SQM00B000'><span   style='mso-bookmark:SQM00O003'><span lang=\"EN-US\">4.7</span></  span></span></a><span style='mso-bookmark:SQM00OC03'><span  style='mso-bookmark:SQM00B000'><span   style='mso-bookmark:SQM00O003'><span style='font-family:華康中明體; mso-ascii-font-family: Calibri;  mso-ascii-theme-font:minor-latin'>×</span><span lang=\"EN-US\">10</ span></span></span></span><span  style='mso-bookmark:SQM00OC03'><span   style='mso-bookmark:SQM00B000'><span  style='mso-bookmark:SQM00O003'><sup><span style='font-family:\"新細  明體\",\"serif\"; mso-ascii-font-family:Calibri;  mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體; mso-fareast-theme-font:minor-fareast;  mso-hansi-font-family:Calibri;  mso-hansi-theme-font:minor-latin'>－</span><span lang=\"EN-US\">7</  span></sup></span></span></span><a name=\"SQM00O004\"></a><a  name=\"SQM00OT04\"><span style='mso-bookmark:SQM00O004'><span  style='mso-bookmark:SQM00B000'><sup><span lang=\"EN-US\"><br> (D)</  span></sup></span></span></a><a name=\"SQM00OC04\"><span  style='mso-bookmark: SQM00B000'><span  style='mso-bookmark:SQM00O004'><span lang=\"EN-US\">3.5</span></ span></span></a><span style='mso-bookmark:SQM00OC04'><span   style='mso-bookmark:SQM00B000'><span  style='mso-bookmark:SQM00O004'><span style='font-family:華康中明體;  mso-ascii-font-family: Calibri; mso-ascii-theme-font:minor-latin'>×</span><span lang=\"EN-US\">10</  span></span></span></span><span   style='mso-bookmark:SQM00OC04'><span  style='mso-bookmark:SQM00B000'><span   style='mso-bookmark:SQM00O004'><sup><span style='font-family:\"新細 明體\",\"serif\"; mso-ascii-font-family:Calibri; mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體;  mso-fareast-theme-font:minor-fareast; mso-hansi-font-family:Calibri;   mso-hansi-theme-font:minor-latin'>－</span><span lang=\"EN-US\">7</ span></sup></span></span></span></p> </div> ",
            "answer": " <div class=\"WordSection1\" style='layout-grid:18.  0pt'> <p class=\"MsoNormal\"><a name=\"SAM00B000\"><span  style='font-family:\"新細明體\",\"serif\";   mso-ascii-font-family:Calibri;mso-ascii-theme-font:minor-latin; mso-fareast-font-family: 新細明體; mso-fareast-theme-font:minor-fareast;  mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin'>答 案：</span></a><a name=\"SAM00BC00\"><span   style='mso-bookmark:SAM00B000'><span lang=\"EN-US\">(C)</span></  span></a></p> </div> ",
            "analyze": " <div class=\"WordSection1\" style='layout-grid:18. 0pt'> <p class=\"MsoNormal\"><a name=\"SZM00B000\"><span   style='font-family:\"新細明體\",\"serif\";  mso-ascii-font-family:Calibri;mso-ascii-theme-font:minor-latin;  mso-fareast-font-family: 新細明體;  mso-fareast-theme-font:minor-fareast; mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin'>解  析：</span><span lang=\"EN-US\"><o:p></o:p></span></a></p> <p   class=\"MsoNormal\"><span style='mso-bookmark:SZM00B000'><a   name=\"SZM00BC00\"><span style='font-family:\"新細明體\",\"serif\"; mso-ascii-font-family:Calibri;mso-ascii-theme-font: minor-latin; mso-fareast-font-family:新細明體;  mso-fareast-theme-font:minor-fareast;   mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin; color:black'>∵<span style='mso-font-width:33%'>　</span></ span><span lang=\"EN-US\" style='color:black'>10</span></a></  span><span style='mso-bookmark:SZM00B000'><span   style='mso-bookmark:SZM00BC00'><sup><span style='font-family:\"新細 明體\",\"serif\";mso-ascii-font-family:Calibri;  mso-ascii-theme-font: minor-latin;mso-fareast-font-family:新細明體; mso-fareast-theme-font:minor-fareast;  mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin;  color:black'>－</span><span lang=\"EN-US\" style='color:black'>7</  span></sup></span></span><span  style='mso-bookmark:SZM00B000'><span   style='mso-bookmark:SZM00BC00'><span style='font-family:\"新細明體  \",\"serif\";mso-ascii-font-family:Calibri;mso-ascii-theme-font:  minor-latin;mso-fareast-font-family:新細明體;  mso-fareast-theme-font:minor-fareast;   mso-hansi-font-family:Calibri;mso-hansi-theme-font:minor-latin; color:black'>＞</span><span lang=\"EN-US\" style='color:black'>10</  span></span></span><span style='mso-bookmark: SZM00B000'><span  style='mso-bookmark:SZM00BC00'><sup><span style='font-family: \"新 細明體\",\"serif\";mso-ascii-font-family:Calibri;  mso-ascii-theme-font:minor-latin; mso-fareast-font-family:新細明體; mso-fareast-theme-font:minor-fareast;mso-hansi-font-family:  Calibri;mso-hansi-theme-font:minor-latin;color:black'>－</ span><span lang=\"EN-US\" style='color:black'>8</span></sup></ span></span><span style='mso-bookmark:SZM00B000'><span   style='mso-bookmark:SZM00BC00'><span style='font-family:\"新細明體  \",\"serif\"; mso-ascii-font-family:Calibri;  mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體; mso-fareast-theme-font:minor-fareast;  mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin;  color:black'>，又</span><span lang=\"EN-US\" style='color:black'>4. 7</span></span></span><span style='mso-bookmark:SZM00B000'><span   style='mso-bookmark:SZM00BC00'><span style='font-family:\"新細明體  \",\"serif\"; mso-ascii-font-family:Calibri;  mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體; mso-fareast-theme-font:minor-fareast;  mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin;  color:black'>＞</span><span lang=\"EN-US\" style='color:black'>3. 5</span></span></span><span style='mso-bookmark:SZM00B000'><span   style='mso-bookmark:SZM00BC00'><span style='font-family:\"新細明體  \",\"serif\"; mso-ascii-font-family:Calibri;  mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體; mso-fareast-theme-font:minor-fareast;  mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin;  color:black'>　∴<span style='mso-font-width: 33%'>　</span></ span><span lang=\"EN-US\" style='color:black'>4.7</span></span></  span><span style='mso-bookmark:SZM00B000'><span   style='mso-bookmark:SZM00BC00'><span style='font-family:華康標宋體; color:black'>×</span><span lang=\"EN-US\" style='color:black'>10</ span></span></span><span style='mso-bookmark:SZM00B000'><span  style='mso-bookmark:SZM00BC00'><sup><span style='font-family:\"新細  明體\",\"serif\"; mso-ascii-font-family:Calibri;  mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體; mso-fareast-theme-font:minor-fareast;  mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin;  color:black'>－</span><span lang=\"EN-US\" style='color:black'>7</  span></sup></span></span><span  style='mso-bookmark:SZM00B000'><span   style='mso-bookmark:SZM00BC00'><span style='font-family:\"新細明體  \",\"serif\"; mso-ascii-font-family:Calibri;  mso-ascii-theme-font:minor-latin;mso-fareast-font-family: 新細明體; mso-fareast-theme-font:minor-fareast;  mso-hansi-font-family:Calibri; mso-hansi-theme-font:minor-latin;  color:black'>最大</span></span></span></p> </div> "
          },
          "updateTime": "2021-09-10T15:32:38.79Z"
        }
      ]
    }
  }
  ```

  > 欄位說明
  >| Name        | Meaning | Remark |
  > ------------ | ------- | ------ |
  > uid          | 自主練習UID
  > | <font color=#66B3FF> practiceInfo </font> | <font color=#66B3FF> 自主練習資訊 </font>
  > uid          | 自主練習UID
  > name         | 名稱
  > eduSubjectName | 學制科目
  > questionAmount | 試題總數
  > year        | 學年度代碼
  > yearName    | 學年度
  > education   | 學制代碼
  > educationName | 學制
  > subject     | 科目代碼
  > subjectName | 科目
  > maintainer  | 出卷者
  > createTime  | 建立時間
  > lastUpdateTime | 最後維護時間
  > | <font color=#66B3FF> questionGroup </font> | <font color=#66B3FF> 題型群組 </font>
  > typeCode   | 題型代碼
  > typeName   | 題型名稱
  > scoreType  | 配分方式 | 固定以"每題配分, 滿分100試算"
  > score      | 配分分數 | 同上
  > | <font color=#66B3FF> .questionList </font> | <font color=#66B3FF> 試題清單 </font> |
  > sequence     | 題號
  > id           | 試題ID
  > answerAmount | 答案數
  > score        | 該題配分 | 不區分每題或每答, 為該題總分 (試算)
  > | <font color=#66B3FF> questionInfo </font> | <font color=#66B3FF> 試題資訊</font> | 完全相同於 **試卷明細API：<font color=#66B3FF>[GET] /api/Service/ExamPaper/{UID}</font>**
---

### **<font color=#1AFD9C>[POST] /api/Service/ExamResult </font>**
> 回寫自主練習完成紀錄

* ### Header
  *none*

* ### Payload
  >| Name   | Meaning | Remark |
  > ------- | ------- | ------ |
  > otp     | 一次性授權碼 | 建立測驗時產生，使用後失效

* ### Response
  > OneExam指定回傳格式

  > 回寫成功
  ```json
  {
    "status": "success",
    "content": null
  }
  ```
  > 回寫失敗
  ```json
  {
    "status": "error",
    "content": "OTP資料：不存在, 請確認輸入內容。"
  }
  ```
---