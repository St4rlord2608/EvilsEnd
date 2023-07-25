using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : CanBeHit
{
    [SerializeField] protected Transform visualsParent;
    [SerializeField] protected Transform dropOnDeathPrefab;
    [SerializeField] protected Transform ragdollPrefab;

    protected float currentHealth;
    protected int visualChildIndex;
    public abstract void HearsPlayer(Transform player);

    public abstract void HearsSomething(Transform NoiseTransform);

    protected virtual void Awake()
    {
        visualChildIndex = Random.Range(0, visualsParent.childCount - 1);
        Transform visualTransform = visualsParent.GetChild(visualChildIndex);
        visualTransform.gameObject.SetActive(true);
    }

    protected void HandleDeath()
    {
        if (currentHealth <= 0.0f)
        {
            if (lastHitDamage >= 50f)
            {
                lastHitDamage = 200f;
            }
            if (dropOnDeathPrefab != null)
            {
                Instantiate(dropOnDeathPrefab, transform.position, transform.rotation);
            }
            Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
            if(ragdollTransform.TryGetComponent<CorpseRagdoll>(out CorpseRagdoll corpseRagdoll))
            {
                corpseRagdoll.ActivateVisual(visualChildIndex);
            }
            MatchAllChildTransforms(transform, ragdollTransform);
            ApplyExplosionToRagdoll(ragdollTransform, lastHitDamage, lastHitPosition, lastHitDamage);
            if(QuestHandler.Instance != null)
            {
                QuestHandler.Instance.EnemyKilled();
            }
            Destroy(gameObject);
        }
    }

    protected void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                MatchAllChildTransforms(child, cloneChild);
            }
        }
    }

    protected void ApplyExplosionToRagdoll(Transform root, float explosionforce, Vector3 explosionPosition, float explostionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionforce, explosionPosition, explostionRange);
            }
            ApplyExplosionToRagdoll(child, explosionforce, explosionPosition, explostionRange);
        }
    }
}
