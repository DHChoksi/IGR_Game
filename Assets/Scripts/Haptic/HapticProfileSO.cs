using System.Collections.Generic;
using UnityEngine;
using static Constants.Constants;

[CreateAssetMenu(fileName = "HapticProfileSO", menuName = "IGR/HapticProfileSO")]
public class HapticProfileSO : ScriptableObject
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

    public List<HapticData> haptics = new List<HapticData>();

    public HapticData GetHaptic(HapticType type)
    {
        return haptics.Find(h => h.type == type);
    }
}
