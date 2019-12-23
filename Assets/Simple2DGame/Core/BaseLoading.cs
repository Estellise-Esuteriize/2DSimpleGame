using System;
using System.Collections;
using UnityEngine;

// ReSharper disable once CheckNamespace
public abstract class BaseLoading<T> : SinglePersistentBehaviour<T>
{
    public abstract LoadingInformation LoadingInfos { get; set; }
    public abstract IEnumerator SetLoading { set; }

    protected IEnumerator Loading { get; set; }

    private Coroutine _loading;
    private Coroutine _mesialCoroutine;

    protected abstract override T InstanceType();
    
    public void Show()
    {
        _loading = StartCoroutine(ShowLoadingCoroutine());
    }

    public void Hide()
    {
        StopCoroutine(_loading);
    }

    private IEnumerator ShowLoadingCoroutine()
    {
        Set();
        
        yield return InitialLoadingCoroutine();
        
        ShowMesial();
       
        yield return LoadingCoroutine();
        
        StopMesial();
        
        yield return FinalLoadingCoroutine();
        
        Unset();
    }

    private void ShowMesial()
    {
        _mesialCoroutine = StartCoroutine(MedialMovementCoroutine());
    }

    private void StopMesial()
    {
        StopCoroutine(_mesialCoroutine);
    }

    protected abstract void Set();
    protected abstract void Unset();
    protected abstract IEnumerator InitialLoadingCoroutine();
    protected abstract IEnumerator LoadingCoroutine();
    protected abstract IEnumerator FinalLoadingCoroutine();
    protected abstract IEnumerator MedialMovementCoroutine();
}

public struct LoadingInformation
{
    private float _speed;

    public LoadingAnimationType LoadingAnimation;
    public float TransitionSpeed 
    {
        get 
        {
            if (!IsSpeedValid())
            {
                RetrieveSpeedPreferences();

                if (!IsSpeedValid())
                {
                    return DefaultSpeed();
                }
            }
            return _speed;
        }
        set => _speed = value;
    }

    private bool IsSpeedValid()
    {
        if (_speed <= 0f)
        {
            return false;
        }

        return true;
    }

    private void RetrieveSpeedPreferences()
    {
        _speed = PlayerPrefsManager.RetrievePrefs<float>(Constant.SETTINGS_LOADING_SPEED);
    }

    private float DefaultSpeed()
    {
        return .7f;
    }
}
