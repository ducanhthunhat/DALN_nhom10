using UnityEngine.EventSystems;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    public AudioClip mainMenuMusic;
    public AudioClip inGameMusic;

    public AudioClip enemyDestroyedClip;
    public AudioClip towerPlaceClip;
    public AudioClip missionCompleteClip;
    public AudioClip gameOverClip;

    public AudioClip buttonClickClip;
    public AudioClip buttonHoverClip;
    public AudioClip pauseClip;
    public AudioClip unpauseClip;
    public AudioClip speedSlowClip;
    public AudioClip speedNormalClip;
    public AudioClip speedFastClip;
    public AudioClip panelToggleClip;
    public AudioClip warningClip;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            musicSource.volume = 0.4f;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlaySound(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayTowerPlace() => PlaySound(towerPlaceClip);
    public void PlayEnemyDestroyed() => PlaySound(enemyDestroyedClip);
    public void PlayButtonClick() => PlaySound(buttonClickClip);
    public void PlayButtonHover() => PlaySound(buttonHoverClip);
    public void PlayMissionComplete() => PlaySound(missionCompleteClip);
    public void PlayGameOver() => PlaySound(gameOverClip);
    public void PlayPause() => PlaySound(pauseClip);
    public void PlayUnpause() => PlaySound(unpauseClip);
    public void PlaySpeedSlow() => PlaySound(speedSlowClip);
    public void PlaySpeedNormal() => PlaySound(speedNormalClip);

    public void PlaySpeedFast() => PlaySound(speedFastClip);
    public void PlayPanelToggle() => PlaySound(panelToggleClip);
    public void PlayWarning() => PlaySound(warningClip);


    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource.clip == musicClip && musicSource.isPlaying) return;
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlayButtonHover();
    }
}
