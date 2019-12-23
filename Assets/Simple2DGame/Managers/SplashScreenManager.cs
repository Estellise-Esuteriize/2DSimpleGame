using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
// ReSharper disable once CheckNamespace
public class SplashScreenManager : BaseLoading<SplashScreenManager>
{

    public float speed;
    public float timeLimit;
    
    public Text splashScreenTest;
    public Text developerText;

    public Image background;
    
    
    public override LoadingInformation LoadingInfos
    {
        get; 
        set;
    }

    public override IEnumerator SetLoading
    {
        set => Loading = value;
    }
    protected override SplashScreenManager InstanceType()
    {
        return this;
    }


    protected override void Awake()
    {
        base.Awake();
        
        Initialize();
    }

    protected override void Start()
    {
        base.Start();
        
        Show();
    }

    protected override IEnumerator InitialLoadingCoroutine()
    {
        var time = 0f;

        while (time < timeLimit)
        {

            var color = time / timeLimit;
            
            background.SetColorAlpha(color);
            
            time += Time.deltaTime * speed;
            
            yield return null;
        }
        
        splashScreenTest.SetActive(true);
        developerText.SetActive(true);
    }

    protected override IEnumerator LoadingCoroutine()
    {
        yield return new WaitForSeconds(5f);
    }

    protected override IEnumerator FinalLoadingCoroutine()
    {
        yield return null;
    }

    protected override IEnumerator MedialMovementCoroutine()
    {
        yield return null;
    }

    protected override void Set()
    {
        background.SetColorAlpha(0f);
        background.SetActive(true);
    }

    protected override void Unset()
    {
        Destroy(gameObject);
    }
    
    
    private void Initialize()
    {
        background.SetActive(false);
        
        splashScreenTest.SetActive(false);
        developerText.SetActive(false);
    }

}
