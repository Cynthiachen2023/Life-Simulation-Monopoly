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

    [Header("��Ӣ��-Maincanvas")]
    public TMP_Text title, startbtn, language;

    [Header("��Ӣ��-Gameboard")]
    public TMP_Text drawNumber, ageText, healthText, wealthText, intelligenceText, socialText, personalityText, educationText, careerText, happinessText;
    public TMP_Text eventLogBtn; //����

    [Header("��Ӣ��-lifeReview")]
    public TMP_Text LFbackToGameBtn;

    [Header("��Ӣ��-storyDetail")]
    public TMP_Text SDbackToGameBtn;

    [Header("��Ӣ��-currentEvent")]
    public TMP_Text CEbackToGameBtn;

    [Header("��Ӣ��-Gameover")]
    public TMP_Text GOtitle, gameOverText, lifereviewBtn, restartBtn;

    public GameManager gameManager;

    void Start()
    {
        //��ʱֻ��ʾ���˵�
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

    void OnEnable() // �����ڱ�����ʱ
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
        // ��� UI �߼�����ʾ ScrollView
        currentEventScroll.gameObject.SetActive(true);
        lifeReviewScroll.gameObject.SetActive(true);
        storyDetailScroll.gameObject.SetActive(true);
        ResetScrollPosition(); // ȷ����ʱ�ص�����
    }



    public Dictionary<string, string> chineseTexts = new Dictionary<string, string>()
    {
        {"title", "ģ����������"},
        {"startbtn", "��ʼ��Ϸ"},
        {"language", "���ԣ�����"},
        {"drawNumber", "Ͷ��"},
        {"age", "���䣺"},
        {"health", "������"},
        {"wealth", "�Ƹ���"},
        {"intelligence", "������"},
        {"social", "�罻��"},
        {"personality", "�Ը�"},
        {"education", "������"},
        {"career", "ְҵ��"},
        {"happiness", "���֣�"},
        {"eventLogBtn", "..����"},
        {"LFbackToGameBtn", "������Ϸ"},
        {"SDbackToGameBtn", "������Ϸ"},
        {"CEbackToGameBtn", "����"},
        {"GOtitle", "��Ϸ����"},
        {"gameOverText", "���򽡿�ֵ�ľ��������������ó̵��˽�����"},
        {"lifereviewBtn", "�ع�����"},
        {"restartBtn", "��Ͷ����"}


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
        // ֻ���ǰ�Ա�Ľ�ɫ��������һ��
        maleCharacter.SetActive(gameManager.playerGender == GameManager.Gender.Male);
        femaleCharacter.SetActive(gameManager.playerGender == GameManager.Gender.Female);
    }

    public void realPlayerSetFalse()
    {
        // ֱ���������н�ɫ
        maleCharacter.SetActive(false);
        femaleCharacter.SetActive(false);
    }
}
