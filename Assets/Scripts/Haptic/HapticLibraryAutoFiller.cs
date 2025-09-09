#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using static Constants.Constants;

public class HapticLibraryAutoFiller
{
    [MenuItem("Tools/Haptics/Auto-Fill Default Haptic Types")]
    public static void AutoFill()
    {
        var library = Selection.activeObject as HapticLibrarySO;

        if (library == null)
        {
            Debug.LogError("Please select a HapticLibrarySO asset in the Project window.");
            return;
        }

        library.haptics.Clear();

        void Add(HapticType type, float dur, float freq, float amp, bool bothHands = false)
        {
            var data = new HapticLibrarySO.HapticData
            {
                type = type,
                duration = dur,
                frequency = freq,
                amplitude = amp,
                bothHands = bothHands
            };
            library.haptics.Add(data);
        }

        Add(HapticType.JetpackStart, 0.25f, 0.8f, 0.6f);
        Add(HapticType.JetpackSustain, 0.1f, 0.2f, 0.2f);
        Add(HapticType.GunShoot, 0.1f, 1.0f, 0.6f);
        Add(HapticType.GunReload, 0.2f, 0.6f, 0.4f);
        Add(HapticType.WebAttach, 0.1f, 0.4f, 0.3f);
        Add(HapticType.WebSwing, 0.2f, 0.3f, 0.4f);
        Add(HapticType.PlayerHitLight, 0.15f, 0.6f, 0.5f);
        Add(HapticType.PlayerHitHeavy, 0.3f, 1.0f, 0.8f, true);
        Add(HapticType.EnemyHit, 0.1f, 0.5f, 0.4f);
        Add(HapticType.GrabObject, 0.1f, 0.5f, 0.3f);
        Add(HapticType.ThrowCharge, 0.25f, 0.2f, 0.5f);
        Add(HapticType.ObjectImpact, 0.15f, 0.8f, 0.5f);
        Add(HapticType.LowHealth, 0.3f, 0.2f, 0.5f, true);
        Add(HapticType.CriticalHealth, 0.2f, 0.2f, 1.0f, true);
        Add(HapticType.DeathPulse, 0.4f, 1.0f, 1.0f, true);

        EditorUtility.SetDirty(library);
        AssetDatabase.SaveAssets();

        Debug.Log("HapticLibrarySO has been populated with default values.");
    }
}
#endif
