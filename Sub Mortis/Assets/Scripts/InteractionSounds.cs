using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSounds : MonoBehaviour
{
    HingeJoint hingeJoint;
    ConfigurableJoint configurableJoint;
    AudioSource source;
    Rigidbody rb;
    float limit;
    public AudioClip maxLimitSound;
    public AudioClip minLimitSound;
    public AudioClip moveSound;
    public float maxSpeed = 10f;
    public Vector3 startingPosition;
    public bool atMax = false;
    public bool atMin = false;
    // Start is called before the first frame update
    async void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (GetComponent<HingeJoint>())
        {
            hingeJoint = GetComponent<HingeJoint>();
        }
        else if (GetComponent<ConfigurableJoint>())
        {
            configurableJoint = GetComponent<ConfigurableJoint>();
            limit = Mathf.Round(configurableJoint.linearLimit.limit * 100f) / 100f;
        }
        source = GetComponent<AudioSource>();
        startingPosition = transform.position;
        if (GetComponent<SetPhysicsObjectPosition>())
        {
            GetComponent<SetPhysicsObjectPosition>().SetPosition();
        }
    }
    private void FixedUpdate()
    {
        if (atMax || atMin)
        {
            return;
        }
        if (rb.velocity.magnitude > maxSpeed)
        {
            Debug.Log(rb.velocity.magnitude);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
        if (hingeJoint != null)
        {
            if (rb.angularVelocity.y > 2 && !source.isPlaying)
            {
                source.clip = moveSound;
                source.Play();
            }
        }
        else if (configurableJoint != null)
        {
            //Debug.Log(rb.velocity);
            if (Mathf.Abs(rb.velocity.z) > 1 && !source.isPlaying)
            {
                source.clip = moveSound;
                source.Play();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hingeJoint != null)
        {
            //Debug.Log(hingeJoint.angle - hingeJoint.limits.min > 1);
            if (hingeJoint.limits.max - hingeJoint.angle >= 1)
            {
                atMax = false;
            }
            else if (hingeJoint.limits.max - hingeJoint.angle <= 1 && !atMax)
            {
                PlayMaxSound();
            }
            if (hingeJoint.angle - hingeJoint.limits.min > 1)
            {
                atMin = false;
            }
            else if (hingeJoint.angle - hingeJoint.limits.min <= 1 && !atMin)
            {
                PlayMinSound();
            }
        }

        else if (configurableJoint != null)
        {
            if (Mathf.Approximately((Mathf.Round((configurableJoint.transform.position.z - startingPosition.z) * 100f) / 100f), limit) && !atMin)
            {
                source.Stop();
                source.clip = minLimitSound;
                source.Play();
                atMin = true;
            }
            else if (!Mathf.Approximately((Mathf.Round((configurableJoint.transform.position.z - startingPosition.z) * 100f) / 100f), limit))
            {
                atMin = false;
            }
            if (Mathf.Approximately((Mathf.Round((configurableJoint.transform.position.z - startingPosition.z) * 100f) / 100f), -limit) && !atMax)
            {
                source.Stop();
                source.clip = maxLimitSound;
                source.Play();
                atMax = true;
            }
            else if (!Mathf.Approximately((Mathf.Round((configurableJoint.transform.position.z - startingPosition.z) * 100f) / 100f), -limit))
            {
                atMax = false;
            }
        }
    }
    public void PlayMaxSound()
    {
        source.Stop();
        source.clip = maxLimitSound;
        //source.volume = rb.angularVelocity.y / 2;
        source.Play();
        atMax = true;
    }
    public void PlayMinSound()
    {
        source.Stop();
        source.clip = minLimitSound;
        //source.volume = -rb.angularVelocity.y / 2;
        source.Play();
        atMin = true;
    }
}
