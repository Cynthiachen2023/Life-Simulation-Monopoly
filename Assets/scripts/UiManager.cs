using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject gameBoardCanvas;
    public GameObject storyDetailCanvas;
    public GameObject currentEventCanvas;
    public GameObject lifeReviewCanvas;
    public GameObject gameOverCanvas;
    public GameObject about;

    public ScrollRect currentEventScroll;
    public ScrollRect lifeReviewScroll;
    public ScrollRect storyDetailScroll;

    public RectTransform currentEventcontent;
    public RectTransform lifeReviewcontent;
    public RectTransform storyDetailcontent;

    public GameObject maleCharacter;
    public GameObject femaleCharacter;

    public enum Language { Chinese, English}
    public Language selectLanguage = Language.Chinese;
    public TMP_Text LanguageButtonText;

    [Header("中英文-Maincanvas")]
    public TMP_Text title, startbtn, language,aboutBtn;

    [Header("中英文-Gameboard")]
    public TMP_Text drawNumber, ageText, healthText, wealthText, intelligenceText, socialText, personalityText, educationText, careerText, happinessText;
    public TMP_Text eventLogBtn; //更多

    [Header("中英文-lifeReview")]
    public TMP_Text LFbackToGameBtn;

    [Header("中英文-storyDetail")]
    public TMP_Text SDbackToGameBtn;

    [Header("中英文-currentEvent")]
    public TMP_Text CEbackToGameBtn;

    [Header("中英文-Gameover")]
    public TMP_Text GOtitle, gameOverText, lifereviewBtn, restartBtn;

    [Header("中英文-About")]
    public TMP_Text backToMenu,content;


    public GameManager gameManager;

    void Start()
    {
        about.SetActive(true);
        about.SetActive(false);
        MainMenu();

    }

    public void MainMenu() {
        mainMenuCanvas.SetActive(true);
        gameBoardCanvas.SetActive(false);
        storyDetailCanvas.SetActive(false);
        currentEventCanvas.SetActive(false);
        lifeReviewCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        about.SetActive(false);
        realPlayerSetFalse();

    }

    // Update is called once per frame
    public void StartGame()
    {
        mainMenuCanvas.SetActive(false );
        gameBoardCanvas.SetActive(true);
        storyDetailCanvas.SetActive(false);
        realPlayerSetTrue();
        currentEventCanvas.SetActive(false);
        lifeReviewCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        about.SetActive(false);
    }

    public void storyDetail() { 
        mainMenuCanvas.SetActive(false );
        gameBoardCanvas.SetActive(false);   
        storyDetailCanvas.SetActive(true);
        realPlayerSetFalse();
        currentEventCanvas.SetActive(false);
        about.SetActive(false);
    }

    public void currenEvent() {
        mainMenuCanvas.SetActive(false);
        gameBoardCanvas.SetActive(true);
        storyDetailCanvas.SetActive(false);
        realPlayerSetTrue();
        currentEventCanvas.SetActive(true);
        about.SetActive(false);

    }

    public void backToGameboard() {
        mainMenuCanvas.SetActive(false);
        gameBoardCanvas.SetActive(true);
        storyDetailCanvas.SetActive(false);
        realPlayerSetTrue();
        currentEventCanvas.SetActive(false);
        about.SetActive(false);
    }

    public void backToGameOver() {
        lifeReviewCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
        gameBoardCanvas.SetActive(false);
        storyDetailCanvas.SetActive(false);
        currentEventCanvas.SetActive(false);
        about.SetActive(false);
    }

    public void lifeReview() {
        lifeReviewCanvas.SetActive(true);
        gameOverCanvas.SetActive(false);
        mainMenuCanvas.SetActive(false);
        gameBoardCanvas.SetActive(false);
        storyDetailCanvas.SetActive(false);
        currentEventCanvas.SetActive(false);
        about.SetActive(false);
        realPlayerSetFalse();
    }

    public void Gameover() {
        lifeReviewCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
        gameBoardCanvas.SetActive(false);
        storyDetailCanvas.SetActive(false);
        currentEventCanvas.SetActive(false);
        about.SetActive(false);
    }

    public void aboutPage() {
        lifeReviewCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        mainMenuCanvas.SetActive(false);
        gameBoardCanvas.SetActive(false);
        storyDetailCanvas.SetActive(false);
        currentEventCanvas.SetActive(false);
        about.SetActive(true);

    }

    void OnEnable() // 当窗口被激活时
    {
        ResetScrollPosition();
    }

    public void ResetScrollPosition()
    {
        StartCoroutine(ResetScrollNextFrame());


    }

    IEnumerator ResetScrollNextFrame()
    { 
        yield return new WaitForEndOfFrame();
        currentEventScroll.verticalNormalizedPosition = 1.0f;
        lifeReviewScroll.verticalNormalizedPosition = 1.0f;
        storyDetailScroll.verticalNormalizedPosition = 1.0f;
        currentEventcontent.anchoredPosition = new Vector2(currentEventcontent.anchoredPosition.x, 0);
        lifeReviewcontent.anchoredPosition = new Vector2(lifeReviewcontent.anchoredPosition.x, 0);
        storyDetailcontent.anchoredPosition = new Vector2(storyDetailcontent.anchoredPosition.x, 0);


    }
    public void ShowEventLog()
    {
        // 你的 UI 逻辑，显示 ScrollView
        currentEventScroll.gameObject.SetActive(true);
        lifeReviewScroll.gameObject.SetActive(true);
        storyDetailScroll.gameObject.SetActive(true);
        ResetScrollPosition(); // 确保打开时回到顶部
    }



    public Dictionary<string, string> chineseTexts = new Dictionary<string, string>()
    {
        {"title", "模拟人生大富翁"},
        {"startbtn", "开始游戏"},
        {"language", "语言：中文"},
        {"drawNumber", "投掷"},
        {"age", "年龄："},
        {"health", "健康："},
        {"wealth", "财富："},
        {"intelligence", "智力："},
        {"social", "社交："},
        {"personality", "性格："},
        {"education", "教育："},
        {"career", "职业："},
        {"happiness", "快乐："},
        {"eventLogBtn", "..更多"},
        {"LFbackToGameBtn", "返回游戏"},
        {"SDbackToGameBtn", "返回游戏"},
        {"CEbackToGameBtn", "返回"},
        {"GOtitle", "游戏结束"},
        {"gameOverText", "你因健康值耗尽而死亡，人生旅程到此结束。"},
        {"lifereviewBtn", "回顾人生"},
        {"restartBtn", "重投再来"},
        {"aboutbtn","关于"},
        {"aboutBack", "返回"},
        {"aboutContent",
        "模拟人生大富翁 \n\n" +
        "这是一个由 AI 生成的人生模拟游戏，在这里，\n" +
        "你将经历千变万化的人生轨迹，\n" +
        "掷骰子，做出选择，见证命运的轮回。\n" +
        "由 AI 负责编写和导演，体验一场独特的赛博人生！\n\n" +
        "开发者\n" +
        "- 制作人: Cynthia Chen\n" +
        "- 联系方式: cynthiachen202304@gmail.com\n" +
        "- 特别感谢: 我的朋友 Florence 帮助我进行测试\n\n" +
        "版本信息\n" +
        "- 版本: v1.0.0\n" +
        "- 发布日期: 2025.03\n" +
        "- 主要功能: AI 生成人物故事、角色成长、人生模拟\n\n" +
        "鸣谢\n" +
        "- 游戏音乐: Suno AI\n" +
        "- 视觉素材: RPG Maker, OpenAI\n" +
        "感谢所有支持本游戏的玩家！\n\n" +
        "© 2025 Cynthia Chen. **All Rights Reserved.**"
            }



    };

    public Dictionary<string, string> englishTexts = new Dictionary<string, string>()
    {
        {"title", "Life Simulation Monopoly"},
        {"startbtn", "Start Game"},
        {"language", "Language: English"},
        {"drawNumber", "Roll Dice"},
        {"age", "Age:"},
        {"health", "HP:"},
        {"wealth", "$:"},
        {"intelligence", "IQ:"},
        {"social", "Soc.:"},
        {"personality", "Pers.:"},
        {"education", "Edu.:"},
        {"career", "Job:"},
        {"happiness", "Joy:"},
        {"eventLogBtn", "..More"},
        {"LFbackToGameBtn", "Back to Game"},
        {"SDbackToGameBtn", "Back to Game"},
        {"CEbackToGameBtn", "Back"},
        {"GOtitle", "Game Over"},
        {"gameOverText", "You have died due to running out of health. Your life journey ends here."},
        {"lifereviewBtn", "Life Review"},
        {"restartBtn", "Restart"},
        { "aboutbtn","About"},
        {"aboutBack", "Back"},
        {"aboutContent",
        "Life Monopoly \n\n" +
        "This is an AI-generated life simulation game where you\n" +
        "experience countless life paths,\n" +
        "roll the dice, make choices, and witness the cycle of fate.\n" +
        "Directed and written by AI, enjoy a unique cyber life journey!\n\n" +
        "Developer\n" +
        "- Producer: Cynthia Chen\n" +
        "- Contact: cynthiachen202304@gmail.com\n" +
        "- Special Thanks: My friend Florence for testing \n\n" +
        "Version Info\n" +
        "- Version: v1.0.0\n" +
        "- Release Date: March 2025\n" +
        "- Key Features AI-generated stories, character growth, life simulation\n\n" +
        "Credits\n" +
        "- Music: Suno AI\n" +
        "- Visual Assets: RPG Maker, OpenAI\n" +
        "- Thanks to all players for your support!\n\n" +
        "© 2025 Cynthia Chen. **All Rights Reserved." }

    };



    public void ToggleLanguage() { 
    
        selectLanguage = (selectLanguage == Language.Chinese) ? Language.English : Language.Chinese;
        UpdateLanguageUI();
    }

    void UpdateLanguageUI()
    {
        Dictionary<string, string> currentTexts = (selectLanguage == Language.Chinese) ? chineseTexts : englishTexts;

        language.text = currentTexts["language"];
        title.text = currentTexts["title"];
        startbtn.text = currentTexts["startbtn"];
        ageText.text = currentTexts["age"];
        healthText.text = currentTexts["health"];
        wealthText.text = currentTexts["wealth"];
        intelligenceText.text = currentTexts["intelligence"];
        socialText.text = currentTexts["social"];
        personalityText.text = currentTexts["personality"];
        educationText.text = currentTexts["education"];
        careerText.text = currentTexts["career"];
        happinessText.text = currentTexts["happiness"];
        eventLogBtn.text = currentTexts["eventLogBtn"];
        LFbackToGameBtn.text = currentTexts["LFbackToGameBtn"];
        SDbackToGameBtn.text = currentTexts["SDbackToGameBtn"];
        CEbackToGameBtn.text = currentTexts["CEbackToGameBtn"];
        GOtitle.text = currentTexts["GOtitle"];
        gameOverText.text = currentTexts["gameOverText"];
        lifereviewBtn.text = currentTexts["lifereviewBtn"];
        restartBtn.text = currentTexts["restartBtn"];
        drawNumber.text = currentTexts["drawNumber"];
        backToMenu.text = currentTexts["aboutBack"];
        content.text = currentTexts["aboutContent"];
        aboutBtn.text = currentTexts["aboutbtn"];

    }

    public void realPlayerSetTrue()
    {
        // 只激活当前性别的角色，隐藏另一个
        maleCharacter.SetActive(gameManager.playerGender == GameManager.Gender.Male);
        femaleCharacter.SetActive(gameManager.playerGender == GameManager.Gender.Female);
    }

    public void realPlayerSetFalse()
    {
        // 直接隐藏所有角色
        maleCharacter.SetActive(false);
        femaleCharacter.SetActive(false);
    }
}
