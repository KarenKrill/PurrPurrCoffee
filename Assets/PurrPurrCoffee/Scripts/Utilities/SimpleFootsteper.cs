using KarenKrill.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class SimpleFootsteper : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> _footstepSamples = new();
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void StepEvent(AnimationEvent _)
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
        _audioSource.PlayOneShot(_footstepSamples[Random.Range(0, _footstepSamples.Count)]);
    }
}
