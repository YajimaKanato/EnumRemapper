using EnumRemapper.Runtime;
using UnityEngine;

[EnumRemap]
public enum TestEnum
{
    [InspectorName("待機")] Idle,
    [InspectorName("攻撃")] Attack,
    [InspectorName("移動")] Move,
    [InspectorName("ジャンプ")] Jump,
}
