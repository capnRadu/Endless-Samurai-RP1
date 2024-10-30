using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class FearMeter : MonoBehaviour
{
    [SerializeField] private HeroKnight playerScript;

    [SerializeField] private Image fillImage;
    private float maxFear = 100f;
    private float currentFear = 0f;
    public float CurrentFear { get { return currentFear; } }


    [SerializeField] private Volume volume;
    private ChromaticAberration chromaticAberration;
    private ColorAdjustments colorAdjustments;
    private FilmGrain filmGrain;
    private LensDistortion lensDistortion;

    private void Start()
    {
        fillImage.fillAmount = currentFear / maxFear;

        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out filmGrain);
        volume.profile.TryGet(out lensDistortion);
    }

    private void Update()
    {
        lensDistortion.xMultiplier.value = Mathf.Sin(Time.time * 10) * (currentFear / maxFear);
        lensDistortion.yMultiplier.value = Mathf.Sin(Time.time * 10) * (currentFear / maxFear);

        float hueShiftRange = 180f * (currentFear / maxFear);
        float hueShiftValue = hueShiftRange * Mathf.Sin(Time.time * 2f);
        colorAdjustments.hueShift.value = hueShiftValue;
    }

    public void UpdateFearLevel(float fear)
    {
        currentFear += fear;
        fillImage.fillAmount = currentFear / maxFear;

        chromaticAberration.intensity.value = currentFear / maxFear;
        filmGrain.intensity.value = currentFear / maxFear;

        if (currentFear >= maxFear)
        {
            playerScript.Die();
        }

        Debug.LogWarning("Fear level: " + currentFear);
    }
}
