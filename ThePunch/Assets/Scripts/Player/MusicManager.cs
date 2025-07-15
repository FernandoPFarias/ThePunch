// ===============================
// MusicManager.cs
// Gerencia a música e efeitos sonoros do jogo, incluindo volumes e reprodução de SFX.
// ===============================
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [Header("Referências de Áudio")]
    [Tooltip("Fonte de áudio para música de fundo")] public AudioSource musicSource;
    [Tooltip("Fonte de áudio para efeitos sonoros")] public AudioSource sfxSource;
    [Header("Clipes de Áudio")]
    [Tooltip("Som de passo")] public AudioClip stepClip;
    [Tooltip("Som de soco")] public AudioClip punchClip;
    [Tooltip("Som de upgrade")] public AudioClip upgradeClip;
    [Tooltip("Som de venda")] public AudioClip sellClip;

    void Awake()
    {
        // Inicializa singleton e garante que só exista um MusicManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        // Reproduz música de fundo
        if (musicSource == null) return;
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        // Reproduz efeito sonoro
        if (sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void SetMusicVolume(float volume)
    {
        // Ajusta o volume da música
        if (musicSource != null)
            musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        // Ajusta o volume dos efeitos sonoros
        if (sfxSource != null)
            sfxSource.volume = volume;
    }
} 