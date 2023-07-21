using System;
using UnityEngine;

public static class HMDebug
{
    public static void Log(object o)
    {
        #if UNITY_EDITOR
            Debug.Log(o);
        #endif
    }

    public static void LogError(object o)
    {
        #if UNITY_EDITOR
            Debug.LogError(o);
        #endif
    }

    public static void LogWarning(object o)
    {
        #if UNITY_EDITOR
            Debug.LogWarning(o);
        #endif
    }

    public static void LogAssertion(object o)
    {
        #if UNITY_EDITOR
            Debug.LogAssertion(o);
        #endif
    }

    public static void LogException(Exception e)
    {
        #if UNITY_EDITOR
            Debug.LogException(e);
        #endif
    }
}
