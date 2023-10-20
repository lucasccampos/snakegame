using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public delegate void ItemAppliedEvent(Transform t);
    public event ItemAppliedEvent itemUsedEvent;

    public ParticleSystem particlePrefab;

    public string pickSoundName;

    private void Start(){
        Destroy(gameObject, Random.Range(5f, 8f));
    }

    public virtual void ApplyEffect(Snake snake){
        GameManager.instance?.AddScore(1);
        AudioManager.instance.PlaySfx(pickSoundName);
        Destroy(Instantiate(particlePrefab, transform.position, Quaternion.identity), 5f);

        Destroy(gameObject);
    }
}
