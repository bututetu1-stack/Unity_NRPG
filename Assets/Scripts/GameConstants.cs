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
}