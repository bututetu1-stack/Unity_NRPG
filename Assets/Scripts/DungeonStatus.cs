using UnityEngine;

[System.Serializable]// データをJson化して保存するための属性
public class DungeonStatus
{
    public int Floor;
    public int Food;

    // コンストラクタ（引数にボーナス）
    public DungeonStatus(int bonus)
    {
        Floor = GameConstants.InitialFloor;// 階層を初期化
        Food = GameConstants.StartFood + bonus;// 食料の初期値にボーナスを加えて初期化
    }
}