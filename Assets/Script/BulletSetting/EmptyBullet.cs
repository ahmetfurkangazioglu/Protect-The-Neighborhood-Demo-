using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBullet : MonoBehaviour
{
    AudioSource BulletAudio;
 
    void Start()
    {
        BulletAudio = GetComponent<AudioSource>();
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        BulletAudio.Play();
        if (!BulletAudio.isPlaying)
        {
            Destroy(gameObject, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
