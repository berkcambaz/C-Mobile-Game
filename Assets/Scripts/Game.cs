using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class Game : MonoBehaviour
{
    public GameObject player;

    public GameObject customizeContent;

    // --- UI STUFF --- //
    public GameObject notification;
    public GameObject hud;
    public GameObject mainMenu;
    public GameObject upgradesMenu;
    public GameObject customizeMenu;
    public GameObject settingsMenu;

    public GameObject background;
    public GameObject levelContent;

    public Text mapLevelText;
    public Text levelText;
    public Text moneyText;

    public Text qualityText;
    public Text particleText;
    public Text fpsText;

    public GameObject playButton;
    public GameObject upgradesButton;
    public GameObject customizeButton;
    public GameObject settingsButton;

    public GameObject pauseButton;
    public GameObject goBackButton;
    public GameObject backButton;

    private SaveData saveData;

    private float durationLimit = -1f;
    private float timer;

    private int selectedIndex = -1;

    void Start()
    {
        ConfigAspectRatio();
        InitGame();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > durationLimit && durationLimit >= 0f)
        {
            ButtonEvent();
            durationLimit = -1f;
        }
    }

    public void ButtonClick(int index)
    {
        timer = 0.001f;
        durationLimit = 0f;

        selectedIndex = index;
        switch (index)
        {
            case 0:     // Play button
                durationLimit = 0.35f * Time.timeScale;
                UI.playButton.GetComponent<Button>().colors = Utility.ButtonColor(true);

                UI.pauseButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
                break;
            case 1:     // Settings button
                SoundManager.PlaySound("textButtonSelect");
                durationLimit = 0.35f * Time.timeScale;
                UI.settingsButton.GetComponent<Button>().colors = Utility.ButtonColor(true);

                UI.backButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
                break;
            case 8:     // Back to menu button without text
                durationLimit = 0.35f * Time.timeScale;
                UI.goBackButton.GetComponent<Button>().colors = Utility.ButtonColor(true);
                UI.backButton.GetComponent<Button>().colors = Utility.ButtonColor(true);

                UI.playButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
                UI.upgradesButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
                UI.customizeButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
                UI.settingsButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
                break;
            case 9:     // Back to menu button with text
                SoundManager.PlaySound("textButtonSelect");
                durationLimit = 0.35f * Time.timeScale;
                UI.goBackButton.GetComponent<Button>().colors = Utility.ButtonColor(true);
                UI.backButton.GetComponent<Button>().colors = Utility.ButtonColor(true);

                UI.playButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
                UI.upgradesButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
                UI.customizeButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
                UI.settingsButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
                break;
            case 11:    // Upgrades button
                SoundManager.PlaySound("textButtonSelect");
                durationLimit = 0.35f * Time.timeScale;
                UI.upgradesButton.GetComponent<Button>().colors = Utility.ButtonColor(true);

                UI.goBackButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
                break;
            case 12:    // Customize button
                SoundManager.PlaySound("textButtonSelect");
                durationLimit = 0.35f * Time.timeScale;
                UI.customizeButton.GetComponent<Button>().colors = Utility.ButtonColor(true);

                UI.goBackButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
                break;
        }
    }

    private void ButtonEvent()
    {
        switch (selectedIndex)
        {
            case 0:
                Menu.PlayButton();
                Play();
                break;
            case 1:
                Menu.SettingsButton();
                break;
            case 2:
                Menu.QualityIncreaseButton();
                SoundManager.PlaySound("settingChange");
                break;
            case 3:
                Menu.QualityDecreaseButton();
                SoundManager.PlaySound("settingChange");
                break;
            case 4:
                Menu.ParticleIncreaseButton();
                SoundManager.PlaySound("settingChange");
                break;
            case 5:
                Menu.ParticleDecreaseButton();
                SoundManager.PlaySound("settingChange");
                break;
            case 6:
                Menu.FpsIncreaseButton();
                SoundManager.PlaySound("settingChange");
                break;
            case 7:
                Menu.FpsDecreaseButton();
                SoundManager.PlaySound("settingChange");
                break;
            case 8: // Back button without text
                Menu.BackToMenuButton();
                break;
            case 9: // Back button with text
                Menu.BackToMenuButton();
                break;
            case 10:
                Menu.PauseButton();

                UI.playButton.GetComponent<Button>().colors = Utility.ButtonColor(false);
                UI.settingsButton.transform.position = UI.upgradesButton.transform.position;
                break;
            case 11:
                Menu.UpgradesButton();
                break;
            case 12:
                Menu.CustomizeButton();
                break;
        }
    }

    private void Play()
    {
        UserData.isPlaying = true;

        if (!UserData.isAlive)
        {
            ObstacleManager.SetupMapLevel();
            UserData.isAlive = true;
            Instantiate(player);
        }
    }

#if DEBUG
    void OnApplicationQuit()    // Runs when the game is quitting on pc
    {
        Save();
    }

#else

    void OnApplicationPause(bool pauseStatus)   // Runs when pressed to home button on android
    {
        if (pauseStatus)
            Save();
        else if (!pauseStatus)
            InitGame();
    }
