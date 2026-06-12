using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLights : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform torchLightContainer;
    [SerializeField] private Light torchLight;

    [Header("Settings")]
    [SerializeField] private float effectHeight;
    
    [Space]
    [SerializeField] private float lightRandomPositionOffset;
    [SerializeField] private float lightIntensityFrom;
    [SerializeField] private float lightIntensityTo;
    [SerializeField] private float flickerDurationFrom;
    [SerializeField] private float flickerDurationTo;
    
    private Coroutine _flickerCoroutine;

    public void Start()
    {
        //ShowLight(true, new Vector3(2.5f, 0, 0));
    }

    public void ShowLight(bool show, Vector3 position)
    {
        if (_flickerCoroutine != null)
        {
            StopCoroutine(_flickerCoroutine);
            _flickerCoroutine = null;
            torchLight.enabled = false;
        }

        if (show)
        {
            torchLight.enabled = true;
            _flickerCoroutine = StartCoroutine(FlickerSequence());
            position.y = effectHeight;
            torchLightContainer.position = position;
        }
    }
    
    private IEnumerator FlickerSequence()
    {
        while (true)
        {
            Vector3 startPosition = torchLight.transform.localPosition;
            Vector3 endPosition = new Vector3(
                Random.Range(-lightRandomPositionOffset, lightRandomPositionOffset),
                Random.Range(-lightRandomPositionOffset, lightRandomPositionOffset),
                Random.Range(-lightRandomPositionOffset, lightRandomPositionOffset));
            float startIntensity = torchLight.intensity;
            float endIntensity = Random.Range(lightIntensityFrom, lightIntensityTo);
            float duration =  Random.Range(flickerDurationFrom, flickerDurationTo);
            float startTime = Time.time;
            while (Time.time - startTime < duration)
            {
                float t =  (Time.time - startTime) / duration;
                torchLight.transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
                torchLight.intensity = Mathf.Lerp(startIntensity, endIntensity, t);
                yield return null;
            }
        }
    }
}
