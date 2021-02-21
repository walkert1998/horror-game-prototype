using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservablePropSO<T> : ObservableSO
{
    public T _value;

    public T value
    {
        get
        {
            return _value;
        }

        set
        {
            this._value = value;
            Notify();
        }
    }
}
