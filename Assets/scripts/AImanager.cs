﻿using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class UnityAndGeminiKey
{
    public string key;
}

[System.Serializable]
public class Response
{
    public Candidate[] candidates;
}

public class ChatRequest
{
    public Content[] contents;
}

[System.Serializable]
public class Candidate
{
    public Content content;
}

[System.Serializable]
public class Content
{
    public string role;
    public Part[] parts;
}

[System.Serializable]
public class Part
{
    public string text;
}

public class AImanager : MonoBehaviour
{
    [Header("JSON API Configuration")]
    public TextAsset jsonApi;
    private string apiKey = "";
    private string apiEndpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent"; // Edit it and choose your prefer model

    //private Content[] chatHistory;
    public TMP_Text storyText;
    private List<string> eventHistory = new List<string>();

    public UiManager uiManager;
    public GameManager gameManager;


    void Start()
    {
        UnityAndGeminiKey jsonApiKey = JsonUtility.FromJson<UnityAndGeminiKey>(jsonApi.text);
        apiKey = jsonApiKey.key;

    }

    private IEnumerator SendPromptRequestToGemini(string promptText, int health, int wealth, int intelligence, int social, int happiness)
    {
        string url = $"{apiEndpoint}?key={apiKey}";

        string jsonData = "{ \"contents\": [{ \"parts\": [{ \"text\": \"" + promptText + "\" }] }] }";

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        // Create a UnityWebRequest with the JSON data
        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Request complete!");
                ProcessAIresponse(www.downloadHandler.text, health, wealth, intelligence, social, happiness);
            }
        }
    }


    public void GenerateEvent(int age, int health, int wealth, int intelligence, int social,
                      string personality, string education, string career,
                      int happiness, List<string> previousEvents)
    {
        string gender = gameManager.playerGender == GameManager.Gender.Male ? "男性" : "女性";

        // 随机生成变化值
        int healthChange = (age > 80) ? UnityEngine.Random.Range(-60, -40) : UnityEngine.Random.Range(-20,20);

        int wealthChange =(age <20) ?
            UnityEngine.Random.Range(-1000, 1000):
             UnityEngine.Random.Range(-30000, 30000);

        int intelligenceChange = UnityEngine.Random.Range(0, 5);     
        int socialChange = UnityEngine.Random.Range(-20, 30);        
        int happinessChange = UnityEngine.Random.Range(-20, 30);

        health = health + healthChange;
        wealth = wealth + wealthChange;
        intelligence = intelligence + intelligenceChange;
        social = social + socialChange;
        happiness =  happiness + happinessChange;


        int newHealth = health + healthChange;
        int newWealth = wealth + wealthChange;
        int newIntelligence = intelligence + intelligenceChange;
        int newSocial = social + socialChange;
        int newHappiness = happiness + happinessChange;

        string playerInfoCN =
                           $"玩家当前状态：\n" +
                           $"- 性别：{gender}  \n"+
                           $"- 年龄：{age} 岁 \n" +
                           $"- 健康：{newHealth} \n" +
                           $"- 财富：{newWealth} \n" +
                           $"- 智力：{newIntelligence} \n" +
                           $"- 社交：{newSocial} \n" +
                           $"- 性格：{personality} \n" +
                           $"- 教育：{education} \n" +
                           $"- 职业：{career} \n" +
                           $"- 快乐值：{newHappiness} \n";
        Debug.Log("当前玩家状态："+ playerInfoCN);

        string playerInfoEN =
                 "## Current Player Status:\n" +
                 $"- Gender: {(gender == "男性" ? "Male" : "Female")}\n" +
                 $"- Age: {age} years old\n" +
                 $"- Health: {newHealth}\n" +
                 $"- Wealth: {newWealth}\n" +
                 $"- Intelligence: {newIntelligence}\n" +
                 $"- Social: {newSocial}\n" +
                 $"- Personality: {personality}\n" +
                 $"- Education: {education}\n" +
                 $"- Career: {career}\n" +
                 $"- Happiness: {newHappiness}\n\n";

        //历史事件只取最近5个
        string eventSummary = " ";
        if (previousEvents.Count > 0)
        {
            int startIndex = Mathf.Max(0, previousEvents.Count - 5); // 只取最近5个
            for (int i = startIndex; i < previousEvents.Count; i++)
            {
                eventSummary += $"- {previousEvents[i]} \n";  // ✅ 正确的字符串拼接
            }
        }
        else
        {
            eventSummary += "玩家没有过往事件，这是人生的起点。\n";
        }

        string prompt;

        if (uiManager.selectLanguage == UiManager.Language.Chinese)
        {

            if (newHealth <= 0){
                string jsonExample = "{\\\"attribute_changes\\\": {\\\"education\\\": \\\"Primary School\\\", \\\"career\\\": \\\"Teacher\\\", \\\"personality\\\": }, \\\"short_description\\\": \\\"突发心脏病\\\", \\\"detailed_description\\\": \\\"在一个平静的早晨，你感到胸口剧烈疼痛，随即失去了意识。尽管被迅速送往医院，但最终不治身亡。\\\"}";

                prompt = 
                 "你是一个生活模拟人工智能，可以生成逼真的生活事件。"+
                 "这是玩家过往事件"+
                 $"{eventSummary}\n" +
                 "角色现在健康值低于0，玩家已经死亡，这是玩家最后的状态：" +
                 $"{playerInfoCN}\n"+
                 "请返回死亡事件。\n" +
                "   - 死亡事件必须合理，例如：疾病、事故、年老、意外等。\n" +
                "   - 必须描述 **死亡原因**（如 “你因突发心脏病去世”）。\n"+
                "事件必须富有故事性和情感，而不仅仅是简单的描述。\n" +
                "**严格按照以下 JSON 格式返回数据**，只能返回单个JSON，不能返回多个JSON数组！！ 不能包含任何额外的文本、换行或 Markdown 代码块。\n\n" +
                jsonExample
                ;
            }
            else {
                string jsonExample = "{\\\"attribute_changes\\\": {\\\"education\\\": \\\"小学\\\", \\\"career\\\": \\\"教师\\\", \\\"personality\\\": \\\"开朗\\\"}, \\\"short_description\\\": \\\"成功成为小学教师\\\", \\\"detailed_description\\\": \\\"经过多年的努力，你终于考取了教师资格证，成为了一名小学教师，教育水平提升，财富也有所增加。\\\"}";

                prompt = $"{playerInfoCN}\n{eventSummary}\n" +
                "你是一个人生模拟器 AI，模拟真实人生轨迹，事件需要有足够的细节、情感和戏剧性, 你需要基于角色当前状态，生成符合年龄、性别设定的当前人生重大事件，生动形象" +
                "事件必须富有故事性和情感，而不仅仅是简单的描述。\n" +
                "事件必须符合 " + $"{age}" + "岁的玩家，职业不能与年龄冲突！（例如 21 岁不能是“小学生”）**\n" +
                "请确保事件有完整的情节，包括背景、发展和结局，不要给出小标题，详细描述玩家的感受和抉择。\n" +
                "不要只谈事业，家庭（父母，子女，伴侣），友情也是会影响人的，可以言情，可以励志，但是不要脱离现实" +
                "不同性别可能会遇到不同的职业发展、家庭角色、社会影响。" +
                 "## **⚠ 重要规则（必须遵守）**\n" +

               

                "1 **人生不总是悲惨的**：\n" +
                "   - 生成的事件不能总是负面，也需要**幸福、美好、欢乐、成就感的事件**。\n" +
                "   - 允许出现 **人生高光时刻**，例如：\n" +
                "     - **事业成功**（但可能带来家庭失衡）\n" +
                "     - **找到挚爱**（但可能遇到挑战）\n" +
                "     - **获得巨大财富**（但可能失去朋友）\n" +
                "   - 人生是 **悲喜交加**，不能只生成悲剧。\n\n" +

                "2 **事件必须涵盖事业、友情、爱情、家庭，而不仅仅是事业**：\n" +
                "   - 友情（朋友、兄弟、姐妹、同事、撕逼、背叛、欺骗）\n" +
                "   - 爱情（恋爱、婚姻、分手、结婚、生子、出轨、LGBT、开放性关系、也可以单身主义）\n" +
                "   - 亲情（父母、兄弟姐妹、孩子、丁克）\n" +
                "   - 事业（工作、创业、升职、转行、犯罪、特殊工种）\n" +
                "   - **不同阶段人生侧重点不同**：\n" +

                "3 健康值低时，可能通过财富挽救（如高端医疗）\n\n" +

                "4 **职业变化可以丰富多样**：\n" +
                "   - 人生不一定一成不变，角色可以**转行、创业、改变职业**。\n" +
                "   - 允许出现 **特殊职业**（杀手、军人、医生、科学家、黑帮、特工）。\n" +
                "   - 职业不能**只关于网络（网暴、网安、直播等）**。\n\n" +

                 "5 **不同性别可能遇到不同的人生轨迹**：\n" +
                "   - **男性角色** 可能会面临更多的职业发展、社会竞争。\n" +
                "   - **女性角色** 可能会在家庭、社会角色上有不同的影响。\n" +
                "   - AI 生成的事件需要符合角色的 **性别、年龄、文化背景、社会现实**。\n\n" +

                " **年龄对教育和职业的约束（严格遵守）如果中途退学，请说明原因：**\n" +
                "   - **0 - 5 岁：** 无职业，可能在幼儿园或家庭照顾\n" +
                "   - **6 - 12 岁：** 处于小学阶段，可以中断学业\n" +
                "   - **13 - 18 岁：** 中学或高中阶段，可以是实习生或兼职，可以中断学业\n" +
                "   - **19 - 22 岁：** 处于大学阶段，可能是大学生或实习生，可以中断学业\n" +
                "   - **23+ 岁：** 大概率有正式职业，可以无业，不能是小学生或幼儿园\n" +
                "   - **50+ 岁：** 可能进入退休阶段\n" +

                "---\n\n" +
                "你只需要返回：教育、职业、性格、事件故事**，其他数值由游戏计算。\n"+

                "**严格按照以下 JSON 格式返回数据**，只能返回单个JSON，不能返回多个JSON数组！！ 不能包含任何额外的文本、换行或 Markdown 代码块。\n\n" +

                jsonExample;
            }

        }
        else {

            if (newHealth <= 0)
            {
                string jsonExample = "{\\\"attribute_changes\\\": {\\\"education\\\": \\\"Primary School\\\", \\\"career\\\": \\\"Teacher\\\", \\\"personality\\\": }, \\\"short_description\\\": \\\"突发心脏病\\\", \\\"detailed_description\\\": \\\"在一个平静的早晨，你感到胸口剧烈疼痛，随即失去了意识。尽管被迅速送往医院，但最终不治身亡。\\\"}";
                prompt =
                 "You are a life simulation AI that generates realistic life events." +
                 "This is the player past event" +
                 $"{eventSummary}\n" +
                 "The character is now below 0 health, the player is dead, and this is the player's last state:" +
                 $"{playerInfoEN}\n" +
                 "Please return the death event. \n" +
                 "The events must be **story-driven and emotionally impactful**.\n\n" +
                "   -The death event must be reasonable, e.g. illness, accident, old age, accident, etc. \n" +
                "   - Must describe **the cause of death** (e.g. ‘You died of a sudden heart attack’). \n"+
                "STRICTLY return data in the following **JSON format**. Only return a **single JSON object**—DO NOT return multiple JSON objects or arrays.\n" +
                jsonExample;


            }
            else {
            string jsonExample = "{\\\"attribute_changes\\\": {\\\"education\\\": \\\"Primary School\\\", \\\"career\\\": \\\"Teacher\\\", \\\"personality\\\": }, \\\"short_description\\\": \\\"Successfully became a primary school teacher\\\", \\\"detailed_description\\\": \\\"After years of effort, you finally obtained a teaching qualification and became a primary school teacher. Your education level improved, and your wealth increased.\\\"}";

            prompt = $"{playerInfoEN}\n{eventSummary}\n" +
            "The event must be appropriate for a "+ $"{age}"+ "-year-old character,Occupation cannot conflict with age!" +
            "You are a life simulation AI that generates realistic life events. The events must have rich details, emotions, and drama.\n" +
            "You must generate major life events based on the player's current status, ensuring that the events are vivid, engaging, and aligned with the player's age and gender.\n" +
            "The events must be **story-driven and emotionally impactful**.\n\n" +
            "Ensure that each event includes a background, development, and conclusion, and describes the player's thoughts, emotions, and choices in detail.\n" +
            "Do not focus only on career; **family (parents, children, spouse) and friendships also influence a person's life**. \n" +
            "The events can be romantic or inspirational, but they must remain **realistic and culturally appropriate**.\n" +
            "Different genders may experience different career developments, family roles, and social expectations.\n\n" +
            

            "## **⚠ IMPORTANT RULES (STRICTLY FOLLOW)**\n\n" +

            

            "1 **Life is not always tragic:**\n" +
            "   - Events should not always be negative. There should also be **happy, joyful, and fulfilling moments**.\n" +
            "   - Include **major achievements** in life, such as:\n" +
            "     - **Career success** (which might come with family sacrifices)\n" +
            "     - **Finding true love** (but facing relationship challenges)\n" +
            "     - **Gaining immense wealth** (but potentially losing friends)\n" +
            "   - Life is **a mix of joy and sorrow**; do not generate only tragic events.\n\n" +

            "2 **Events must cover career, friendship, love, and family, not just career:**\n" +
            "   - **Friendship** (close friends, siblings, colleagues,tearing up, betrayal, cheating)\n" +
            "   - **Romance** (dating, marriage, breakups, having children, cheating, LGBT, open relationships, also singlism)\n" +
            "   - **Family** (relationships with parents, siblings, children,dinks)\n" +
            "   - **Career** (working, starting a business, promotions, career changes, specialised jobs)\n" +
            "   - **Different life stages should focus on different aspects.**\n\n" +

            "3 **Death probability must follow realistic patterns，If you drop out midway through the programme, please state the reason::**\n" +
            "   - Most people will not die before age 23.\n" +
            "   - Death probability increases after age 60.\n" +
            "   - If health is low, wealth may help delay death (e.g., access to advanced healthcare).\n\n" +

            "4**Career changes should be diverse:**\n" +
            "   - Life is not static; characters may **change careers, start businesses, or switch professions**.\n" +
            "   - Special professions are allowed, such as **assassin, soldier, doctor, scientist, gangster, or secret agent**.\n" +
            "   - However, careers **should not focus solely on internet-related jobs** (e.g., online influencers, cybersecurity, internet scandals).\n\n" +

            "5 **Different genders may experience different life paths:**\n" +
            "   - **Male characters** may face more career challenges and social competition.\n" +
            "   - **Female characters** may have different social roles, family expectations, and life experiences.\n" +
            "   - AI-generated events must align with **the character’s gender, cultural background, and social reality**.\n\n" +


            "Age constraints on education and occupation (strictly observed):" +
            "   - **0 - 5 years old:：** No occupation, may be in kindergarten or home care\n" +
            "   - **6 - 12 years old:：** in primary school, may interrupt studies\n" +
            "   - **13 - 18 years old:：** At secondary or high school level, may be an intern or part-time, may interrupt studies\n" +
            "   - **19 - 22 岁years old:  at university level, may be a college student or intern, can interrupt studies\n" +
            "   - **23+ years old:：** Probable to have a formal career, can be unemployed, cannot be a primary school student or kindergarten\n" +
            "   - **50+ years old:：** Likely to enter retirement\n" +


            "---\n\n" +

            
            "DO NOT include any additional text, line breaks, or Markdown code blocks.\n\n" +
            "The values of `attribute_changes` **must change dynamically** based on the player's status. Especially age!!!!!! \n" +
            "You only need to return: education, occupation, personality, event story**, other values are calculated by the game.\n" +
            "STRICTLY return data in the following **JSON format**. Only return a **single JSON object**—DO NOT return multiple JSON objects or arrays.\n" +
            jsonExample;
            }
        }

        //prompt

        StartCoroutine(SendPromptRequestToGemini(prompt, newHealth, newWealth, newIntelligence, newSocial, newHappiness));

        Debug.Log("当前的prompt:"+ prompt);

    }
    //创建 JSON 数据类 解析 AI 返回的 JSON 根对象
    [System.Serializable]
    public class AIresponse
    {

        public AttributeChanges attribute_changes;
        public string short_description;
        public string detailed_description;

    }

    //创建 JSON 数据类 解析 角色属性变动
    [System.Serializable]
    public class AttributeChanges
    {
        //public int health;
        //public int wealth;
        //public int intelligence;
        //public int social;
        //public int happiness;
        public string education;
        public string career;
        public string personality;

    }


    void ProcessAIresponse(string jsonResponse, int health, int wealth, int intelligence, int social, int happiness)
    {
        try
        {
            Debug.Log("AI 返回的 JSON: " + jsonResponse);
            Response response = JsonUtility.FromJson<Response>(jsonResponse);

            if (response.candidates.Length > 0 && response.candidates[0].content.parts.Length > 0)
            {
                string jsonText = response.candidates[0].content.parts[0].text; // 这里是 JSON 字符串

                // 去掉 ```json 和 ```
                if (jsonText.StartsWith("```json"))
                {
                    jsonText = jsonText.Replace("```json", "").Replace("```", "").Trim();
                }

                //解析 JSON 并映射到 AIResponse 类
                AIresponse aiData = JsonUtility.FromJson<AIresponse>(jsonText);

                if (aiData.attribute_changes !=null) {
               
                // 解析数据
                //int healthChange = aiData.attribute_changes.health;
                //int wealthChange = aiData.attribute_changes.wealth;
                //int intelligenceChange = aiData.attribute_changes.intelligence;
                //int socialChange = aiData.attribute_changes.social;
                //int happinessChange = aiData.attribute_changes.happiness;
                string educationChange = aiData.attribute_changes.education;
                string careerChange = aiData.attribute_changes.career;
                string personalityChange = aiData.attribute_changes.personality;

                if (!string.IsNullOrEmpty(educationChange)) gameManager.ModifyAttribute("education", educationChange);
                if (!string.IsNullOrEmpty(careerChange)) gameManager.ModifyAttribute("career", careerChange);
                if (!string.IsNullOrEmpty(personalityChange)) gameManager.ModifyAttribute("personality", personalityChange);


                }
                // 获取事件描述
                string shortDescription = aiData.short_description;
                string detailedDescription = aiData.detailed_description;

                Debug.Log("事件详情: " + detailedDescription);

                // 更新 UI
                storyText.text = detailedDescription;
                int currentPosition = gameManager.currentPosition;
                gameManager.LogEvent(currentPosition, shortDescription, detailedDescription);

                // 更新角色属性
                gameManager.ModifyAttribute("health", health);
                gameManager.ModifyAttribute("wealth", wealth);
                gameManager.ModifyAttribute("intelligence", intelligence);
                gameManager.ModifyAttribute("social", social);
                gameManager.ModifyAttribute("happiness", happiness);

                
                Object.FindFirstObjectByType<UiManager>().currenEvent();
            }
            else
            {
                Debug.LogError("AI响应为空");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("解析 AI JSON 失败: " + e.Message);
        }
    }
}


