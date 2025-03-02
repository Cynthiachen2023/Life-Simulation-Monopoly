using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject gameBoardCanvas;
    public GameObject storyDetailCanvas;
    public GameObject player;
    public GameObject currentEventCanvas;
    public GameObject lifeReviewCanvas;
    public GameObject gameOverCanvas;



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
    }


}
