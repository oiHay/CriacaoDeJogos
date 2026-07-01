using System;
using DG.Tweening;
using UnityEngine;

public class TutorialEnemyController : MonoBehaviour
{
   [Header("Entry Animation")] 
   [SerializeField] private float offScreenZ = 20f;
   [SerializeField] private float entryDuration = 1.5f;
   [SerializeField] private Ease entryEase = Ease.OutCubic;

   [Header("Movement")] 
   [SerializeField] private WaypointPath waypointPath;
   [SerializeField] private float moveSpeed = 3f;

   [Header("Setup")] 
   [SerializeField] private EnemyDataSO enemyData;
   [SerializeField] private GameStatesEventSO gameStatesEvent;

   private EnemyBehaviour _enemyBehaviour;
   private int _currentWaypointIndex;
   private bool _isActive;
   private Vector3 _entryTargetPosition;

   private void Awake()
   {
      _enemyBehaviour = GetComponentInChildren<EnemyBehaviour>();
      _enemyBehaviour.gameObject.SetActive(false);
      _enemyBehaviour.Initialize(enemyData, 0, gameStatesEvent);
      _entryTargetPosition = transform.position;
      gameStatesEvent.OnRaised += HandleStateChanged;
   }

   private void OnDestroy()
   {
      if (gameStatesEvent != null)
         gameStatesEvent.OnRaised -= HandleStateChanged;
   }

   private void HandleStateChanged(GameState state)
   {
      _isActive = state == GameState.Playing;
      _enemyBehaviour.SetGameState(state);
   }

   private void Update()
   {
      if(!_isActive || waypointPath == null) return;

      MoveToWaypoint();
   }

   private void MoveToWaypoint()
   {
      Transform target = waypointPath.GetWayPoint(_currentWaypointIndex);

      transform.position = Vector3.MoveTowards(
         transform.position,
         target.position,
         moveSpeed * Time.deltaTime
      );

      if (Vector3.Distance(transform.position, target.position) < 0.1f)
         _currentWaypointIndex = (_currentWaypointIndex + 1) % waypointPath.GetWaypointCount();
   }

   public void PlayEntryAnimation(Action onComplete = null)
   {
      gameStatesEvent.OnRaised += HandleStateChanged;

      Vector3 startPos = _entryTargetPosition;
      startPos.z = offScreenZ;
      transform.position = startPos;

      transform.DOMoveZ(_entryTargetPosition.z, entryDuration)
         .SetEase(entryEase)
         .OnComplete(() => onComplete?.Invoke());
   }

   public EnemyBehaviour GetBehaviour() => _enemyBehaviour;

   public void ShowEnemy() => _enemyBehaviour.gameObject.SetActive(true);
}
