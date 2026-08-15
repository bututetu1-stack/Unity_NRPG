using UnityEngine;
using TMPro;

public class BattleDirector : MonoBehaviour
{
    public SpriteRenderer EnemySprite;

    public StatusWindowController StatusController;
    public StatusWindowController EnemyStatusController;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance == null)
        {
            GameManager.LoadScene(SceneCode.Title);
            return;
        }

        SoundManager.Instance.PlayBattleBGM();

        GameManager.Instance.PlayRecord.SceneCode = SceneCode.Battle;

        // エンカウントしたら敵キャラを作成する（ロードされたバトルの続きなら処理しない）
        if (GameManager.Instance.PlayRecord.EnemyStatus == null || string.IsNullOrEmpty(GameManager.Instance.PlayRecord.EnemyStatus.ID))
        {
            EnemyParams randomEnemy = GameManager.Instance.EnemyData.GetRandomEnemyParams();// ランダムで敵キャラのデータを取得

            int enemyLevelBonus = GameManager.Instance.PlayRecord.DungeonStatus.Floor - 1;// 敵キャラの強さの上昇量（レベルアップ量）
            GameManager.Instance.PlayRecord.EnemyStatus = new UnitStatus(randomEnemy.ID, enemyLevelBonus);// 敵キャラを初期化
        }

        EnemyParams enemyParams = GameManager.Instance.EnemyData.GetEnemyParams(GameManager.Instance.PlayRecord.EnemyStatus.ID);
        EnemySprite.sprite = enemyParams.EnemySprite;// 敵キャラ画像を表示

        TurnHero();// 主人公のターンから始まる
    }


    // Update is called once per frame
    //void Update()
    //{

    //}

    // ステータスウィンドウの更新
    private void UpdateStatus()
    {
        StatusController.UpdateUnitStatus(GameManager.Instance.PlayRecord.HeroStatus);// 主人公のステータス
        StatusController.UpdateDungeonStatus(GameManager.Instance.PlayRecord.DungeonStatus);// ダンジョンのステータス
        EnemyStatusController.UpdateUnitStatus(GameManager.Instance.PlayRecord.EnemyStatus);// 敵キャラのステータス
    }

    // 主人公のターン
    private void TurnHero()
    {
        UpdateStatus();// ステータスウィンドウの更新

    }
}