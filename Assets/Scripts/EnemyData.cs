using UnityEngine;
using System.Collections.Generic;// Listを使うのに必要

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private List<EnemyParams> enemyList;

    // enemyListの中からランダムなEnemyParamsを返す
    public EnemyParams GetRandomEnemyParams()
    {
        return enemyList[Random.Range(0, enemyList.Count)];
    }

    // enemyListの中からIDが一致するEnemyParamsを返す
    public EnemyParams GetEnemyParams(string id)
    {
        foreach (EnemyParams enemy in enemyList)
        {
            if (enemy.ID == id) return enemy;
        }
        return null;
    }
}