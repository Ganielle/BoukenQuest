using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacterController : MonoBehaviour
{
    [SerializeField] private BattleSystemController battleSystem;
    [SerializeField] private ParticleSystem playerAura;
    [SerializeField] private GameObject magicAttack;
    [SerializeField] private Vector3 magicPositionOffset;

    public void SummonAttack()
    {
        playerAura.Stop();
        GameObject attackObj = Instantiate(magicAttack);
        attackObj.GetComponent<BattleAttackController>().finalAction = () =>
        {
            battleSystem.NextPhaseAfterAttack();
        };
        attackObj.transform.position = battleSystem.EnemyTF().position - magicPositionOffset;
    }

    public void PlayerDeath() => battleSystem.GameOverPlayer();

    public void EnemyDeath() => battleSystem.NextEnemy();

    public void SummonEnemyAttack()
    {
        playerAura.Stop();
        GameObject attackObj = Instantiate(magicAttack);
        attackObj.GetComponent<BattleAttackController>().finalAction = () =>
        {
            battleSystem.NexPhaseEnemyToPlayer();
            battleSystem.WinPlayer();
        };
        attackObj.transform.position = battleSystem.PlayerTF().position;
    }
}
