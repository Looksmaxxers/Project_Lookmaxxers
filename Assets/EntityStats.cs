using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityStats
{
    public void TakeDamage(int damage);
    public IEnumerator Die();

    public void StartStagger();

    public void EndStagger();

    public void OnAttackBegin();

    public void OnAttackEnd();
}
