using System.Collections;
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
    private string apiEndpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent"; // Edit it and choose your prefer model

    //private Content[] chatHistory;
    public TMP_Text storyText;
    private List<string> eventHistory = new List<string>();

    void Start()
    {
        UnityAndGeminiKey jsonApiKey = JsonUtility.FromJson<UnityAndGeminiKey>(jsonApi.text);
        apiKey = jsonApiKey.key;
        //chatHistory = new Content[] { };
    }

    private IEnumerator SendPromptRequestToGemini(string promptText)
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
                ProcessAIresponse(www.downloadHandler.text);
            }
        }
    }


    public void GenerateEvent(int age, int health, int wealth, int intelligence, int social, int creativity,
                      string personality, string education, string career,
                      int happiness, List<string> previousEvents)
    {
        string playerInfo = $"玩家当前状态：\n" +
                           $"- 年龄：{age} 岁 \n" +
                           $"- 健康：{health} \n" +
                           $"- 财富：{wealth} \n" +
                           $"- 智力：{intelligence} \n" +
                           $"- 社交：{social} \n" +
                           $"- 性格：{personality} \n" +
                           $"- 教育：{education} \n" +
                           $"- 创造力：{creativity} \n" +
                           $"- 职业：{career} \n" +
                           $"- 快乐值：{happiness} \n";
        //历史事件只取最近5个
        string eventSummary = "过往人生经历：\n";
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


        //prompt

        string jsonExample = "{\\\"attribute_changes\\\": {\\\"health\\\": -10, \\\"wealth\\\": 5000, \\\"intelligence\\\": 0, \\\"social\\\": 5, \\\"happiness\\\": 20, \\\"education\\\": \\\"小学\\\", \\\"career\\\": \\\"教师\\\", \\\"personality\\\": \\\"开朗\\\"}, \\\"short_description\\\": \\\"成功成为小学教师\\\", \\\"detailed_description\\\": \\\"经过多年的努力，你终于考取了教师资格证，成为了一名小学教师，教育水平提升，财富也有所增加。\\\"}";

        string prompt = $"{playerInfo}\n{eventSummary}\n" +
            "你是一个人生模拟器 AI，需要结合角色的属性和过往经历，生成一个合理的、影响深远的人生重大事件，并用生动简洁的故事风格呈现。\n" +
            "人生充满了意外，虽然概率小，但是在老年之前会出现死亡。如果出现死亡事件，请返回健康-10000\n" +
            "请生成一个符合逻辑的故事，并按照以下 JSON 格式返回：\n\n" +
            "可以是好人好事，也可以出现暗黑向的故事（犯罪，暴力，色情，恐怖），一切都可能发生，主角可以是好人，也可以不是一个好人，但是必须写实，生活现实可能发生的事情，注意时代背景是现代"+
            "你生成的故事如果对角色属性 健康（一般出生是100，但也可以0-100，小于等于0即刻死亡）、财富、智力、社交、性格、教育、创造力、职业、快乐值产生影响，请合理调整数值，至少影响一项数值"+
            "23岁之前一般都是在上学，6-12小学，12-15初中，15-18高中（职业高中0，18以后就是大学（大专）不一定被限制死，随便发挥）"+
            "智商会有个初始值，随机给一个"+
            jsonExample;
        StartCoroutine(SendPromptRequestToGemini(prompt));

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
        public int health;
        public int wealth;
        public int intelligence;
        public int social;
        public int happiness;
        public string education;
        public string career;
        public string personality;

    }


    void ProcessAIresponse(string jsonResponse)
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

                Debug.Log("aiData:" + aiData);
                // 解析数据
                int healthChange = aiData.attribute_changes.health;
                int wealthChange = aiData.attribute_changes.wealth;
                int intelligenceChange = aiData.attribute_changes.intelligence;
                int socialChange = aiData.attribute_changes.social;
                int happinessChange = aiData.attribute_changes.happiness;
                string educationChange = aiData.attribute_changes.education;
                string careerChange = aiData.attribute_changes.career;
                string personalityChange = aiData.attribute_changes.personality;
                string creativityChange = aiData.attribute_changes.personality;

                // 获取事件描述
                string shortDescription = aiData.short_description;
                string detailedDescription = aiData.detailed_description;

                Debug.Log("事件详情: " + detailedDescription);

                // 更新 UI
                storyText.text = detailedDescription;
                int currentPosition = Object.FindFirstObjectByType<GameManager>().currentPosition;
                Object.FindFirstObjectByType<GameManager>().LogEvent(currentPosition, shortDescription, detailedDescription);

                // 更新角色属性
                Object.FindFirstObjectByType<GameManager>().ModifyAttribute("health", healthChange);
                Object.FindFirstObjectByType<GameManager>().ModifyAttribute("wealth", wealthChange);
                Object.FindFirstObjectByType<GameManager>().ModifyAttribute("intelligence", intelligenceChange);
                Object.FindFirstObjectByType<GameManager>().ModifyAttribute("social", socialChange);
                Object.FindFirstObjectByType<GameManager>().ModifyAttribute("happiness", happinessChange);
                Object.FindFirstObjectByType<GameManager>().ModifyAttribute("creativity", creativityChange);

                if (!string.IsNullOrEmpty(educationChange)) Object.FindFirstObjectByType<GameManager>().ModifyAttribute("education", educationChange);
                if (!string.IsNullOrEmpty(careerChange)) Object.FindFirstObjectByType<GameManager>().ModifyAttribute("career", careerChange);
                if (!string.IsNullOrEmpty(personalityChange)) Object.FindFirstObjectByType<GameManager>().ModifyAttribute("personality", personalityChange);
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


