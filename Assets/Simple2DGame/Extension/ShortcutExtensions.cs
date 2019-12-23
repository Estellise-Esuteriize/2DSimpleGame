using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
public static class ShortcutExtensions
{
    public static Text SetColorAlpha(this Text text, float alpha)
    {
        var temp = text.color;

        temp.a = alpha;

        text.color = temp;
        
        return text;
    }
    
    public static Image SetColorAlpha(this Image image, float alpha)
    {
        var temp = image.color;

        temp.a = alpha;

        image.color = temp;
        
        return image;
    }

    public static void SetActive<T>(this T unknownObject, bool value)
    {
        var component = unknownObject as Component;

        if (component == null)
        {
            throw new NullReferenceException("Object is not of type Component");
        }

        component.gameObject.SetActive(value);

    }




}
