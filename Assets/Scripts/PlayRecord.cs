using UnityEngine;

[System.Serializable]
public class PlayRecord
{
    public SceneCode SceneCode;
    public DungeonStatus DungeonStatus;
    public UnitStatus HeroStatus;
    public UnitStatus EnemyStatus;

    public int Bonus = 0;
    public int BonusFood = 0;
    public int BonusLevel = 0;

    public void InitStatus()
    {
        DungeonStatus = new DungeonStatus(BonusFood);
        HeroStatus = new UnitStatus("HERO", BonusLevel);
        EnemyStatus = null;
    }
}
