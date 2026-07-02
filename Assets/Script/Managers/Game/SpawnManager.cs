using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameStatesEventSO gameStateEvent;
    [SerializeField] private RoundSO round;                  // um único round por cena
    [SerializeField] private WaypointPath[] pathsPerWave;    // um path por wave

    private int _currentWaveIndex;
    private int _activeFormationCount;

    private void OnEnable() => gameStateEvent.OnRaised += HandleStateChanged;
    private void OnDisable() => gameStateEvent.OnRaised -= HandleStateChanged;

    private void HandleStateChanged(GameState state)
    {
        if (state == GameState.Playing && _currentWaveIndex == 0 && _activeFormationCount == 0)
            StartCoroutine(SpawnWavesSequence());
    }

    private IEnumerator SpawnWavesSequence()
    {
        while (_currentWaveIndex < round.waves.Length)
        {
            SpawnWave(_currentWaveIndex);
            _currentWaveIndex++;

            if (_currentWaveIndex < round.waves.Length)
                yield return new WaitForSeconds(round.waveInterval);
        }
    }

    private void SpawnWave(int waveIndex)
    {
        WaveSO currentWave = round.waves[waveIndex];

        GameObject formationObj = new GameObject($"Formation_{waveIndex}");
        formationObj.transform.position = pathsPerWave[waveIndex].GetWayPoint(0).position;

        FormationController formation = formationObj.AddComponent<FormationController>();
        formation.OnFormationEmpty += HandleFormationEmpty;
        formation.Initialize(
            pathsPerWave[waveIndex],
            currentWave.formationSpeed,
            currentWave.formationLayout,
            currentWave,
            gameStateEvent,
            waveIndex
        );

        _activeFormationCount++;
    }

    private void HandleFormationEmpty(FormationController formation)
    {
        formation.OnFormationEmpty -= HandleFormationEmpty;
        Destroy(formation.gameObject);
        _activeFormationCount--;

        // round só termina quando todas as formações estiverem vazias
        if (_activeFormationCount <= 0 && _currentWaveIndex >= round.waves.Length)
            PlayerSceneAnimation.Instance.PlayExitAnimation(CustomSceneManager.LoadNextScene);
            //GameManager.Instance.ChangeState(GameState.ChoosingPowerUp);
    }
}
