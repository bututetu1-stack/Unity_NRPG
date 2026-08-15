using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource audioSource;

    public AudioClip TitleBGM;
    public AudioClip TouchSE;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
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

    private void PlayBgm (AudioClip audioClip)
    {
        if (audioSource.clip == audioClip) return;

        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void PlayTitleBGM()
    {
        PlayBgm(TitleBGM);
    }

    public void PlayTouchSE()
    {
        audioSource.PlayOneShot(TouchSE);
    }
}
