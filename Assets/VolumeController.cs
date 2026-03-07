using System.Collections;
using UnityEngine.Rendering;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private VolumeProfile normal, parry;
    private Volume volume;

    private void Awake()
    {
        volume = GetComponent<Volume>();
        volume.profile = normal;

        EventManager.Parry += HandleParry;
    }
    private void HandleParry()
    {
        StartCoroutine(nameof(ParryCoroutine));
    }
    private IEnumerator ParryCoroutine()
    {
        volume.profile = parry;

        yield return new WaitForSecondsRealtime(0.2f);

        volume.profile = normal;
    }
}
