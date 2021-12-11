using UnityEngine;
using System.Collections;


//副本类型
public enum CopyType
{
    MoveLimit,
    TimeLimit,
}

//副本模式
public enum CopyMode
{
    ElE = 1,
    TAFANG,
}

//产生元素类型;
public enum GenerolItemType
{
    New,
    Reset,
    Produce,
}

public enum AffectType
{
    NONE = -1,
    AROUND = 0,
    ROW = 1,
    COL = 2,
    WANNENG = 3,
}

public enum ErrorMsgType
{
    None = 0,
    ReLogin = 1,
    BuyRubyFailed = 2,
}

public enum ContinuousGesturePhase
{
    None = 0,
    Started,
    Updated,
    Ended,
}

public enum SwipeDirection
{
    /// <summary>
    /// Moved to the right
    /// </summary>
    Right = 1 << 0,

    /// <summary>
    /// Moved to the left
    /// </summary>
    Left = 1 << 1,

    /// <summary>
    /// Moved up
    /// </summary>
    Up = 1 << 2,

    /// <summary>
    /// Moved down
    /// </summary>
    Down = 1 << 3,

    /// <summary>
    /// North-East diagonal
    /// </summary>
    UpperLeftDiagonal = 1 << 4,

    /// <summary>
    /// North-West diagonal
    /// </summary>
    UpperRightDiagonal = 1 << 5,

    /// <summary>
    /// South-East diagonal
    /// </summary>
    LowerRightDiagonal = 1 << 6,

    /// <summary>
    /// South-West diagonal
    /// </summary>
    LowerLeftDiagonal = 1 << 7,

    //--------------------

    None = 0,
    Vertical = Up | Down,
    Horizontal = Right | Left,
    Cross = Vertical | Horizontal,
    UpperDiagonals = UpperLeftDiagonal | UpperRightDiagonal,
    LowerDiagonals = LowerLeftDiagonal | LowerRightDiagonal,
    Diagonals = UpperDiagonals | LowerDiagonals,
    All = Cross | Diagonals,
}

//public enum AdustType
//{
//    _3T4,
//    _2T3,
//    _10T19,
//    _9T16,
//}

//道具基础类型
public enum EquipEnumID
{
    Invalid = -1,
    Hammer = 0,  //锤子
    AddStep = 1, //增加5步
    AddTime = 3,  //增加30s时间的道具数量
    ResetItem = 5,     //重置item
    Exchange = 8,		//强制交换
    RowColEliminate = 9,//直线消除
    BomEffect = 10,//爆炸效果
    desStep = 100,//减少步数;
    Bomb_SameCor = 101,//消除同色;
}

//产生效果类型;
public enum EquipEffectType
{
    Invalid = -1,
    Hammer = 0,  //锤子
    AddStep = 1, //增加5步
    AddTime = 3,  //增加30s时间的道具数量
    ResetItem = 5,     //重置item
    Exchange = 8,		//强制交换

    BombRow = 9,//行消除
    BombCol = 10,//列消除
    BomEffect = 11,//爆炸效果

    desStep = 100,//减少步数;
    Bomb_SameCor = 101,//消除同色;

}

public enum EquipTypeTmp
{
    AddThree,
    Miracle,
    Specital,
}

public enum ShopType
{
    Equip = 0,
    Tili = 1,
    Zhuanshi = 2,
    Libao = 3,
}

public enum AnimationCurveType
{
    Lerp = 0,
    QUXIAN = 1,
}

public enum SignState
{
    None = 0,
    NotReward = 1,
    HasReward = 2,
    TipReward = 3,
}

public enum ClassID
{
    Player = 1,
    Equip = 2,
}

public enum TFYXState
{
    NONE,
    CANMOVE,
    ENDPOINT,
    DEAD,
}

public enum UIEliminateItemState
{
    None,
    Idle,
    Drop,
    Move,
    FlyToTarget,
}

public enum EliminateState
{
    None,
    GetCombine,
    RemoveCombine,
    WaitRemoveCompleted,
    DropItemOneSquare,
    WaitDropItemCompleted,

    OneEliminateOver,
}