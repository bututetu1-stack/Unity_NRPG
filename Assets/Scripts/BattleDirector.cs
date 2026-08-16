using UnityEngine;
using TMPro;
using Unity.Collections;
using UnityEngine.Events;
using System.Collections;// IEnumerator型を使用するために必要

public class BattleDirector : MonoBehaviour
{
    public GameObject CameraObject;// カメラのオブジェクト（主人公被ダメ時に使用）
    public SpriteRenderer EnemySprite;

    public StatusWindowController StatusController;
    public StatusWindowController EnemyStatusController;

    public GameObject AttackCommand;// Button1（攻撃）のオブジェクト
    public TextMeshProUGUI AttackPower;// Button1のパワーの表示
    public GameObject DefenseCommand;// Button2（防御）のオブジェクト
    public GameObject RecoveryCommand;// Button3（回復）のオブジェクト
    public TextMeshProUGUI RecoveryPower;// Button3のパワーの表示

    public GameObject LevelupObject;// レベルアップしたときに表示するオブジェクト

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

        // 主人公の生死判定
        if (heroStatus.HP <= 0 || GameManager.Instance.PlayRecord.DungeonStatus.Food <= 0)
        {
            StartCoroutine(Lost());
            return;
        }

        heroStatus.IsGuard = false;// ガード状態を初期化

        heroStatus.ProgressCT();// CTを進める

        GameManager.Instance.Save();

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

        // 敵キャラの生死判定
        if (enemyStatus.HP <= 0)
        {
            StartCoroutine(Won());
            return;
        }

        UseSkill(enemyStatus, heroStatus, "Attack", TurnHero, CameraObject);
    }

    // スキル処理（スキルの使用者、スキルの対象者、スキルコード、次の処理を引数、揺らすオブジェクトとして渡す）
    private void UseSkill(UnitStatus user, UnitStatus target, string skillCode, UnityAction nextTurn, GameObject shakeObject)
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
                        if (user.ID == "HERO")
                        {
                            SoundManager.Instance.PlayEnemyDamageSE();// 主人公の攻撃（敵キャラ被ダメ）のときの効果音
                        }
                        else
                        {
                            SoundManager.Instance.PlayDamageSE();// 敵キャラの攻撃（主人公被ダメ）のときの効果音
                        }

                        target.SetDamage(user.GetAttack());

                        StartCoroutine(Shake(shakeObject));
                    }
                    else
                    {
                        SoundManager.Instance.PlayGuardSE();// ガードしているときの効果音
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
        UseSkill(GameManager.Instance.PlayRecord.HeroStatus, GameManager.Instance.PlayRecord.EnemyStatus, "Attack", TurnEnemy, EnemySprite.gameObject);
    }

    // 防御スキルをタッチしたときの処理（食料を消費する。ターンも進む。）
    public void TouchButton2()
    {
        SoundManager.Instance.PlayTouchSE();

        AttackCommand.SetActive(false);// スキルボタンを全て非表示にする
        DefenseCommand.SetActive(false);
        RecoveryCommand.SetActive(false);

        GameManager.Instance.PlayRecord.DungeonStatus.Food--;
        UseSkill(GameManager.Instance.PlayRecord.HeroStatus, GameManager.Instance.PlayRecord.EnemyStatus, "Defense", TurnEnemy, EnemySprite.gameObject);
    }

    // 回復スキルをタッチしたときの処理（食料を消費しない。ターンも進まない。）
    public void TouchButton3()
    {
        SoundManager.Instance.PlayTouchSE();

        RecoveryCommand.SetActive(false);// 回復ボタンだけ非表示

        UseSkill(GameManager.Instance.PlayRecord.HeroStatus, GameManager.Instance.PlayRecord.EnemyStatus, "Recovery", null, EnemySprite.gameObject);
    }

    // 指定されたオブジェクトを小刻みに揺らす処理
    private IEnumerator Shake(GameObject shakeObject)
    {
        // 揺らす前の元の位置を記録
        Vector3 originalPos = shakeObject.transform.localPosition;

        // 経過時間
        float elapsed = 0f;

        // ShakeDuration秒間だけ揺れを繰り返す
        while (elapsed < GameConstants.ShakeDuration)
        {
            // -1～1の範囲でランダムな値を取り、それにmagnitudeを掛けて揺れ幅を決定
            float offsetX = Random.Range(-1f, 1f) * GameConstants.ShakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * GameConstants.ShakeMagnitude;

            // 元の位置にランダムなオフセットを加えて揺らす
            shakeObject.transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);

            // 経過時間を更新
            elapsed += Time.deltaTime;

            // 1フレーム待ってから次のループへ
            yield return null;
        }

        // 揺れが終わったら元の位置に戻す
        shakeObject.transform.localPosition = originalPos;
    }

    IEnumerator Won()
    {
        yield return new WaitForSeconds(GameConstants.BattleFinishDelay);// BattleFinishDelay秒待機

        EnemySprite.sprite = null;// 敵キャラの画像を消す

        yield return new WaitForSeconds(GameConstants.BattleFinishDelay);

        UnitStatus heroStatus = GameManager.Instance.PlayRecord.HeroStatus;// 主人公のデータを参照

        heroStatus.Exp += GameManager.Instance.PlayRecord.EnemyStatus.GetExp();// 経験値を取得する
        if (heroStatus.LevelUp())
        {
            SoundManager.Instance.PlayLevelupSE();
            LevelupObject.gameObject.SetActive(true);
            UpdateStatus();

            yield return new WaitForSeconds(GameConstants.BattleFinishDelay);
        }

        GameManager.Instance.BattleWon();
    }

    IEnumerator Lost()
    {
        SoundManager.Instance.StopBGM();// BGMを即時止める

        yield return new WaitForSeconds(GameConstants.BattleFinishDelay);// BattleFinishDelay秒待機

        GameManager.Instance.BattleLost();
    }
}