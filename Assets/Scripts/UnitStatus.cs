using UnityEngine;

[System.Serializable]
public class UnitStatus
{
    public string ID;
    public int Level;
    public int Exp;
    public int HP;

    public UnitStatus(string id, int bonus)
    {
        ID = id;
        Level = GameConstants.StartLevel + bonus;
        Exp = 0;
        HP = GetMaxHP();
    }


    public int GetMaxHP()
    {
        if (ID == "HERO")
        {
            return GameConstants.HeroBaseHp + Level * GameConstants.HeroRateHp;
        }
        else
        {
            return GameConstants.EnemyBaseHp + Level * GameConstants.EnemyRateHp;
        }
    }

    public int GetRecovery()
    {
        return (int)(GetMaxHP() * GameConstants.RecoveryRate);
    }

    public void Recovery()
    {
        HP += GetRecovery();
        if (HP > GetMaxHP()) HP = GetMaxHP();
    }
}
