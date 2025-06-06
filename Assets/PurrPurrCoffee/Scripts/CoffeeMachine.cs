using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CoffeeMachine : MonoBehaviour
{
    public void MakeCoffee() => StartCoroutine(MakeCoffeeCoroutine());
    public IEnumerator MakeCoffeeCoroutine()
    {
        foreach (var audioClip in _audioClips)
        {
            _audioSource.PlayOneShot(audioClip);
            yield return new WaitForSeconds(audioClip.length);
        }
        _completedCoffeeObject.SetActive(true);
    }

    [SerializeField]
    private List<AudioClip> _audioClips = new();
    [SerializeField]
    private GameObject _completedCoffeeObject;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
}
