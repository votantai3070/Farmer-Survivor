using System;
using UnityEngine;
using UnityEngine.UI;

public enum SoundType { button, bmg, win, lose, hit, levelup, range, melee, dead }

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public static Action<SoundType> onChangeSoundState;

    [SerializeField] private AudioSource buttonSelectSource;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource winSource;
    [SerializeField] private AudioSource loseSource;
    [SerializeField] private AudioSource hitSource;
    [SerializeField] private AudioSource levelupSource;
    [SerializeField] private AudioSource rangeSource;
    [SerializeField] private AudioSource meleeSource;
    [SerializeField] private AudioSource deadSource;

    [SerializeField] private Image openSoundBtn;
    [SerializeField] private Image closeSoundBtn;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        SetSoundState(SoundType.bmg);
        EnableSound();
    }

    private void Start()
    {
        onChangeSoundState += GameSoundStateChangedCallback;
    }
    private void OnDestroy()
    {
        onChangeSoundState -= GameSoundStateChangedCallback;
    }

    private void GameSoundStateChangedCallback(SoundType source)
    {
        switch (source)
        {
            case SoundType.bmg:
                bgmSource.Play();
                break;
            case SoundType.win:
                winSource.Play();
                break;
            case SoundType.lose:
                loseSource.Play();
                break;
            case SoundType.range:
                rangeSource.Play();
                break;
            case SoundType.melee:
                meleeSource.Play();
                break;
            case SoundType.hit:
                hitSource.Play();
                break;
            case SoundType.levelup:
                levelupSource.Play();
                break;
            case SoundType.dead:
                deadSource.Play();
                break;
        }
    }

    public void SetSoundState(SoundType soundType) => onChangeSoundState?.Invoke(soundType);

    public void EnableSound()
    {
        openSoundBtn.transform.GetChild(0).gameObject.SetActive(false);
        closeSoundBtn.transform.GetChild(0).gameObject.SetActive(true);

        buttonSelectSource.volume = .02f;
        bgmSource.volume = .01f;
        winSource.volume = .05f;
        loseSource.volume = .05f;
        rangeSource.volume = .05f;
        meleeSource.volume = .05f;
        levelupSource.volume = .05f;
        deadSource.volume = .05f;
        hitSource.volume = .05f;
    }

    public void DisableSound()
    {
        openSoundBtn.transform.GetChild(0).gameObject.SetActive(true);
        closeSoundBtn.transform.GetChild(0).gameObject.SetActive(false);

        buttonSelectSource.volume = 0;
        bgmSource.volume = 0;
        winSource.volume = 0;
        loseSource.volume = 0;
        rangeSource.volume = 0;
        meleeSource.volume = 0;
        levelupSource.volume = 0;
        deadSource.volume = 0;
        hitSource.volume = 0;
    }
}
