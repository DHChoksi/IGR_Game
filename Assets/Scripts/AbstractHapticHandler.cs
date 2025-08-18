using UnityEngine;

[CreateAssetMenu(fileName = "HapticHandler", menuName = "IGR/NewScriptableObjectScript")]
public abstract class AbstractHapticHandler : ScriptableObject
{
    public abstract void Play(HapticProfileSO.HapticData data);
    public abstract void StopAll();
}
