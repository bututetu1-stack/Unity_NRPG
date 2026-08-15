using UnityEngine;
using TMPro;

public class StatusWindowController : MonoBehaviour
{
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI FloorText;
    public TextMeshProUGUI FoodText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUnitStatus(UnitStatus unitStatus)
    {
        if (LevelText != null)
        {
            LevelText.text = $"LV {unitStatus.Level}";
        }

        if (HPText != null)
        {
            HPText.text = $"HP {unitStatus.HP} / {unitStatus.GetMaxHP()}";
        }
    }

    public void UpdateDungeonStatus(DungeonStatus dungeonStatus)
    {
        if (FloorText != null)
        {
            FloorText.text = $"{dungeonStatus.Floor}F";
        }

        if (FoodText != null)
        {
            FoodText.text = $"食料{dungeonStatus.Food}";
        }
    }
}
