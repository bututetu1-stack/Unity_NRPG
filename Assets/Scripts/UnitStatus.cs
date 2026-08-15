using Unity.Collections;
using UnityEngine;

[System.Serializable]
public class UnitStatus
{
    public string ID;
    public int Level;
    public int Exp;
    public int HP;

    public bool IsGuard = false;// ガードしているかどうか
    public int CT = 0;// 回復が使用できるまでのクールタイム 

    public UnitStatus(string id, int bonus)
    {
        ID = id;
        Level = GameConstants.StartLevel + bonus;// レベルの初期値にボーナスを加えて初期化
        Exp = 0;
        HP = GetMaxHP();
    }

    // 最大HPを取得する
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

    // 回復量を取得する
    public int GetRecovery()
    {
        return (int)(GetMaxHP() * GameConstants.RecoveryRate);// 最大HPをRecoveryRateの割合だけ回復
    }

    // 回復
    public void Recovery()
    {
        HP += GetRecovery();
        if (HP > GetMaxHP()) HP = GetMaxHP();// 最大HPを超えないようにする
    }

    // 攻撃力を取得する
    public int GetAttack() 
    {
        if (ID == "HERO")
        {
            return GameConstants.HeroBaseAttack + Level * GameConstants.HeroRateAttack;
        }
        else
        {
            return Level;// 敵キャラはレベルがそのまま攻撃力
        }
    }

    // HPを減らす
    public void SetDamage(int damage) 
    {
        HP -= damage;
        if (HP < 0) HP = 0;
    }

    // CTを進める
    public void ProgressCT() 
    {
        if (CT > 0) CT--;
    }
}