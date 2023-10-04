using UnityEngine;

public enum EventID
{
    None = 0,

    SelectBrush = 1,
    UpdatePath = 2,
    DetectFill = 3,
    OnFillDone = 4,
    NextFill = 5,
    CanMouseUp = 6,
    TriggerMouseUp = 7,
    CanMouseDown = 8,

    OnChoiceColor = 100,
    OnShowColor = 101,
    OnUpdateScore = 102,
    OnShowStar = 104,
    OnSpawnFx = 106,

    SendStar = 151,
    SelectPenLeft = 153,
    PauseBrush = 154,

    OnDone = 200,
    LockPenRotate = 201,
    SpawnVFXFill = 202,

    OnChangeScaleCamera = 300,

    /// NewGame
    ///

    OnClickPenItem = 1000,
    OnClickStarRate = 1001,
}

// Message ---

public struct MsgSpawnFX
{
    public int type;
    public Vector3 pos;

    public MsgSpawnFX(int type, Vector3 pos)
    {
        this.type = type;
        this.pos = pos;
    }
}

public struct MsgShowColor
{
    public bool active;
    public int index;

    public MsgShowColor(bool active, int index)
    {
        this.active = active;
        this.index = index;
    }
}

// End ---
