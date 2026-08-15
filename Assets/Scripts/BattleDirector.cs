using UnityEngine;
using TMPro;
using Unity.Collections;
using UnityEngine.Events;
using System.Collections;// IEnumerator型を使用するために必要

public class BattleDirector : MonoBehaviour
{
    public SpriteRenderer EnemySprite;

    public StatusWindowController StatusController;
    public StatusWindowController EnemyStatusController;

    public GameObject AttackCommand;// Button1（攻撃）のオブジェクト
    public TextMeshProUGUI AttackPower;// Button1のパワーの表示
    public GameObject DefenseCommand;// Button2（防御）のオブジェクト
    public GameObject RecoveryCommand;// Button3（回復）のオブジェクト
    public TextMeshProUGUI RecoveryPower;// Button3のパワーの表示

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

        UnitStatus heroStatus = GameManager.Instance.PlayRecord.HeroStatus;// 主人公のデータを参照

        heroStatus.IsGuard = false;// ガード状態を初期化

        heroStatus.ProgressCT();// CTを進める

        AttackCommand.SetActive(true);// スキルボタンの表示
        DefenseCommand.SetActive(true);
        RecoveryCommand.SetActive(heroStatus.CT == 0);// CTが回復していたら表示

        AttackPower.text = heroStatus.GetAttack().ToString();// 攻撃力の表示
        RecoveryPower.text = heroStatus.GetRecovery().ToString();// 回復力の表示
    }

    // 敵キャラのターン
    private void TurnEnemy()
    {
        UpdateStatus();// ステータスウィンドウの更新

        UnitStatus heroStatus = GameManager.Instance.PlayRecord.HeroStatus;// 主人公のデータを参照
        UnitStatus enemyStatus = GameManager.Instance.PlayRecord.EnemyStatus;// 敵キャラのデータを参照

        UseSkill(enemyStatus, heroStatus, "Attack", TurnHero);
    }

    // スキル処理（スキルの使用者、スキルの対象者、スキルコード、次の処理を引数として渡す）
    private void UseSkill(UnitStatus user, UnitStatus target, string skillCode, UnityAction nextTurn)
    {
        StartCoroutine(BootSkill());

        IEnumerator BootSkill()
        {
            switch (skillCode)
            {
                case "Attack":

                    yield return new WaitForSeconds(GameConstants.TurnSpeed);// TurnSpeedの時間を待ってから処理

                    if (!target.IsGuard)// 対象者がガードしていなければダメージを与える
                    {
                        target.SetDamage(user.GetAttack());
                    }

                    if (nextTurn != null) nextTurn();// 次の処理があればメソッドを呼び出す

                    break;

                case "Defense":
                    yield return new WaitForSeconds(GameConstants.TurnSpeed);

                    user.IsGuard = true; // ガードを有効にする

                    if (nextTurn != null) nextTurn();// 次の処理があればメソッドを呼び出す
                    break;

                case "Recovery":
                    SoundManager.Instance.PlayRecoverySE();
                    user.Recovery(); // 回復する
                    user.CT = GameConstants.RecoveryCT;// CTの設定

                    if (nextTurn != null) nextTurn();// 次の処理があればメソッドを呼び出す
                    break;
            }

            UpdateStatus();

        }
    }

    // 攻撃スキルをタッチしたときの処理（食料を消費する。ターンも進む。）
    public void TouchButton1()
    {
        SoundManager.Instance.PlayTouchSE();

        AttackCommand.SetActive(false);// スキルボタンを全て非表示にする
        DefenseCommand.SetActive(false);
        RecoveryCommand.SetActive(false);

        GameManager.Instance.PlayRecord.DungeonStatus.Food--;
        UseSkill(GameManager.Instance.PlayRecord.HeroStatus, GameManager.Instance.PlayRecord.EnemyStatus, "Attack", TurnEnemy);
    }

    // 防御スキルをタッチしたときの処理（食料を消費する。ターンも進む。）
    public void TouchButton2()
    {
        SoundManager.Instance.PlayTouchSE();

        AttackCommand.SetActive(false);// スキルボタンを全て非表示にする
        DefenseCommand.SetActive(false);
        RecoveryCommand.SetActive(false);

        GameManager.Instance.PlayRecord.DungeonStatus.Food--;
        UseSkill(GameManager.Instance.PlayRecord.HeroStatus, GameManager.Instance.PlayRecord.EnemyStatus, "Defense", TurnEnemy);
    }

    // 回復スキルをタッチしたときの処理（食料を消費しない。ターンも進まない。）
    public void TouchButton3()
    {
        SoundManager.Instance.PlayTouchSE();

        RecoveryCommand.SetActive(false);// 回復ボタンだけ非表示

        UseSkill(GameManager.Instance.PlayRecord.HeroStatus, GameManager.Instance.PlayRecord.EnemyStatus, "Recovery", null);
    }
}