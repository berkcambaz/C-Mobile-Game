using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static AudioSource audioSrc;

    private static AudioClip upgrade;
    private static AudioClip cantSelect;
    private static AudioClip explosion;
    private static AudioClip obstacleExplosion;
    private static AudioClip playerExplosion;
    private static AudioClip buttonSelect;
    private static AudioClip textButtonSelect;
    private static AudioClip settingChange;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        upgrade = Resources.Load<AudioClip>("Sounds/upgrade");
        cantSelect = Resources.Load<AudioClip>("Sounds/cant_select");
        explosion = Resources.Load<AudioClip>("Sounds/explosion");
        obstacleExplosion = Resources.Load<AudioClip>("Sounds/obstacle_explosion");
        playerExplosion = Resources.Load<AudioClip>("Sounds/player_explosion");
        buttonSelect = Resources.Load<AudioClip>("Sounds/button_select");
        textButtonSelect = Resources.Load<AudioClip>("Sounds/text_button_select");
        settingChange = Resources.Load<AudioClip>("Sounds/setting_change");
    }

    public static void PlaySound(string sound)
    {
        switch (sound)
        {
            case "upgrade":
                audioSrc.PlayOneShot(upgrade);
                break;
            case "cantSelect":
                audioSrc.PlayOneShot(cantSelect);
                break;
            case "explosion":
                audioSrc.PlayOneShot(explosion);
                break;
            case "obstacleExplosion":
                audioSrc.PlayOneShot(obstacleExplosion);
                break;
            case "playerExplosion":
                audioSrc.PlayOneShot(playerExplosion);
                break;
            case "buttonSelect":
                audioSrc.PlayOneShot(buttonSelect);
                break;
            case "textButtonSelect":
                audioSrc.PlayOneShot(textButtonSelect);
                break;
            case "settingChange":
                audioSrc.PlayOneShot(settingChange);
                break;
            default:
                break;
        }
    }
}
