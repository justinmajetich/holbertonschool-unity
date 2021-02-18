using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;


/// <summary>
/// Maintains a static variable tracking game state and related event invocations.
/// </summary>
public class GameManager : MonoBehaviour
{
    public delegate void GameEvent();
    public static event GameEvent onGameSetup;
    public static event GameEvent onStartGame;
    public static event GameEvent onGameOver;

    [SerializeField] ARSession session;

    // UIs
    public GameObject setupScreen;
    public GameObject inGameScreen;
    public GameObject endScreen;


    private void OnEnable() {
        ARSurfaceManager.onGamePlaneReady += TransitionToInGame;
        Slingshot.onAmmoDepleted += TransitionToEndGame;
        TargetManager.onAllTargetsDestroyed += TransitionToEndGame;
    }

    private void Start() {
        if (!session) { session = GameObject.Find("AR Session").GetComponent<ARSession>(); }
    }

    private void Update() {
        
    }

    void TransitionToSetup() {

        // Activate/De-activate necessary UIs.
        setupScreen.SetActive(true);
        inGameScreen.SetActive(false);
        endScreen.SetActive(false);

        onGameSetup();
    }

    void TransitionToInGame() {

        // Activate/De-activate necessary UIs.
        setupScreen.SetActive(false);
        inGameScreen.SetActive(true);
        endScreen.SetActive(false);

        onStartGame();
    }
    
    void TransitionToEndGame() {

        // Activate/De-activate necessary UIs.
        setupScreen.SetActive(false);
        inGameScreen.SetActive(false);
        endScreen.SetActive(true);

        onGameOver();
    }

    public void NewGame() {
        TransitionToSetup();
    }

    public void Replay() {
        TransitionToInGame();
    }

    public void Quit() {
        Application.Quit();
    }

    private void OnDisable() {
        ARSurfaceManager.onGamePlaneReady -= TransitionToInGame;
        Slingshot.onAmmoDepleted -= TransitionToEndGame;
        TargetManager.onAllTargetsDestroyed -= TransitionToEndGame;
    }



}
