using UnityEngine;
using static Constants.Constants;

// How to use

// Jetpack
//  HapticManager.Instance.Play(HapticType.JetpackStart);

// Gun
// HapticManager.Instance.Play(HapticType.GunShoot);

// Web swinging
// HapticManager.Instance.Play(HapticType.WebAttach);

// Player damage
// HapticManager.Instance.Play(HapticType.PlayerHitLight);

// Stop all ongoing haptics
// HapticManager.Instance.StopAllHaptics();


public class HapticManager : MonoBehaviour
{
    public static HapticManager Instance { get; private set; }

    [Header("Haptic Library")]
    public HapticLibrarySO hapticLibrary;

    private AbstractHapticHandler hapticHandler;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        DontDestroyOnLoad(gameObject);
        InitHandler();
    }

    private void InitHandler()
    {
        var handlerObj = new GameObject("Haptic_Handler");
        handlerObj.transform.SetParent(transform);

        hapticHandler = handlerObj.AddComponent<QuestHapticHandler>();
        hapticHandler.Init();
    }

    public void Play(HapticType type)
    {
        var data = hapticLibrary.GetHaptic(type);
        hapticHandler.Play(data);
    }

    public void StopAllHaptics()
    {
        hapticHandler.StopAll();
    }
}
