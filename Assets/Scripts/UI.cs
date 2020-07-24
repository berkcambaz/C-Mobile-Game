using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class UI
{
    public static GameObject notification;
    public static GameObject hud;
    public static GameObject mainMenu;
    public static GameObject upgradesMenu;
    public static GameObject customizeMenu;
    public static GameObject settingsMenu;

    public static GameObject background;
    public static GameObject mapLevelContent;

    public static Text scoreText;
    public static Text highScoreText;
    public static Text mapLevelText;
    public static Text moneyText;
    public static Text expText;

    public static Text qualityText;
    public static Text particleText;
    public static Text fpsText;

    public static GameObject playButton;
    public static GameObject upgradesButton;
    public static GameObject customizeButton;
    public static GameObject settingsButton;

    public static GameObject pauseButton;
    public static GameObject goBackButton;
    public static GameObject backButton;

    public static Vector3 settingsButtonPos;

    public static void Init()
    {
        RefreshSettings();

        settingsButtonPos = settingsButton.transform.localPosition;  // Get settings button's start position

        playButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
        upgradesButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
        customizeButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
        settingsButton.GetComponent<Button>().colors = Utility.ButtonColor(false);

        pauseButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
        goBackButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
        backButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
    }

    public static void Update()
    {
        // Refresh the texts
        mapLevelText.text = UserData.mapLevel.ToString();
        scoreText.text = UserData.score.ToString();
        highScoreText.text = UserData.highScore.ToString();
        moneyText.text = UserData.money.ToString();
    }

    public static void UpdateScore()
    {
        scoreText.text = UserData.score.ToString();
        UserData.highScore = UserData.score > UserData.highScore ? UserData.score : UserData.highScore;
        highScoreText.text = UserData.highScore.ToString();
    }

    public static void LevelEnded()
    {
        // Open main menu & update score
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);    // Reset selected button
        playButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
        settingsButton.transform.localPosition = settingsButtonPos; // Reset settings button's pos to original one

        upgradesButton.SetActive(true);
        customizeButton.SetActive(true);
        mainMenu.SetActive(true);

        Update();
    }

    public static void RefreshSettings()
    {
        RefreshQualityText();
        RefreshParticleText();
        RefreshFpsText();
    }

    private static void RefreshQualityText()
    {
        switch (UserData.quality)
        {
            case true:
                qualityText.text = "high";
                break;
            case false:
                qualityText.text = "low";
                break;
        }
    }

    private static void RefreshParticleText()
    {
        switch (UserData.particles)
        {
            case 3:
                particleText.text = "low";
                break;
            case 5:
                particleText.text = "medium";
                break;
            case 10:
                particleText.text = "high";
                break;
        }
    }

    private static void RefreshFpsText()
    {
        if (UserData.fps == Screen.currentResolution.refreshRate)
        {
            fpsText.text = "default";
            return;
        }

        fpsText.text = UserData.fps.ToString();
    }
}
