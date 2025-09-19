using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Collectable : MonoBehaviour
{
    public CollectablesManager collectablesManager;

    public GameObject collectableIcon;
    public ParticleSystem collectableEffect;
    public Collider collider;
    public Animator animator;
    public string animationOffsetParameterName = "offset";

    [Header("Collectable settings")]
    public string collectableGroup;
    public float animationOffset = 0f;

    public void Collect()
    {
        collectableIcon.SetActive(false);
        collider.enabled = false;

        if (collectableEffect != null)
        {
            collectableEffect.Play();
        }

        if (collectablesManager != null)
        {
            collectablesManager.Collect(collectableGroup);
        }
    }

    public void Reset()
    {
        collectableIcon.SetActive(true);
        collider.enabled = true;
        if (collectableEffect != null)
        {
            collectableEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    public void Setup()
    {
        collectablesManager = FindAnyObjectByType<CollectablesManager>();
    }

    private void Start()
    {
        animator.SetFloat(animationOffsetParameterName, animationOffset);
        Reset();
    }
    private void OnTriggerEnter(Collider other)
    {
        Collect();
    }
}
