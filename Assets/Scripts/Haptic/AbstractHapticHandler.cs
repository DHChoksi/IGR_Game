using UnityEngine;

public abstract class AbstractHapticHandler : MonoBehaviour
{
    public abstract void Init();
    public abstract void Play(HapticLibrarySO.HapticData data);
    public abstract void StopAll();
}
