using UnityEngine;
using UnityEngine.SceneManagement;// ここに追加

public enum SceneCode
{
    Title,
    Dungeon,
    Battle,
    Result, // Endから変更。Sceneファイル名に合わせました。
    //Start 不要（この行は削除してもらっても構いません）
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;// 静的な変数

    public PlayRecord PlayRecord;// プレイデータ

    public EnemyData EnemyData;// 敵キャラデータ

    // Startメソッドより先に呼び出される
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;// 実体化されたらいつでもアクセス可能
            DontDestroyOnLoad(gameObject);// シーンをまたいでもオブジェクトは消えない

            PlayRecord = new PlayRecord();// プレイデータを作成しておく（仮）
            PlayRecord.InitStatus();// プレイデータの初期化
        }
        else
        {
            Destroy(gameObject);// すでに存在していたら削除
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

    public static void LoadScene(SceneCode sceneCode)
    {
        SceneManager.LoadScene(sceneCode.ToString());
    }

    // 戦闘に勝利したら呼び出す
    public void BattleWon()
    {
        PlayRecord.EnemyStatus = null;
        LoadScene(SceneCode.Dungeon);
    }

    // 戦闘に敗北したら呼び出す
    public void BattleLost()
    {
        PlayRecord.EnemyStatus = null;
        LoadScene(SceneCode.Result);
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(PlayRecord);// PlayRecordをシリアライズする
        PlayerPrefs.SetString(Application.productName, json);// アプリ名をキーにして保存する
    }

    public void Load()
    {
        string json = PlayerPrefs.GetString(Application.productName);// 保存している文字列を取得
        if (string.IsNullOrEmpty(json))
        {
            // 文字列がnullか空白の場合は保存データがない（新規プレイ）
            PlayRecord = new PlayRecord();
            PlayRecord.InitStatus();
        }
        else
        {
            // 文字列を取得できたらPlayRecordクラスにデシリアライズしてPlayRecordに代入する
            PlayRecord = JsonUtility.FromJson<PlayRecord>(json);
        }
    }
}