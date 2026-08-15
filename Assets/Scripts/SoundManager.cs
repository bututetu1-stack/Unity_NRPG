using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource[] audioSources;// アタッチした2つのAudioSourceを代入
    private AudioSource audioSource;
    private AudioSource bgmSource;// BGM用のAudioSource

    public AudioClip TitleBGM;
    public AudioClip DungeonBGM;
    public AudioClip BattleBGM;
    public AudioClip TouchSE;
    public AudioClip RecoverySE;
    public AudioClip DamageSE;
    public AudioClip EnemyDamageSE;
    public AudioClip GuardSE;
    public AudioClip LevelupSE;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSources = GetComponents<AudioSource>();// アタッチした2つのAudioSourceを配列で取得
            audioSource = audioSources[0];// 1つめを効果音用にする
            bgmSource = audioSources[1];// 2つめをBGM用にする
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PlayBgm(AudioClip audioClip)
    {
        if (bgmSource.clip == audioClip) return;// すでに再生されているBGMなら処理しない

        bgmSource.clip = audioClip;
        bgmSource.Play();
    }

    public void PlayTitleBGM()
    {
        PlayBgm(TitleBGM);
    }
    public void PlayDungeonBGM()
    {
        PlayBgm(DungeonBGM);
    }
    public void PlayBattleBGM()
    {
        PlayBgm(BattleBGM);
    }

    public void PlayTouchSE()
    {
        audioSource.PlayOneShot(TouchSE);
    }

    public void PlayRecoverySE()
    {
        audioSource.PlayOneShot(RecoverySE);
    }

    public void PlayDamageSE()
    {
        audioSource.PlayOneShot(DamageSE);
    }

    public void PlayEnemyDamageSE()
    {
        audioSource.PlayOneShot(EnemyDamageSE);
    }

    public void PlayGuardSE()
    {
        audioSource.PlayOneShot(GuardSE);
    }

    public void PlayLevelupSE()
    {
        audioSource.PlayOneShot(LevelupSE);
    }

    public void StopBGM()
    {
        bgmSource.Stop();// BGMを止める
    }
}