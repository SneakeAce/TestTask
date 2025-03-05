using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Configs/ButtonConfig", fileName = "ButtonConfig")]
public class ButtonConfig : ScriptableObject
{
    [field: SerializeField] public Button Button { get; set; }
}
