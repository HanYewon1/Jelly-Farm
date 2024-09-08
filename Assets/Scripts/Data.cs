using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
public class Data //ตฅภฬลอ
{
    public int _id;
    public int _level;
    public float _exp;
    public Vector3 _pos;

    public Data(Vector3 _pos, int _id, int _level, float _exp)
    {
        this._pos = _pos;
        this._id = _id;
        this._level = _level;
        this._exp = _exp;
    }
}
