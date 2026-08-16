using UnityEngine;
using TMPro;

public class ResultDirector : MonoBehaviour
{
    public TextMeshProUGUI PointText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.Instance == null)
        {
            GameManager.LoadScene(SceneCode.Title);
            return;
        }

        SoundManager.Instance.PlayResultBGM();// BGMの再生

        int point = GameManager.Instance.PlayRecord.DungeonStatus.Floor;// 進んだ階層がそのまま取得できるポイント
        GameManager.Instance.PlayRecord.Bonus += point;// ポイント取得

        PointText.text = point.ToString();// 取得したポイントを表示

        GameManager.Instance.PlayRecord.SceneCode = SceneCode.Title;// タイトルシーンを登録しておく

        GameManager.Instance.Save();// データの保存
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void TouchResult()
    {
        SoundManager.Instance.PlayTouchSE();
        GameManager.LoadScene(SceneCode.Title);// タイトルシーンに遷移
    }
}