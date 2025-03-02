using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject gameBoardCanvas;
    public GameObject storyDetailCanvas;
    public GameObject player;
    public GameObject currentEventCanvas;
    public GameObject lifeReviewCanvas;
    public GameObject gameOverCanvas;

    public ScrollRect currentEventScroll;
    public ScrollRect lifeReviewScroll;
    public ScrollRect storyDetailScroll;

    public RectTransform currentEventcontent;
    public RectTransform lifeReviewcontent;
    public RectTransform storyDetailcontent;


    void Start()
    {
        //此时只显示主菜单
        mainMenuCanvas.SetActive(true);
        gameBoardCanvas.SetActive(false);
        storyDetailCanvas.SetActive(false);
        player.SetActive(false);
        currentEventCanvas.SetActive(false);
        lifeReviewCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);

    }

    // Update is called once per frame
    public void StartGame()
    {
        mainMenuCanvas.SetActive(false );
        gameBoardCanvas.SetActive(true);
        storyDetailCanvas.SetActive(false);
        player.SetActive(true);
        currentEventCanvas.SetActive(false);
        lifeReviewCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
    }

    public void storyDetail() { 
        mainMenuCanvas.SetActive(false );
        gameBoardCanvas.SetActive(false);   
        storyDetailCanvas.SetActive(true);
        player.SetActive(false);
        currentEventCanvas.SetActive(false);
    }

    public void currenEvent() {
        mainMenuCanvas.SetActive(false);
        gameBoardCanvas.SetActive(true);
        storyDetailCanvas.SetActive(false);
        player.SetActive(true);
        currentEventCanvas.SetActive(true);

    }

    public void backToGameboard() {
        mainMenuCanvas.SetActive(false);
        gameBoardCanvas.SetActive(true);
        storyDetailCanvas.SetActive(false);
        player.SetActive(true);
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
        player.SetActive(false);
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
}
