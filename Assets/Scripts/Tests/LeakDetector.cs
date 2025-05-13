#if UNITY_EDITOR
using UnityEngine;
using Unity.Collections;

public class LeakDetector : MonoBehaviour
{
    private void Awake()
    {
        //NativeLeakDetection.Mode = NativeLeakDetectionMode.Enabled;
        //Debug.Log("Проверка утечек включена!");
    }
}
#endif
