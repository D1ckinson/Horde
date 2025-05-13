#if UNITY_EDITOR
using Unity.Collections;
using UnityEngine;

public class LeakTest : MonoBehaviour
{
    private void Start()
    {
        //NativeArray<int> leakyArray = new(100, Allocator.Temp);
    }
}
#endif
