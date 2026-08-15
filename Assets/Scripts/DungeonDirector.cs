using UnityEngine;
using System.Collections;
using Unity.VectorGraphics;
using JetBrains.Annotations;
using Unity.VisualScripting;

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

        SoundManager.Instance.PlayDungeonBGM();

        cameraTransform = Camera.main.transform;

        StatusWindow.UpdateUnitStatus(GameManager.Instance.PlayRecord.HeroStatus);

        StatusWindow.UpdateDungeonStatus(GameManager.Instance.PlayRecord.DungeonStatus);

        GameManager.Instance.PlayRecord.SceneCode = SceneCode.Dungeon;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TouchMove()
    {
        ButtonMove.SetActive(false);
        ButtonRest.SetActive(false);

        GameManager.Instance.PlayRecord.AdvanceFloor();
        StatusWindow.UpdateDungeonStatus(GameManager.Instance.PlayRecord.DungeonStatus);
        StartCoroutine(MoveForward());
    }

    private IEnumerator MoveForward()
    {
        while (cameraTransform.position.z < GameConstants.MoveDistance)
        {
            cameraTransform.Translate(cameraTransform.forward * GameConstants.MoveSpeed * Time.deltaTime);
            yield return null;
        }

        Camera.main.gameObject.transform.position = Vector3.zero;

        if (GameManager.Instance.PlayRecord.DungeonStatus.Food <= 0)
        {
            GameManager.LoadScene(SceneCode.End);
        }
        else if (GameConstants.EncountRate > Random.Range(0,100))
        {
            GameManager.LoadScene(SceneCode.Battle);
        }
        else
        {
            ButtonMove.SetActive(true);
            ButtonRest.SetActive(true);
        }
    }

    public void TouchRest()
    {
        SoundManager.Instance.PlayRecoverySE();
        GameManager.Instance.PlayRecord.Rest();

        StatusWindow.UpdateUnitStatus(GameManager.Instance.PlayRecord.HeroStatus);
        StatusWindow.UpdateDungeonStatus(GameManager.Instance.PlayRecord.DungeonStatus);

        if (GameManager.Instance.PlayRecord.DungeonStatus.Food <= 0)
        {
            GameManager.LoadScene(SceneCode.End);
        }
    }
}
