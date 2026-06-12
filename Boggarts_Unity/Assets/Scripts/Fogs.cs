using System.Collections;
using UnityEngine;

public class Fogs : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private ParticleSystem areaFogEffect;
    [SerializeField] private ParticleSystem surroundFogEffect;
    [SerializeField] private ParticleSystem centreFogEffect;
    [SerializeField] private Transform swirlingContainer;
    [SerializeField] private ParticleSystem swirlingFogEffect;
    [SerializeField] private ParticleSystem spookLightEffect;

    [Header("Swirling Settings")] 
    [SerializeField] private float swirlRadius;
    [SerializeField] private float swirlSpeed;
    
    [Header("Spook Lights Settings")]
    [SerializeField] private float spookDisappearChance;
    [SerializeField] private float spookRadius;
    
    [Space]
    [SerializeField] private float spookHappyMinDuration;
    [SerializeField] private float spookHappyMaxDuration;
    [SerializeField] private float spookHappyHideDuration;
    
    [Space]
    [SerializeField] private float spookAngryMinDuration;
    [SerializeField] private float spookAngryMaxDuration;
    [SerializeField] private float spookAngryHideDuration;
    
    private Coroutine _swirlCoroutine;
    private Coroutine _spookLightMovementCoroutine;
    
    // ==== Unity ====
    
    private void Start()
    {
        ShowAreaFog(true);
        //ShowSwirlingAt(true, false, new Vector3(0.5f, 0f, 0f));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spookRadius);
    }

    // ==== Controls ====
    
    public void ShowAreaFog(bool show)
    {
        if (show)
        {
            areaFogEffect.Play();
        }
        else
        {
            areaFogEffect.Stop();
        }
    }

    public void ShowSurroundFogAt(bool show, Vector3 position)
    {
        if (show)
        {
            surroundFogEffect.Play();
            surroundFogEffect.transform.position = new Vector3(position.x, 1f, position.z);
        }
        else
        {
            surroundFogEffect.Stop();
        }
    }

    public void ShowCentreFogAt(bool show, Vector3 position)
    {
        if (show)
        {
            centreFogEffect.Play();
            centreFogEffect.transform.position = new Vector3(position.x, 1f, position.z);
        }
        else
        {
            centreFogEffect.Stop();
        }
    }
    
    public void ShowSwirlingAt(bool show, bool angry, Vector3 position)
    {
        if (show)
        {
            swirlingFogEffect.Play();
            spookLightEffect.Play();
            
            PlaySwirl(true, position);
            PlaySpookLightMovement(true, angry);
        }
        else
        {
            swirlingFogEffect.Stop();
            spookLightEffect.Stop();
            
            PlaySwirl(false, position);
            PlaySpookLightMovement(false, angry);
        }
    }
    
    // ==== Swirl ====

    private void PlaySwirl(bool play, Vector3 position)
    {
        if (spookLightEffect == null)
            return;
        
        if (_swirlCoroutine != null)
        {
            StopCoroutine(_swirlCoroutine);
            _swirlCoroutine = null;
        }

        if (play)
        {
            _swirlCoroutine = StartCoroutine(SwirlingSequence(position));
        }
    }

    private IEnumerator SwirlingSequence(Vector3 position)
    {
        Vector3 basePosition = transform.position;
        basePosition.y = 0f;
        position.y = 0f;

        swirlingContainer.transform.position = position + (position - basePosition).normalized * swirlRadius + new Vector3(0f, 1f, 0f);
        while (true)
        {
            swirlingContainer.transform.position = RotatePointAroundPivot(swirlingContainer.transform.position, position,  swirlSpeed * Time.deltaTime);
            yield return null;
        }
    }
    
    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, float yAngle) 
    {
        return Quaternion.Euler(0, yAngle, 0) * (point - pivot) + pivot;
    }
    
    // ==== Spooklight ====
    
    private void PlaySpookLightMovement(bool show, bool angry)
    {
        if (spookLightEffect == null)
            return;
        
        if (_spookLightMovementCoroutine != null)
        {
            StopCoroutine(_spookLightMovementCoroutine);
            _spookLightMovementCoroutine = null;
        }

        if (show)
        {
            _spookLightMovementCoroutine = StartCoroutine(SpookLightMovementSequence(
                angry ? spookAngryMinDuration: spookHappyMinDuration,
                angry ? spookAngryMaxDuration: spookHappyMaxDuration,
                angry ? spookAngryHideDuration: spookHappyHideDuration));
        }
    }
    
    private IEnumerator SpookLightMovementSequence(float minDuration, float maxDuration, float hideDuration)
    {
        while (true)
        {
            if (Random.Range(0f, 1f) > spookDisappearChance)
            {
                spookLightEffect.Play();
                
                Vector3 randomPoint = Random.insideUnitSphere * spookRadius;
                float randomDuration = Random.Range(minDuration, maxDuration);
                
                float startTime = Time.time;
                Vector3 startPosition = spookLightEffect.transform.localPosition;
                while (Time.time - startTime < randomDuration)
                {
                    spookLightEffect.transform.localPosition = Mathfx.Hermite(startPosition, randomPoint, (Time.time - startTime) / randomDuration);
                    yield return null;
                }
            }
            else
            {
                spookLightEffect.Stop();
                yield return new WaitForSeconds(hideDuration);
            }
        }
    }
}
