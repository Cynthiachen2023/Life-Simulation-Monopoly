using UnityEngine;

public class BgmManager : MonoBehaviour
{
    //播放音乐的组件，用于控制 BGM 播放、暂停、停止等操作。
    public AudioSource audioSource;

    //确保游戏中 只有一个 BGMManager 实例，避免多个音乐管理器同时播放。
    public static BgmManager instance;

    public AudioClip menuMusic;
    public AudioClip gameplayMusic;


    private void Awake()
    {
        //如果 instance == null，则将当前 BGMManager 设为唯一实例。
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
        

    }
    void Start()
    {
        playMusic(menuMusic);
    }


    public void playMusic(AudioClip music) {

        if (audioSource.clip == music) return;

        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.Play();
    }
}
