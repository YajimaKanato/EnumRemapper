using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "TestSO", menuName = "Scriptable Objects/TestSO")]
public class TestSO : ScriptableObject
{
    [SerializeField] TestEnum _test;
    [SerializeField] TestEnum[] _testArray;
    [SerializeField] List<TestEnum> _testList;
    [SerializeField] SubClass _class;
    [SerializeField] SubClass[] _subClassArray;
    [SerializeField] List<SubClass> _subClassList;

    [Serializable]
    class SubClass
    {
        [SerializeField] TestEnum _nestTest;
    }
}
