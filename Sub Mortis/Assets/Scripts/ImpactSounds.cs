using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSounds : MonoBehaviour
{
    AudioSource source;
    public List<AudioClip> impactSounds;
    public AudioClip slidingSound;
    public bool collided = false;
    public float threshold;
    public Vector3 prevVelocity;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    void LateUpdate()
    {
        if (collided)
        {
            //if (rb.velocity.magnitude > Mathf.Abs(threshold))
            //{
            //    if (source.clip != slidingSound)
            //    {
            //        //int n = Random.Range(1, slidingSounds.Count);
            //        source.clip = slidingSound;
            //        //source.loop = true;
            //        source.volume = rb.velocity.magnitude;
            //        source.Play();
            //    }
            //}
            //else
            //{
            //    source.Stop();
            //}
        }
    }

    void OnCollisionEnter()
    {
        if (Vector3.Distance(prevVelocity, rb.velocity) > Mathf.Abs(threshold))
        {
            int n = Random.Range(1, impactSounds.Count);
            source.clip = impactSounds[n];
            impactSounds[n] = impactSounds[0];
            impactSounds[0] = source.clip;
            source.volume = rb.velocity.magnitude;
            source.PlayOneShot(source.clip);
        }
        collided = true;
    }

    void OnCollisionExit()
    {
        if (source.clip == slidingSound)
        {
            source.loop = false;
            source.Stop();
        }
        collided = false;
    }
}
