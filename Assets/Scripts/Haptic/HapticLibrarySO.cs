using System.Collections.Generic;
using UnityEngine;
using static Constants.Constants;

[CreateAssetMenu(menuName = "Haptics/Haptic Library")]
public class HapticLibrarySO : ScriptableObject
{
    [System.Serializable]
    public class HapticData
    {
        public HapticType type;
        public float duration = 0.1f;
        public float frequency = 0.5f;
        public float amplitude = 0.5f;
        public bool bothHands = false;
    }

    [Header("Haptic Profiles")]
    public List<HapticData> haptics = new List<HapticData>();

    public HapticData GetHaptic(HapticType type)
    {
        return haptics.Find(h => h.type == type);
    }
}
