using UnityEngine;

public class TitleDirector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.Instance.PlayTitleBGM();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TouchStart()
    {
        SoundManager.Instance.PlayTouchSE();
        GameManager.LoadScene(SceneCode.Dungeon);
    }
}
