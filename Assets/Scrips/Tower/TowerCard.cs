using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerCard : MonoBehaviour
{
    [SerializeField] private Image TowerImage;
    [SerializeField] private TMP_Text CostText;

    private TowerData _towerData;
    public static event Action<TowerData> OnTowerSelected;
    public void Initialize(TowerData data)
    {
        _towerData = data;
        TowerImage.sprite = data.sprite;
        CostText.text = data.cost.ToString();
    }
    public void PlaceTower()
    {
        OnTowerSelected?.Invoke(_towerData);
    }
}
