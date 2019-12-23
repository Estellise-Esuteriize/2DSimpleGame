using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Color = System.Drawing.Color;

[DisallowMultipleComponent]
// ReSharper disable once CheckNamespace
public class SimpeLoadingManager : BaseLoading<SimpeLoadingManager>
{

    public Image backgroundImage;

    public Text loadingText;

    public float initializationSpeed = 2f;
    public float finishingSpeed = 2f;
    public float defaultLoading = 5f;


    public override LoadingInformation LoadingInfos
    {
        get; 
        set;
    }
    public override IEnumerator SetLoading
    {
        set => Loading = value;
    }
    
    protected override SimpeLoadingManager InstanceType()
    {
        return this;
    }
    
    protected override IEnumerator InitialLoadingCoroutine()
    {

        var initializationTime = 0f;
        var backgroundImageColor = backgroundImage.color;

        backgroundImageColor.a = 0f;

        while (initializationTime < initializationSpeed)
        {
            backgroundImageColor.a = initializationTime / initializationSpeed;

            backgroundImage.color = backgroundImageColor;
            
            initializationTime += Time.deltaTime;
            
            yield return null;
        }
    }

    protected override IEnumerator LoadingCoroutine()
    {
        if (Loading != null)
        {
            yield return Loading;
        }
        else
        {
            yield return new WaitForSeconds(defaultLoading);
        }

        yield return HideLoadingTextTransition();
    }

    protected override IEnumerator FinalLoadingCoroutine()
    {
        var finishingTime = 0f;
        var backgroundImageColor = backgroundImage.color;

        while (finishingTime < finishingSpeed)
        {
            backgroundImageColor.a = (finishingSpeed - finishingTime) / finishingSpeed;

            backgroundImage.color = backgroundImageColor;

            finishingTime += Time.deltaTime;
            
            yield return null;
        }
    }

    protected override IEnumerator MedialMovementCoroutine()
    {    
        while (true)
        {
            SetLoadingTextAlpha(Mathf.PingPong(Time.time, 1f));
            
            yield return null;
        }
    }

    protected override void Set()
    {        
        var color = loadingText.color;
        color.a = 0f;
        
        loadingText.color = color;

        color = backgroundImage.color;
        color.a = 0f;

        backgroundImage.color = color;
        
        backgroundImage.SetActive(true);
        loadingText.SetActive(true);
    }

    protected override void Unset()
    {
        backgroundImage.SetActive(false);
        loadingText.SetActive(false);

        Loading = null;
    }
    private IEnumerator HideLoadingTextTransition()
    {
        while (true)
        {
            var alpha = loadingText.color.a;
            
            SetLoadingTextAlpha(alpha - Time.deltaTime);
            
            if (alpha <= 0f)
            {
                yield break;
            }

            yield return null;
        }
    }

    private void SetLoadingTextAlpha(float alpha)
    {
        var loadingTextColor = loadingText.color;

        loadingTextColor.a = alpha;

        loadingText.color = loadingTextColor;
    }

}
