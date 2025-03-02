using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMP_Text diceResultText; //显示投掷的数字
    public Button rollDiceButton; //投掷数字的按钮

    public TMP_Text age; //显示年龄
    private int ageNumber;

    public Transform playerPiece; //玩家棋子的位置
    private Vector3[] boardPositions; //存储所有棋盘格子的位置
    public int currentPosition = 0; //角色当前所在的格子编号
    // public PlayerStats playerStats;  //另一个脚本管理角色的 年龄、财富、智力等属性

    public GameObject eventIconPrefab; //事件预制体
    public Transform evenIconParent; //事件 Icon 父级
    public TMP_Text currentStoryText;  //显示当前事件的文本
    public Transform currentStoryCanvas; // 当前事件窗口
    public TMP_Text storyDetailText;  //显示所有事件的文本
    public Transform storyDetailCanvas; // 所有历史事件窗口
    public Transform eventlogScrolView; // 简要事件日志窗口
    public TMP_Text eventlogText; // 简要事件日志文本

    //字典里面的key是position,内容是具体故事
    private Dictionary<int, string> eventHistory = new Dictionary<int, string>(); //存储事件详情
    private List<string> allEventList = new List<string>(); //存储所有历史事件


    //gameover
    public GameObject gameOverCanvas; // 死亡面板
    public TMP_Text gameOverText; // 显示死亡文本
    public TMP_Text lifeReviewText; // 显示完整人生回顾的文本框
    public ScrollRect lifeReviewScrollView; // 滚动视图


    //角色属性
    public int health;
    public int wealth;
    public int intelligence;
    public int social;
    public int happiness;
    public int creativity;
    public string education;
    public string career;
    public string personality;


    //UI组件
    public TMP_Text healthText;
    public TMP_Text wealthText;
    public TMP_Text intelligenceText;
    public TMP_Text socialText;
    public TMP_Text happinessText;
    public TMP_Text educationText;
    public TMP_Text careerText;
    public TMP_Text personalityText;
    public TMP_Text creativityText;

    //ai 脚本
    public AImanager aiManager;

    void Start()
    {
        GenerateBoardPositions(); // 生成蛇形棋盘坐标
        playerPiece.position = boardPositions[currentPosition];  // 让角色从起点开始
        rollDiceButton.onClick.AddListener(RollDice);  // 绑定投掷按钮
        diceResultText.enabled = true;
    }

    public void RollDice() {

        
        //生成1-10随机数
        int diceNumber = Random.Range(1, 11);

        diceResultText.text = diceNumber.ToString(); //将数字转化为为文本，然后展示投掷数字，因为text组件只接受string

        //更新角色位置,此时的角色位置已经更新为投掷完色子之后的格子
        int targetPosition = currentPosition + diceNumber;

        //增加年龄
        ageNumber = targetPosition;
        age.text = $"年龄：{ageNumber}";



        //启动IEnumerator 函数（协程）移动角色
        StartCoroutine(MovePlayerSoomthly(targetPosition));

        //生成AI事件

   

    }
    void GenerateBoardPositions() {
        int rows = 15;
        int cols = 13;
        float gridSize = 0.59f;
        Vector3 startPosition = new Vector3(-8f, -3.9f, 0f);
        boardPositions = new Vector3[rows * cols];

        int index = 0;
        for (int col = 0; col < cols; col++) {
            if (col % 4 == 0) //0 4 8 12是从下往上增加
            {
                for (int row = 0; row < rows; row++)
                {
                    boardPositions[index] = new Vector3(
                                        startPosition.x + col * gridSize, // x 轴变化
                                        startPosition.y + row * gridSize, // y 轴变化
                                        0);
                    index++;
                }
            }
            else if (col % 4 == 1) //奇数列 1 5 9 在上面只有1个格子
            {
                boardPositions[index] = new Vector3(
                                       startPosition.x + col * gridSize, // x 轴变化
                                       startPosition.y + 14 * gridSize, // y 轴变化
                                       0);
                index++;
            }
            else if (col % 4 == 2) //偶数列 2 6 10是从上往下增加
            {
                for (int row = 14; row >= 0; row--)
                {
                    boardPositions[index] = new Vector3(
                    startPosition.x + col * gridSize, // x 轴变化
                    startPosition.y + row * gridSize, // y 轴变化
                    0);
                    index++;
                }
            }
            else if (col % 4 == 3) // 奇数列 3 7 11在下面
            {
                boardPositions[index] = new Vector3(
                 startPosition.x + col * gridSize, // x 轴变化
                 startPosition.y, // y 轴在最底部
                 0);
                index++;
            }
        }
    }




    /// <summary>
    /// 为了不让角色瞬移，而是平滑的移动
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    IEnumerator MovePlayerSoomthly(int targetPosition) {
        rollDiceButton.interactable = false; //禁用投掷按钮，防止多次点击

        //如果角色距离 targetPosition 还比较远，就不断地往目标方向移动。
        while (currentPosition < targetPosition) {
            currentPosition++;
            Vector3 nextPostion = boardPositions[currentPosition]; //找到下一步的位置

            while (Vector3.Distance(playerPiece.position,nextPostion) > 0.1f) {
                playerPiece.position = Vector3.Lerp(playerPiece.position, nextPostion, Time.deltaTime * 5);

                //让 Unity 等待 下一帧 再继续执行循环，避免卡顿。
                yield return null;

            }
            playerPiece.position = nextPostion;
        }
        
        rollDiceButton.interactable= true;

        //生成事件icon小脚丫
        creatEventIcon(currentPosition);

        //生成AI事件
        aiManager.GenerateEvent(
            ageNumber, health, wealth, intelligence, social, creativity,
            personality, education, career, happiness, allEventList
        );

    }

    void creatEventIcon(int position) {
        GameObject newIcon = Instantiate(eventIconPrefab,evenIconParent);
        newIcon.transform.position = boardPositions[position];

        newIcon.GetComponent<Button>().onClick.AddListener(()=> ShowEventDetail(position));
    }

    /// <summary>
    /// 点击icon之后显示当前格子的事件
    /// </summary>
    /// <param name="position"></param>
    void ShowEventDetail(int position) {
        if (eventHistory.ContainsKey(position)) { 
            currentStoryText.text = eventHistory[position]; // 显示完整事件
            Object.FindFirstObjectByType<UiManager>().currenEvent();
        }
    
    }
    /// <summary>
    /// AI生成的事件存储到history中去
    /// </summary>
    /// <param name="position"></param>
    /// <param name="shortEvent"></param>
    /// <param name="fullEvent"></param>
    public void LogEvent(int position, string shortEvent, string fullEvent)
    { 
        eventHistory[position] = fullEvent;
        allEventList.Add($"{shortEvent}\n{fullEvent}");
        eventlogText.text += $"{shortEvent}\n";
    }

    public void ShowAllEvents()
    {
        storyDetailText.text = string.Join("\n\n", allEventList); // 拼接所有事件
        storyDetailCanvas.gameObject.SetActive(true); // 打开大窗口
    }

    ///<summary>
    ///修改数值和字符串
    /// </summary>
    /// 

    public void ModifyAttribute(string attribute, int value) {
        //switch是一种多分支
        switch (attribute) {
            case "health":
                health += value;
                healthText.text = $"健康：{health}";
                if (health <= 0)
                {
                    GameOver(); // ⬅ 触发游戏结束
                }
                break;
            case "wealth":
                wealth += value;
                wealthText.text = $"财富：{wealth}";
                break;
            case "intelligence":
                intelligence += value;
                intelligenceText.text = $"智力：{intelligence}";
                break;
            case "social":
                social += value;
                socialText.text = $"社交：{social}";
                break;
            case "happiness":
                happiness += value;
                happinessText.text = $"快乐值：{happiness}";
                break;
            case "creativity":
                creativityText.text = $"创造力：{creativity}";
                break;
        }
    }

    public void ModifyAttribute(string attribute, string newValue)
    {
        switch (attribute)
        {
            case "education":
                education = newValue;
                educationText.text = $"教育：{education}";
                break;
            case "career":
                career = newValue;
                careerText.text = $"职业：{career}";
                break;
            case "personality":
                personality = newValue;
                personalityText.text = $"性格：{personality}";
                break;
        }
    }

    public void GameOver() {
        Debug.Log("角色死亡，游戏结束");
        gameOverText.text = "你因健康值耗尽而死亡，人生旅程到此结束。\n";

        string lifeReview = "你的一生回顾：\n\n";
        foreach (string eventLog in allEventList)
        {
            lifeReview += "🔹 " + eventLog + "\n\n"; // 每个事件加上符号
        }
        // 显示在 UI 上
        lifeReviewText.text = lifeReview;

        gameOverCanvas.SetActive(true); // 显示死亡 UI
        rollDiceButton.interactable = false; // 禁用投掷按钮
    }

    public void RestartGame()
    {
        Debug.Log("重新开始游戏");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
