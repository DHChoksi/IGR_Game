using System;
using UnityEngine;
using static Constants.Constants;
public class GeneralEvents : MonoBehaviour 
{
    public static Action<bool> OnHurtEffect;

    public static Action<bool> OnGameResult;

    public static Action<SceneName> OnSceneChangeRequest;

}
