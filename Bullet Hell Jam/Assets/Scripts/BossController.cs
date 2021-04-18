using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(EnemyMovement))]
public class BossController : EnemyController
{
    public static event Action OnBossDeath;

    [SerializeField] private List<BossPhase> phasePlaylist;
    [SerializeField] private bool loopPhases;
    private int phaseIndex;

    private EnemyMovement movement;
    private float nextPhaseTime;


    protected override void Awake()
    {
        base.Awake();
        
        movement = GetComponent<EnemyMovement>();

        if (phasePlaylist.Count > 0)
            ChangePhase(phasePlaylist[0]);
    }

    protected override void FixedUpdate()
    {
        if (awake)
            CheckForNextPhase();

        base.FixedUpdate();
    }

    private void CheckForNextPhase()
    {
        if (Time.time >= nextPhaseTime && phaseIndex < phasePlaylist.Count)
        {
            phaseIndex++;

            if (phaseIndex == phasePlaylist.Count)
            {
                if (loopPhases)
                    phaseIndex = 0;
                else
                    return;
            }

            ChangePhase(phasePlaylist[phaseIndex]);
        }
    }

    private void ChangePhase(BossPhase phase)
    {
        Spawner.pattern = phase.weapon;
        movement.movementBehaviour = phase.movement;
        nextPhaseTime = Time.time + phase.phaseTimeLength;
    }

    public override void Die(bool scorePoints = false)
    {
        base.Die();
        OnBossDeath?.Invoke();
    }

    [Serializable]
    private class BossPhase
    {
        public BulletPattern weapon;
        public MovementBehaviour movement;
        public float phaseTimeLength;
    }
}
