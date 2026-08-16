using UnityEngine;
using TMPro;

public class TitleDirector : MonoBehaviour
{
    public GameObject PointObject;// 強化UI
    public TextMeshProUGUI LevelPowerText;// 現在のレベル強化状態
    public TextMeshProUGUI LevelCostText;// レベル強化のコスト
    public TextMeshProUGUI FoodPowerText;// 現在の食料強化状態
    public TextMeshProUGUI FoodCostText;// 食料強化のコスト
    public TextMeshProUGUI PointText;// 残りのポイント

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.Instance.PlayTitleBGM();
        PointObject.SetActive(false);// 強化UIを非表示
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TouchStart()
    {
        SoundManager.Instance.PlayTouchSE();

        GameManager.Instance.Load();// データのロード

        // データがタイトルシーンのとき（ゲーム新規）
        if (GameManager.Instance.PlayRecord.SceneCode == SceneCode.Title)
        {
            if (GameManager.Instance.PlayRecord.Bonus > 0)// ポイントがあるときは強化UIを表示
            {
                PointObject.SetActive(true);
                ShowPoint();
                return;
            }

            GameManager.Instance.PlayRecord.InitStatus();// データ初期化
            GameManager.LoadScene(SceneCode.Dungeon);// ダンジョンからスタート
        }
        else
        {
            // データがタイトルシーン以外のとき（ゲーム再開）
            GameManager.LoadScene(GameManager.Instance.PlayRecord.SceneCode);
        }

        
    }

    // 強化UIの表示を更新
    public void ShowPoint()
    {
        LevelPowerText.text = (GameConstants.StartLevel + GameManager.Instance.PlayRecord.BonusLevel).ToString();// 現在のレベル強化状態
        FoodPowerText.text = (GameConstants.StartFood + GameManager.Instance.PlayRecord.BonusFood).ToString();// 現在の食料強化状態

        LevelCostText.text = $"UP({GameManager.Instance.PlayRecord.GetLevelCost().ToString()})";// レベル強化のコスト
        FoodCostText.text = $"UP({GameManager.Instance.PlayRecord.GetFoodCost().ToString()})";// 食料強化のコスト

        PointText.text = GameManager.Instance.PlayRecord.Bonus.ToString();// 残りのポイント
    }

    public void TouchLevelup()
    {
        if (GameManager.Instance.PlayRecord.IncreaseLevel())
        {
            SoundManager.Instance.PlayLevelupSE();
            ShowPoint();
        }
    }

    public void TouchFoodUp()
    {
        if (GameManager.Instance.PlayRecord.IncreaseFood())
        {
            SoundManager.Instance.PlayLevelupSE();
            ShowPoint();
        }
    }

    public void TouchPointStart()
    {
        SoundManager.Instance.PlayTouchSE();
        GameManager.Instance.PlayRecord.InitStatus();// データ初期化
        GameManager.LoadScene(SceneCode.Dungeon);// ダンジョンからスタート
    }
}