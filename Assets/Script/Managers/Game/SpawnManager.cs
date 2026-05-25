using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameStatesEventSO gameStateEvent;
    [SerializeField] private RoundSO round;                  // um único round por cena
    [SerializeField] private WaypointPath[] pathsPerWave;    // um path por wave

    private int _currentWaveIndex;
    private FormationController _currentFormation;

    private void OnEnable() => gameStateEvent.OnRaised += HandleStateChanged;
    private void OnDisable() => gameStateEvent.OnRaised -= HandleStateChanged;

    private void HandleStateChanged(GameState state)
    {
        if (state == GameState.Playing && _currentFormation == null)
            StartCoroutine(StartRound());
    }

    private IEnumerator StartRound()
    {
        _currentWaveIndex = 0;
        yield return StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        WaveSO currentWave = round.waves[_currentWaveIndex];

        GameObject formationObj = new GameObject("Formation");
        formationObj.transform.position = pathsPerWave[_currentWaveIndex].GetWayPoint(0).position;

        _currentFormation = formationObj.AddComponent<FormationController>();
        _currentFormation.OnFormationEmpty += HandleFormationEmpty;
        _currentFormation.Initialize(
            pathsPerWave[_currentWaveIndex],
            currentWave.formationSpeed,
            currentWave.formationLayout,
            currentWave,
            gameStateEvent,
            _currentWaveIndex
        );

        yield return new WaitForSeconds(round.waveInterval);

        if (_currentWaveIndex < round.waves.Length - 1)
        {
            _currentWaveIndex++;
            yield return StartCoroutine(SpawnWave());
        }
    }

    private void HandleFormationEmpty()
    {
        _currentFormation.OnFormationEmpty -= HandleFormationEmpty;
        StopAllCoroutines();
        Destroy(_currentFormation.gameObject);
        _currentFormation = null;

        // verifica se ainda há waves no round
        if (_currentWaveIndex < round.waves.Length - 1)
        {
            // ainda há waves — continua o round
            _currentWaveIndex++;
            StartCoroutine(SpawnWave());
        }
        else
        {
            // todas as waves concluídas — round termina
            GameManager.Instance.ChangeState(GameState.ChoosingPowerUp);
        }
    }

    public void OnPowerUpChosen()
    {
        GameManager.Instance.ChangeState(GameState.Playing);
    }
}
