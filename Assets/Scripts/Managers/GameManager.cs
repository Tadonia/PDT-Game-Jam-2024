using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public SceneLoader SceneLoader { get; private set; }
    public TurnManager TurnManager { get; private set; }
    public UIOverlayManager UIOverlayManager { get; private set; }
    public BattleElementManager BattleElementManager { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public MusicManager MusicManager { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SceneLoader = GetComponentInChildren<SceneLoader>();
        TurnManager = GetComponentInChildren<TurnManager>();
        UIOverlayManager = GetComponentInChildren<UIOverlayManager>();
        BattleElementManager = GetComponentInChildren<BattleElementManager>();
        AudioManager = GetComponentInChildren<AudioManager>();
        MusicManager = GetComponentInChildren<MusicManager>();
    }

    public void BattleStart()
    {
        TurnManager.OnBattleSceneStart();
        UIOverlayManager.OnBattleSceneStart();
    }
}
