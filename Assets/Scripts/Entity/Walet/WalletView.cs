using TMPro;
using UnityEngine;

[System.Serializable]
public class WalletView
{
    [SerializeField] private TextMeshProUGUI _label;

    private Wallet _wallet;

    public void Init(Wallet wallet) =>
        _wallet = wallet;

    public void Subscribe() =>
        _wallet.ValueChanged += OnChangingWallet;

    public void Unsubscribe() =>
        _wallet.ValueChanged -= OnChangingWallet;

    public void OnChangingWallet(int count) =>
        _label.text = count.ToString();
}