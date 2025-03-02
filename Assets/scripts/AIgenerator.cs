using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;

public class AIgenerator : MonoBehaviour
{
    private string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";
    private string apiKey = "AIzaSyDf-Dx3jcG0uKS4xDHuESkioZte2gTsNDo";
    public TMP_Text storyText;
    private List<string> eventHistory = new List<string>();

    /// <summary>
    /// 生成AI事件
    /// </summary>
    public void GenerateEvent(int age, int health, int wealth, int intelligence, int social,int creativity,
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
        string prompt = $"{playerInfo}\n{eventSummary}\n" +
            "你是一个人生模拟器 AI，需要结合角色的属性和过往经历，生成一个合理的、影响深远的人生重大事件，并用生动简洁的故事风格呈现。" +
            "人生充满了意外，虽然概率小，但是在老年之前会出现死亡"+
            "可以是好人好事，也可以出现暗黑向的故事（犯罪，暴力，色情，恐怖），一切都可能发生，主角可以是好人，也可以不是一个好人，但是必须写实，生活现实可能发生的事情"+
            "你生成的故事如果对角色属性 健康（0-100，0即刻死亡）、财富、智力、社交、性格、教育、创造力、职业、快乐值产生影响，请合理调整数值，至少影响一项数值"+
            "性格、教育、职业都是文本"+
            "请参考下面的模板返回JSON格式数据：\n" +
            "{\n" +
                    "    \"attribute_changes\": {\n" +
                    "        \"health\": -10,\n" +
                    "        \"wealth\": 5000,\n" +
                    "        \"intelligence\": 0,\n" +
                    "        \"social\": 5,\n" +
                    "        \"happiness\": 20,\n" +
                    "        \"education\": \"小学\",\n" +
                    "        \"career\": \"教师\",\n" +
                    "        \"personality\": \"开朗\"\n" +
                    "    },\n" +
                    "    \"short_description\": \"成功成为小学教师\",\n" +
                    "    \"detailed_description\": \"经过多年的努力，你终于考取了教师资格证，成为了一名小学教师，教育水平提升，财富也有所增加。\"\n" +
                    "}";
        StartCoroutine(SendRequest(prompt));

    }
    /// <summary>
    /// 发送api请求到gemini
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
    IEnumerator SendRequest(string prompt) { 
        string jsonData = "{\"contents\": [{\"parts\": [{\"text\": \"" + prompt + "\"}]}]}";

        //将 jsonData 转换为 byte[]
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        //创建 HTTP 请求
        UnityWebRequest request = new UnityWebRequest(apiUrl + "?key=" + apiKey, "POST");

        //告诉服务器，我们要 上传 bodyRaw（即 jsonData）
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        //接收服务器返回的 AI 生成文本
        request.downloadHandler = new DownloadHandlerBuffer();

        //设置 请求头
        request.SetRequestHeader("Content-Type", "application/json");

        // 发送请求，并等待 AI 响应
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            Debug.Log(response);

            ProcessAIreponse(response);
        }
        else {
            Debug.LogError("❌ AI failed: " + request.responseCode + " - " + request.error);
            Debug.LogError("❌ Response: " + request.downloadHandler.text);
        }
    }

    ///<summary>
    ///解析 AI 返回的数据并修改角色属性
    ///</summary>
    ///

    //创建 JSON 数据类 解析 AI 返回的 JSON 根对象
    [System.Serializable]
    public class AIresponse {

        public AttributeChanges attribute_changes;
        public string short_description;
        public string detailed_description;
    
    }

    //创建 JSON 数据类 解析 角色属性变动
    [System.Serializable] public class AttributeChanges {
        public int health;
        public int wealth;
        public int intelligence;
        public int social;
        public int happiness;
        public string education;
        public string career;
        public string personality;

    }


    void ProcessAIreponse(string jsonResponse)
    {
        try {
            //解析 JSON 并映射到 AIResponse 类
            AIresponse aiData = JsonUtility.FromJson<AIresponse>(jsonResponse);

            // 解析数据
            int healthChange = aiData.attribute_changes.health;
            int wealthChange = aiData.attribute_changes.wealth;
            int intelligenceChange = aiData.attribute_changes.intelligence;
            int socialChange = aiData.attribute_changes.social;
            int happinessChange = aiData.attribute_changes.happiness;
            string educationChange = aiData.attribute_changes.education;
            string careerChange = aiData.attribute_changes.career;
            string personalityChange = aiData.attribute_changes.personality;

            // 获取事件描述
            string shortDescription = aiData.short_description;
            string detailedDescription = aiData.detailed_description;

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
            if (!string.IsNullOrEmpty(educationChange)) Object.FindFirstObjectByType<GameManager>().ModifyAttribute("education", educationChange);
            if (!string.IsNullOrEmpty(careerChange)) Object.FindFirstObjectByType<GameManager>().ModifyAttribute("career", careerChange);
            if (!string.IsNullOrEmpty(personalityChange)) Object.FindFirstObjectByType<GameManager>().ModifyAttribute("personality", personalityChange);

        }
        catch (System.Exception e) {
            Debug.LogError("解析 AI JSON 失败: " + e.Message);
        }
    }
}
