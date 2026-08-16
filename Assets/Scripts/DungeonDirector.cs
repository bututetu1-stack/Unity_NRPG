using UnityEngine;
using System.Collections;// IEnumerator型を使用するために必要

public class DungeonDirector : MonoBehaviour
{
    public StatusWindowController StatusWindow;
    public GameObject ButtonMove;
    public GameObject ButtonRest;

    private Transform cameraTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.Instance == null)
        {
            GameManager.LoadScene(SceneCode.Title);
            return;
        }

        SoundManager.Instance.PlayDungeonBGM();// ダンジョンのBGMを再生

        // メインカメラのtransformを取得
        cameraTransform = Camera.main.transform;

        // 主人公のステータスを表示
        StatusWindow.UpdateUnitStatus(GameManager.Instance.PlayRecord.HeroStatus);

        // ダンジョンのステータスを表示
        StatusWindow.UpdateDungeonStatus(GameManager.Instance.PlayRecord.DungeonStatus);

        GameManager.Instance.PlayRecord.SceneCode = SceneCode.Dungeon;// 現在のシーンを登録
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    // 前進ボタンがタッチされたとき呼び出すメソッド
    public void TouchMove()
    {
        ButtonMove.SetActive(false);// ボタンを非表示にする
        ButtonRest.SetActive(false);

        GameManager.Instance.PlayRecord.AdvanceFloor();// 階層を増やして食料を減らす
        StatusWindow.UpdateDungeonStatus(GameManager.Instance.PlayRecord.DungeonStatus);// ステータスの表示を更新
        StartCoroutine(MoveForward());// 非同期的処理でMoveを呼び出す
    }

    private IEnumerator MoveForward()
    {
        //yield return new WaitForSeconds(GameConstants.MoveSpeed);// MoveSpeed秒だけ待機
        while (cameraTransform.position.z < GameConstants.MoveDistance)// 移動距離まで繰り返す
        {
            cameraTransform.Translate(cameraTransform.forward * GameConstants.MoveSpeed * Time.deltaTime);// z軸方向に移動
            yield return null;// 非同期的処理で1フレームごとに処理をする
        }
 
        Camera.main.gameObject.transform.position = Vector3.zero;// 移動が終わったら元の位置に戻す

        if (GameManager.Instance.PlayRecord.DungeonStatus.Food <= 0)
        {
            GameManager.LoadScene(SceneCode.Result);// 食料切れでゲームオーバー
        }
        else if (GameConstants.EncountRate > Random.Range(0, 100))// 乱数で0から99の値を取得して比較
        {
            GameManager.LoadScene(SceneCode.Battle);// 50%の確率でエンカウント
        }
        else
        {
            ButtonMove.SetActive(true);// 連打を防ぐために一時的に非表示にする
            ButtonRest.SetActive(true);

            GameManager.Instance.Save();// データの保存
        }
    }

    // 休憩ボタンがタッチされたときに呼び出されるメソッド
    public void TouchRest()
    {
        SoundManager.Instance.PlayRecoverySE();// 回復の効果音
        GameManager.Instance.PlayRecord.Rest();// 休憩（HP回復、食料減）

        StatusWindow.UpdateUnitStatus(GameManager.Instance.PlayRecord.HeroStatus);// キャラクターのステータス表示を更新
        StatusWindow.UpdateDungeonStatus(GameManager.Instance.PlayRecord.DungeonStatus);// ダンジョンのステータス表示を更新

        if (GameManager.Instance.PlayRecord.DungeonStatus.Food <= 0)
        {
            GameManager.LoadScene(SceneCode.Result);// 食料切れでゲームオーバー
        }
    }
}