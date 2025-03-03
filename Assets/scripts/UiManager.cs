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
    public TMP_Text title, startbtn, language;

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

    public GameManager gameManager;

    void Start()
    {
        //此时只显示主菜单
        mainMenuCanvas.SetActive(true);
        gameBoardCanvas.SetActive(false);
        storyDetailCanvas.SetActive(false);
        currentEventCanvas.SetActive(false);
        lifeReviewCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
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
    }

    public void storyDetail() { 
        mainMenuCanvas.SetActive(false );
        gameBoardCanvas.SetActive(false);   
        storyDetailCanvas.SetActive(true);
        realPlayerSetFalse();
        currentEventCanvas.SetActive(false);
    }

    public void currenEvent() {
        mainMenuCanvas.SetActive(false);
        gameBoardCanvas.SetActive(true);
        storyDetailCanvas.SetActive(false);
        realPlayerSetTrue();
        currentEventCanvas.SetActive(true);

    }

    public void backToGameboard() {
        mainMenuCanvas.SetActive(false);
        gameBoardCanvas.SetActive(true);
        storyDetailCanvas.SetActive(false);
        realPlayerSetTrue();
        currentEventCanvas.SetActive(false);
    }

    public void backToGameOver() {
        lifeReviewCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
        gameBoardCanvas.SetActive(false);
        storyDetailCanvas.SetActive(false);
        currentEventCanvas.SetActive(false);
    }

    public void lifeReview() {
        lifeReviewCanvas.SetActive(true);
        gameOverCanvas.SetActive(false);
        mainMenuCanvas.SetActive(false);
        gameBoardCanvas.SetActive(false);
        storyDetailCanvas.SetActive(false);
        currentEventCanvas.SetActive(false);
        realPlayerSetFalse();
    }

    public void Gameover() {
        lifeReviewCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
        gameBoardCanvas.SetActive(false);
        storyDetailCanvas.SetActive(false);
        currentEventCanvas.SetActive(false);
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
        {"restartBtn", "重投再来"}


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
        {"restartBtn", "Restart"}
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