#endif

    void InitGame()
    {
        // Read "user.save" & write into "SaveData" class
        saveData = SaveSystem.ReadFile();
        if (!saveData.Checksum())   // If true, user has cheated
        {
            saveData.mapLevel = 1;
            saveData.level = 1;
            saveData.money = 25;
            saveData.exp = 0;

            for (int i = 0; i < 64; ++i)
            {
                saveData.skins[i] = false;
                saveData.upgrades[i] = 0;
            }

            saveData.playerSkinIndex = 0;
        }
        if (saveData.fps == 0)  // If first time opening the game, set fps to phone's refresh rate
            saveData.fps = Screen.currentResolution.refreshRate;

        // --- INIT SAVEDATA ---//
        saveData.skins[0] = true;   // Unlock default skin

        // --- SETUP USERDATA --- //
        UserData.mapLevel = saveData.mapLevel;
        UserData.level = saveData.level;
        UserData.money = saveData.money;
        UserData.exp = saveData.exp;

        UserData.skins = saveData.skins;
        UserData.upgrades = saveData.upgrades;

        UserData.customizables = GetCustomizables();
        UserData.skinSprites = GetSkinSprites();

        UserData.playerSkinIndex = saveData.playerSkinIndex;
        UserData.selectedSkinIndex = saveData.playerSkinIndex;  // Set the selected skin

        UserData.quality = saveData.quality;
        UserData.particles = saveData.particles;
        UserData.fps = saveData.fps;

        UserData.isQuitting = false;

        // --- SETUP SETTINGS --- //
        QualitySettings.vSyncCount = -1;
        Application.targetFrameRate = UserData.fps;
        Utility.camera.GetComponent<PostProcessVolume>().enabled = UserData.quality;
        Utility.camera.GetComponent<PostProcessLayer>().enabled = UserData.quality;

        // --- SETUP UI SYSTEM --- //
        UI.notification = notification;
        UI.hud = hud;
        UI.mainMenu = mainMenu;
        UI.upgradesMenu = upgradesMenu;
        UI.customizeMenu = customizeMenu;
        UI.settingsMenu = settingsMenu;

        UI.background = background;
        UI.levelContent = levelContent;

        UI.mapLevelText = mapLevelText;
        UI.levelText = levelText;
        UI.moneyText = moneyText;

        UI.qualityText = qualityText;
        UI.particleText = particleText;
        UI.fpsText = fpsText;

        UI.playButton = playButton;
        UI.upgradesButton = upgradesButton;
        UI.customizeButton = customizeButton;
        UI.settingsButton = settingsButton;

        UI.pauseButton = pauseButton;
        UI.goBackButton = goBackButton;
        UI.backButton = backButton;

        // Init UI with data from save file
        UI.Init();

        // Update UI with data from save file
        UI.Update();

        /* - Init selected skin - */
        customizeMenu.transform.GetChild(0).GetComponent<CustomizeMenu>().InitSkins();
        /* - Init selected skin - */
    }

    GameObject[] GetCustomizables()
    {
        GameObject[] customizables = new GameObject[customizeContent.transform.childCount];

        for (int i = 0; i < customizables.Length; ++i)
            customizables[i] = customizeContent.transform.GetChild(i).gameObject;

        return customizables;
    }

    Sprite[] GetSkinSprites()
    {
        Sprite[] sprites = new Sprite[UserData.customizables.Length];

        string path = "Sprites/players/";
        sprites[0] = Resources.Load<Sprite>(path + "default");
        sprites[1] = Resources.Load<Sprite>(path + "rainbox");
        sprites[2] = Resources.Load<Sprite>(path + "checkerboard");
        sprites[3] = Resources.Load<Sprite>(path + "patterned");
        sprites[4] = Resources.Load<Sprite>(path + "symmetry");
        sprites[5] = Resources.Load<Sprite>(path + "mr_cube");
        sprites[6] = Resources.Load<Sprite>(path + "confusion");
        sprites[7] = Resources.Load<Sprite>(path + "illusion");
        sprites[8] = Resources.Load<Sprite>(path + "cubstacle");

        //for (int i = 0; i < sprites.Length; ++i)
        //    sprites[i] = UserData.customizables[i].transform.GetChild(0).GetComponent<Image>().sprite;

        return sprites;
    }

    void Save()
    {
        UserData.isQuitting = true;

        // --- UPDATE SAVEDATA --- //
        saveData.mapLevel = UserData.mapLevel;
        saveData.level = UserData.level;
        saveData.money = UserData.money;
        saveData.exp = UserData.exp;

        saveData.skins = UserData.skins;
        saveData.upgrades = UserData.upgrades;

        saveData.playerSkinIndex = UserData.playerSkinIndex;

        saveData.quality = UserData.quality;
        saveData.particles = UserData.particles;
        saveData.fps = UserData.fps;

        // Write checksum
        saveData.Checksum();

        SaveSystem.WriteFile(saveData);
    }

    void ConfigAspectRatio()
    {
        float size = 5f;

        Vector2 defRes = new Vector2(1080f, 1920f);
        Vector2 currRes = new Vector2(Screen.width, Screen.height);
        float dt = size * (defRes.x / defRes.y);

        Utility.camera.orthographicSize = dt / (currRes.x / currRes.y);
        //  (dt / ( defRes.x / defRes.y )   Native resolution of the game
        //  (dt / (currRes.x / currRes.y)   Phone's resolution
    }
}
