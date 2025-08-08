using System;

public class Wallet
{
    private int _value;

    public event Action<int> ValueChanged;

    public void Increase() =>
        ValueChanged?.Invoke(++_value);

    public bool TryPay(int value)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException("Значение должно быть положительным");

        if (_value < value)
            return false;

        _value -= value;
        ValueChanged?.Invoke(_value);

        return true;
    }
}