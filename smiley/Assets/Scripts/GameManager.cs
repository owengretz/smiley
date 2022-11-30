using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Animator UIanim;
    private int score = 0;
    public RectTransform scoreTransform;
    public Sprite[] numbers;
    public Image[] scoreImages;
    public Image[] highscoreImages;

    [HideInInspector] public bool gameOver;

    private List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();
    private List<IRestartGameObserver> restartGameObservers = new List<IRestartGameObserver>();

    private void Awake()
    {
        instance = this;

        scoreTransform.anchoredPosition = new Vector2(70, -63);
        scoreImages[0].rectTransform.anchoredPosition = new Vector2(-70, 5);
        scoreImages[1].gameObject.SetActive(false);
        scoreImages[2].gameObject.SetActive(false);

        RefreshHighscore();
    }

    public void UpdateScore(int enemiesKilled)
    {
        // update score UI
        score = enemiesKilled;
        string scoreString = enemiesKilled.ToString();
        scoreImages[0].sprite = numbers[(int)char.GetNumericValue(scoreString[scoreString.Length - 1])];
        if (scoreString.Length >= 2)
        {
            scoreImages[1].sprite = numbers[(int)char.GetNumericValue(scoreString[scoreString.Length - 2])];
        }
        if (scoreString.Length == 3)
        {
            scoreImages[2].sprite = numbers[(int)char.GetNumericValue(scoreString[scoreString.Length - 3])];
        }

        // adjusting score so it stays centered when # of digits increases
        if (enemiesKilled == 10)
        {
            scoreTransform.anchoredPosition = new Vector2(35, -63);
            scoreImages[0].rectTransform.anchoredPosition = new Vector2(0, 5);
            scoreImages[1].gameObject.SetActive(true);
            scoreImages[1].rectTransform.anchoredPosition = new Vector2(-70, 5);
        }
        if (enemiesKilled == 100)
        {
            scoreTransform.anchoredPosition = new Vector2(0, -63);
            scoreImages[0].rectTransform.anchoredPosition = new Vector2(70, 5);
            scoreImages[1].rectTransform.anchoredPosition = new Vector2(0, 5);
            scoreImages[2].gameObject.SetActive(true);
            scoreImages[2].rectTransform.anchoredPosition = new Vector2(-70, 5);
        }
    }

    // put into function so that it updates right away when you die & get a highscore
    private void RefreshHighscore()
    {
        if (PlayerPrefs.HasKey("highscore"))
        {
            string highscore = PlayerPrefs.GetInt("highscore").ToString();
            highscoreImages[0].sprite = numbers[(int)char.GetNumericValue(highscore[highscore.Length - 1])];
            if (highscore.Length >= 2)
            {
                highscoreImages[1].sprite = numbers[(int)char.GetNumericValue(highscore[highscore.Length - 2])];
            }
            if (highscore.Length == 3)
            {
                highscoreImages[2].sprite = numbers[(int)char.GetNumericValue(highscore[highscore.Length - 3])];
            }
        }
    }

    // when game ends show fade out & text, button fade in & set highscore if applicable
    public void GameOver()
    {
        gameOver = true;
        UIanim.SetTrigger("game over");
        if (score > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", score);
            RefreshHighscore();
        }
        NotifyEndGameObservers();
    }

    // fade everything out then reload scene
    public void Restart()
    {
        UIanim.SetTrigger("reload");
        Invoke("ReloadScene", 2f);
        NotifyRestartGameObservers();
    }
    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }




    public void AddEndGameObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    public void RemoveEndGameObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    public void NotifyEndGameObservers()
    {
        foreach (IEndGameObserver observer in endGameObservers)
        {
            observer.EndGameNotify();
        }
    }


    public void AddRestartGameObserver(IRestartGameObserver observer)
    {
        restartGameObservers.Add(observer);
    }

    public void RemoveRestartGameObserver(IRestartGameObserver observer)
    {
        restartGameObservers.Remove(observer);
    }

    public void NotifyRestartGameObservers()
    {
        foreach (IRestartGameObserver observer in restartGameObservers)
        {
            observer.RestartGameNotify();
        }
    }
}
