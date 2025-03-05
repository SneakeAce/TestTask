using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/CameraConfig", fileName = "CameraConfig")]
public class CameraConfig :  ScriptableObject
{
    [field: SerializeField] public float SensitivityCamera { get; private set; }
}
