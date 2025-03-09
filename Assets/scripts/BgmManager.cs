using UnityEngine;

public class BgmManager : MonoBehaviour
{
    //�������ֵ���������ڿ��� BGM ���š���ͣ��ֹͣ�Ȳ�����
    public AudioSource audioSource;

    //ȷ����Ϸ�� ֻ��һ�� BGMManager ʵ�������������ֹ�����ͬʱ���š�
    public static BgmManager instance;

    public AudioClip menuMusic;
    public AudioClip gameplayMusic;


    private void Awake()
    {
        //��� instance == null���򽫵�ǰ BGMManager ��ΪΨһʵ����
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
