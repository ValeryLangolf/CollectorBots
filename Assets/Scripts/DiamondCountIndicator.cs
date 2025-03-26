using TMPro;
using UnityEngine;

public class DiamondCountIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _label;

    public void UpdateCount(int count) =>
        _label.text = count.ToString();
}