// ===============================
// TutorialManager.cs
// Gerencia o tutorial do jogo, mostrando mensagens contextuais e controlando o fluxo do tutorial.
// ===============================
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    [Header("Referências de UI")]
    [Tooltip("Placa de tutorial para exibir mensagens")] public PlacaTutorial placaTutorial; // Arraste a placa aqui no Inspector
    [Tooltip("Botão de upgrade que será ativado/desativado pelo tutorial")] public GameObject upgradeButton; // Arraste o botão de upgrade aqui

    private int tutorialStep = 0; // 0: início, 1: trouxe inimigo, 2: upou, 3: tutorial acabou

    void Awake()
    {
        // Inicializa a instância singleton do TutorialManager
        Instance = this;
    }

    void Start()
    {
        // Ativa o botão de upgrade e mostra a primeira mensagem do tutorial
        if (upgradeButton != null)
            upgradeButton.SetActive(true);
        ShowStep0();
    }

    void ShowStep0()
    {
        // Mostra a mensagem inicial do tutorial e desativa o botão de upgrade
        tutorialStep = 0;
        placaTutorial.SetTexto("Vá lá, derrube um deles e traga pra mim");
        if (upgradeButton != null)
            upgradeButton.SetActive(false);
    }

    public void OnEnemySold()
    {
        // Avança o tutorial após o jogador vender o primeiro inimigo
        if (tutorialStep == 0)
        {
            tutorialStep = 1;
            placaTutorial.SetTexto("Boa!! Você ganhou 10 pontos, agora pode upar para carregar mais");
            if (upgradeButton != null)
                upgradeButton.SetActive(true);
        }
    }

    public void OnUpgrade()
    {
        // Avança o tutorial após o jogador fazer o primeiro upgrade
        if (tutorialStep == 1)
        {
            tutorialStep = 2;
            placaTutorial.SetTexto("WoW!!! Voce ficou mais bonito e aparentemente mais forte. tente carregar mais inimigos.");
            Invoke("ShowContinueMessage", 5f); // Após 3 segundos, mostra a mensagem de continuar jogando
        }
        else if (tutorialStep == 2)
        {
            tutorialStep = 3;
            placaTutorial.SetTexto("Ótimo! Agora continue jogando e melhore cada vez mais!");
            Invoke("ClearPlaca", 3f); // Limpa a placa após 3 segundos
        }
    }

    void ShowContinueMessage()
    {
        // Mostra mensagem de incentivo para continuar jogando
        placaTutorial.SetTexto("Veja se tem pontos suficientes para upar!!!");
    }

    void ClearPlaca()
    {
        // Limpa e desativa a placa de tutorial
        placaTutorial.Limpar();
        if (placaTutorial != null && placaTutorial.gameObject != null)
            placaTutorial.gameObject.SetActive(false);
    }
} 