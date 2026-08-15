using UnityEngine;

public class GameConstants
{
    public const string SaveDataKey = "HATENAKIDUNGEON";// データ保存で使うキー

    public const int StartLevel = 1;// ゲーム開始時のレベル
    public const int StartFood = 100;// ゲーム開始時の食料

    public const int InitialFloor = 1;// 最初の階層

    public const int HeroBaseHp = 45;// プレイヤーのHP
    public const int HeroRateHp = 5;// プレイヤーのレベル1ごとのHP

    public const int EnemyBaseHp = 8;// 敵キャラのHP
    public const int EnemyRateHp = 3;// 敵キャラのレベル1ごとのHP

    public const int HeroBaseAttack = 3;// プレイヤーの攻撃力
    public const int HeroRateAttack = 3;// プレイヤーのレベル1ごとの攻撃力

    public const int EncountRate = 50;// 敵キャラとの遭遇率（％）

    public const float MoveSpeed = 3f;// 前進の処理の早さ

    public const float MoveDistance = 3f;// 前進した時のカメラの移動距離

    public const float RecoveryRate = 0.2f;// HPの回復量

    public const int RecoveryCT = 3;// 

    public const float TurnSpeed = 1f;// ターンの処理の待ち時間

    public const float ShakeDuration = 0.2f;// 揺れ演出の揺れの時間（秒）

    public const float ShakeMagnitude = 0.1f; // 揺れ演出の揺れの強さ

    public const float BattleFinishDelay = 1f;// 戦闘終了時の演出ステップの待機時間
}