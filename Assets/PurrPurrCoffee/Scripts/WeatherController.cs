using KarenKrill.Storytelling.Abstractions;
using PurrPurrCoffee.Abstractions;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class WeatherController : MonoBehaviour, IWeatherController
{
    public WeatherType Type
    {
        get => _currentWeatherType;
        set
        {
            if (_currentWeatherType != value)
            {
                _currentWeatherType = value;
                OnWeatherChanged();
            }
        }
    }
    public event Action LightningStrikeEnded;

    public void LightningStrike()
    {
        Debug.LogError($"{nameof(LightningStrike)} weather type");
        _lightningAudioSource.PlayOneShot(_lightningAudioClip);
        StartCoroutine(LightningStrikeCoroutine());
    }

    [SerializeField]
    private AudioClip _dryAudioClip;
    [SerializeField]
    private AudioClip _rainAudioClip;
    [SerializeField]
    private AudioClip _lightningAudioClip;
    [SerializeField]
    private AudioSource _lightningAudioSource;
    [SerializeField]
    private GameObject _lightningStrikeLight;
    [SerializeField]
    private GameObject _creepyMan;

    private WeatherType _currentWeatherType = WeatherType.Dry;
    private AudioSource _mainAudioSource;

    private void Awake()
    {
        _mainAudioSource = GetComponent<AudioSource>();
    }
    private void OnWeatherChanged()
    {
        if (_mainAudioSource.isPlaying)
        {
            _mainAudioSource.Stop();
        }
        switch (_currentWeatherType)
        {
            case WeatherType.Dry:
                Debug.LogError($"Dry weather type");
                if (_dryAudioClip != null)
                {
                    _mainAudioSource.clip = _dryAudioClip;
                    _mainAudioSource.Play();
                }
                break;
            case WeatherType.Rain:
                Debug.LogError($"Rain weather type");
                _mainAudioSource.clip = _rainAudioClip;
                _mainAudioSource.Play();
                break;
            default:
                _mainAudioSource.clip = null;
                break;
        }
    }
    public IEnumerator LightningStrikeCoroutine()
    {
        _lightningStrikeLight.SetActive(true);
        yield return new WaitForSeconds(.1f);
        _lightningStrikeLight.SetActive(false);
        yield return new WaitForSeconds(0.9f);
        _creepyMan.SetActive(true);
        yield return new WaitForSeconds(0.9f);
        _lightningStrikeLight.SetActive(true);
        yield return new WaitForSeconds(.07f);
        _lightningStrikeLight.SetActive(false);
        yield return new WaitForSeconds(.1f);
        _lightningStrikeLight.SetActive(true);
        yield return new WaitForSeconds(.04f);
        _lightningStrikeLight.SetActive(false);
        yield return new WaitForSeconds(9f);
        _lightningStrikeLight.SetActive(true);
        yield return new WaitForSeconds(.1f);
        _lightningStrikeLight.SetActive(false);
        yield return new WaitForSeconds(.2f);
        _lightningStrikeLight.SetActive(true);
        yield return new WaitForSeconds(.05f);
        _lightningStrikeLight.SetActive(false);
        _creepyMan.SetActive(false);
        LightningStrikeEnded?.Invoke();
    }
}
