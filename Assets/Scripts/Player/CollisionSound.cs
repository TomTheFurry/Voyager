using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public AudioSource collisionEffect;
    public float minVolume;
    public float maxVolume;
    public float minVolumeForce;
    public float maxVolumForce;
    public float minPitch = 0;
    public float maxPitch = 1.5f;


    public void PlayCollisionSound(Collision collision) {
        float force = collision.impulse.sqrMagnitude;
        if (force < minVolumeForce) { return; }
        force = Mathf.Clamp01((force - minVolumeForce) / (maxVolumForce - minVolumeForce));
        float volume = minVolume + (maxVolume - minVolume) * force;
        float randPitch = Random.Range(minPitch, maxPitch);
        Debug.Log("CollisionSound recieved force of " + force + ", volume " + volume + ", pitch " + randPitch);

        AudioSource child = Instantiate(collisionEffect, transform.position, transform.rotation);
        child.volume = volume;
        child.pitch = randPitch;
        child.gameObject.SetActive(true);
        child.Play();
    }
}
