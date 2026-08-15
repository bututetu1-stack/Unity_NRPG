using UnityEngine;// Spriteや[SerializeField]を使うために必要

[System.Serializable]
public class EnemyParams
{
    [SerializeField] private string id;// 敵キャラを区別するための「識別子」
    public string ID => id;// プロパティ（読み取り専用としてidを返す）

    [SerializeField] private Sprite enemySprite;// 敵キャラの画像
    public Sprite EnemySprite => enemySprite;// プロパティ（読み取り専用としてidを返す）
}