using UnityEngine;

public class PopupUI : BaseUI
{
    public string PoolKey { get; private set; }

    protected void SetPoolKey(string key)
    {
        PoolKey = key;
    }
}
