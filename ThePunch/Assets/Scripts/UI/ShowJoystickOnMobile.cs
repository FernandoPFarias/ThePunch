// ===============================
// ShowJoystickOnMobile.cs
// Ativa o joystick virtual apenas em plataformas mobile (Android/iOS).
// ===============================
using UnityEngine;

public class ShowJoystickOnMobile : MonoBehaviour
{
    void Awake()
    {
        // Ativa ou desativa o joystick conforme a plataforma
#if UNITY_ANDROID || UNITY_IOS
        gameObject.SetActive(true);
#else
        gameObject.SetActive(false);
#endif
    }
} 