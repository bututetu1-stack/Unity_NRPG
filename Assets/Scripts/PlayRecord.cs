using UnityEngine;

[System.Serializable]
public class PlayRecord
{
    public SceneCode SceneCode;// ゲームの状態
    public DungeonStatus DungeonStatus;// ダンジョンの状態
    public UnitStatus HeroStatus;// 主人公の状態
    public UnitStatus EnemyStatus;// 敵キャラの状態を保存するクラスの実体

    public int Bonus = 0;// 所有ボーナスポイント
    public int BonusFood = 0;// ボーナスポイントで強化した初期食料
    public int BonusLevel = 0;// ボーナスポイントで強化した初期レベル

    // ダンジョン探索を開始するときの初期化
    public void InitStatus()
    {
        DungeonStatus = new DungeonStatus(BonusFood);
        HeroStatus = new UnitStatus("HERO", BonusLevel);// 主人公としてUnitStatusを実体化
    }

    // 前進したときに呼び出す
    public void AdvanceFloor()
    {
        DungeonStatus.Floor++;
        DungeonStatus.Food--;
    }

    // 休憩したときに呼び出す
    public void Rest()
    {
        HeroStatus.Recovery();// HPを回復する
        DungeonStatus.Food--;
    }

    // レベル強化のコスト
    public int GetLevelCost()
    {
        return GameConstants.UpgradeStartCost + BonusLevel;
    }

    // 食料強化のコスト
    public int GetFoodCost()
    {
        return GameConstants.UpgradeStartCost + BonusFood;
    }

    // レベル強化
    public bool IncreaseLevel()
    {
        if (Bonus >= GetLevelCost())
        {
            Bonus -= GetLevelCost();
            BonusLevel++;
            return true;// 強化成功
        }
        return false;// 強化失敗（ポイント不足）
    }

    // 食料強化
    public bool IncreaseFood()
    {
        if (Bonus >= GetFoodCost())
        {
            Bonus -= GetFoodCost();
            BonusFood++;
            return true;// 強化成功
        }
        return false;// 強化失敗（ポイント不足）
    }
}