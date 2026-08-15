using UnityEngine;
using UnityEngine.SceneManagement;// ここに追加

public enum SceneCode
{
    Title,
    Dungeon,
    Battle,
    End,
    Start
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

}