/*
 * Tween - A programmer friendly tween library that makes your lerps PRO.
 
 * Vers.: 0.1.0
 * Made by Ivan S. Cavalheiro (Unity 3D programmer)
 * Made in 2018-06-09
 * License: MIT
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public enum AnimationEase
{
    LINEAR,
    SPRING,
    QUAD_IN,
    QUAD_OUT,
    QUAD_IN_OUT,
    CUBIC_IN,
    CUBIC_OUT,
    CUBIC_IN_OUT,
    BOUNCE_IN,
    BOUNCE_OUT,
    ELASTIC_IN,
    ELASTIC_OUT,
    ELASTIC_IN_OUT,
    QUART_IN,
    QUART_OUT,
    QUART_IN_OUT,
    QUINT_IN,
    QUINT_OUT,
    QUINT_IN_OUT,
    SINE_IN,
    SINE_OUT,
    SINE_IN_OUT,
    EXPO_IN,
    EXPO_OUT,
    EXPO_IN_OUT,
    CIRC_IN,
    CIRC_OUT,
    CIRC_IN_OUT,
    BACK_IN,
    BACK_OUT,
    BACK_IN_OUT
}

public static class Tween {
    #region static methods
    public static T Lerp<T>(T from, T to, float t, AnimationEase ease = AnimationEase.LINEAR){
		//apply ease to T
		t = GetTransaction(0, 1, t, ease);
		
		//using dynamic to avoid convertions
		dynamic fromD = from;
		dynamic toD = to;

		//this might cause an "RuntimeBinderException" if "from" and "to" are not valid quantitable objects
		dynamic delta = toD - fromD;
		
		//return start value plus the necessary delta according to "t"
		return (T)(fromD + (delta * t));
    }

    public static T Lerp<T>(T from, T to, float t, AnimationCurve curve){
		return Lerp(from, to, curve.Evaluate(t));
    }

	public static IEnumerator Interval(float seconds, Action<float> callback, AnimationEase ease = AnimationEase.LINEAR){
		float counter = 0;
		seconds = Mathf.Abs(seconds);
		while(counter < seconds){
			yield return null;
			counter += Time.deltaTime;
			callback(Lerp(0f, 1f, counter/seconds, ease));
		}
	}

	public static IEnumerator Interval(float seconds, Action<float> callback, AnimationCurve curve){
		float counter = 0;
		seconds = Mathf.Abs(seconds);
		while(counter < seconds){
			yield return null;
			counter += Time.deltaTime;
			callback(Lerp(0f, 1f, counter/seconds, curve));
		}
	}

    #endregion

    #region Math stuff
	private static float GetTransaction(float p_start, float p_end, float p_normalizedTime, AnimationEase p_ease)
	{
		switch(p_ease)
		{
		default:
		case AnimationEase.LINEAR:
			return Linear(p_start, p_end, p_normalizedTime);
		case AnimationEase.SPRING:
			return SpringEase(p_start, p_end, p_normalizedTime);
		case AnimationEase.QUAD_IN:
			return EaseInQuad(p_start, p_end, p_normalizedTime);
		case AnimationEase.QUAD_OUT:
			return EaseOutQuad(p_start, p_end, p_normalizedTime);
		case AnimationEase.QUAD_IN_OUT:
			return EaseInOutQuad(p_start, p_end, p_normalizedTime);
		case AnimationEase.CUBIC_IN:
			return EaseInCubic(p_start, p_end, p_normalizedTime);
		case AnimationEase.CUBIC_OUT:
			return EaseOutCubic(p_start, p_end, p_normalizedTime);
		case AnimationEase.CUBIC_IN_OUT:
			return EaseInOutCubic(p_start, p_end, p_normalizedTime);
		case AnimationEase.BOUNCE_IN:
			return EaseInBounce(p_start, p_end, p_normalizedTime);
		case AnimationEase.BOUNCE_OUT:
			return EaseOutBounce(p_start, p_end, p_normalizedTime);
		case AnimationEase.ELASTIC_IN:
			return EaseInElastic(p_start, p_end, p_normalizedTime);
		case AnimationEase.ELASTIC_OUT:
			return EaseOutElastic(p_start, p_end, p_normalizedTime);
		case AnimationEase.ELASTIC_IN_OUT:
			return EaseInOutElastic(p_start, p_end, p_normalizedTime);
		case AnimationEase.QUART_IN:
			return EaseInQuart(p_start, p_end, p_normalizedTime);
		case AnimationEase.QUART_OUT:
			return EaseOutQuart(p_start, p_end, p_normalizedTime);
		case AnimationEase.QUART_IN_OUT:
			return EaseInOutQuart(p_start, p_end, p_normalizedTime);
		case AnimationEase.QUINT_IN:
			return EaseInQuint(p_start, p_end, p_normalizedTime);
		case AnimationEase.QUINT_OUT:
			return EaseOutQuint(p_start, p_end, p_normalizedTime);
		case AnimationEase.QUINT_IN_OUT:
			return EaseInOutQuint(p_start, p_end, p_normalizedTime);
		case AnimationEase.SINE_IN:
			return EaseInSine(p_start, p_end, p_normalizedTime);
		case AnimationEase.SINE_OUT:
			return EaseOutSine(p_start, p_end, p_normalizedTime);
		case AnimationEase.SINE_IN_OUT:
			return EaseInOutSine(p_start, p_end, p_normalizedTime);
		case AnimationEase.EXPO_IN:
			return EaseInExpo(p_start, p_end, p_normalizedTime);
		case AnimationEase.EXPO_OUT:
			return EaseOutExpo(p_start, p_end, p_normalizedTime);
		case AnimationEase.EXPO_IN_OUT:
			return EaseInOutExpo(p_start, p_end, p_normalizedTime);
		case AnimationEase.CIRC_IN:
			return EaseInCirc(p_start, p_end, p_normalizedTime);
		case AnimationEase.CIRC_OUT:
			return EaseOutCirc(p_start, p_end, p_normalizedTime);
		case AnimationEase.CIRC_IN_OUT:
			return EaseInOutCirc(p_start, p_end, p_normalizedTime);
		case AnimationEase.BACK_IN:
			return EaseInBack(p_start, p_end, p_normalizedTime);
		case AnimationEase.BACK_OUT:
			return EaseOutBack(p_start, p_end, p_normalizedTime);
		case AnimationEase.BACK_IN_OUT:
			return EaseInOutBack(p_start, p_end, p_normalizedTime);
		}
	}
	#endregion
	
	#region Linear
	private static  float Linear(float p_start, float p_end, float p_normalizedTime)
	{
		float delta = p_end - p_start;
		return p_start + (delta * p_normalizedTime);
	}
	#endregion
	
	#region EaseInQuad
	private static  float EaseInQuad(float p_start, float p_final, float p_normalizedTime)
	{
		p_final -= p_start;
		return p_final * p_normalizedTime * p_normalizedTime + p_start;
	}
	#endregion
	
	#region EaseOutQuad
	private static  float EaseOutQuad(float p_start, float p_end, float p_normalizedTime)
	{
		p_end -= p_start;
		return -p_end * p_normalizedTime * (p_normalizedTime - 2) + p_start;
	}
	#endregion
	
	#region EaseInOutQuad
	private static  float EaseInOutQuad(float p_start, float p_end, float p_normalizedTime)
	{
		p_normalizedTime /= .5f;
		p_end -= p_start;
		if (p_normalizedTime < 1) return p_end / 2 * p_normalizedTime * p_normalizedTime + p_start;
		p_normalizedTime--;
		return -p_end / 2 * (p_normalizedTime * (p_normalizedTime - 2) - 1) + p_start;
	}
	#endregion
	
	#region SpringEase
	private static  float SpringEase(float p_start, float p_end, float p_normalizedTime)
	{
		p_normalizedTime = Mathf.Clamp01(p_normalizedTime);
		p_normalizedTime = (Mathf.Sin(p_normalizedTime * Mathf.PI * (0.2f + 2.5f * p_normalizedTime * p_normalizedTime * p_normalizedTime)) * Mathf.Pow(1f - p_normalizedTime, 2.2f) + p_normalizedTime) * (1f + (1.2f * (1f - p_normalizedTime)));
		return p_start + (p_end - p_start) * p_normalizedTime;
	}
	#endregion
	
	#region EaseInCubic
	private static float EaseInCubic(float p_start, float p_end, float p_normalizedTime)
	{
		p_end -= p_start;
		return p_end * p_normalizedTime * p_normalizedTime * p_normalizedTime + p_start;
	}
	#endregion
	
	#region EaseOutCubic
	private static float EaseOutCubic(float p_start, float p_end, float p_normalizedTime)
	{
		p_normalizedTime--;
		p_end -= p_start;
		return p_end * (p_normalizedTime * p_normalizedTime * p_normalizedTime + 1) + p_start;
	}
	#endregion
	
	#region EaseInOutCubic
	private static float EaseInOutCubic(float p_start, float p_end, float p_normalizedTime)
	{
		p_normalizedTime /= .5f;
		p_end -= p_start;
		if (p_normalizedTime < 1) return p_end / 2 * p_normalizedTime * p_normalizedTime * p_normalizedTime + p_start;
		p_normalizedTime -= 2;
		return p_end / 2 * (p_normalizedTime * p_normalizedTime * p_normalizedTime + 2) + p_start;
	}
	#endregion
	
	#region EaseInBounce
	private static float EaseInBounce(float p_start, float p_end, float p_normalizedTime)
	{
		p_end -= p_start;
		return p_end - EaseOutBounce(0, p_end, 1f-p_normalizedTime) + p_start;
	}
	#endregion
	
	#region EaseOutBounce
	private static float EaseOutBounce(float p_start, float p_end, float p_normalizedTime)
	{
		p_normalizedTime /= 1f;
		p_end -= p_start;
		if (p_normalizedTime < (1 / 2.75f))
		{
			return p_end * (7.5625f * p_normalizedTime * p_normalizedTime) + p_start;
		}
		else if (p_normalizedTime < (2 / 2.75f))
		{
			p_normalizedTime -= (1.5f / 2.75f);
			return p_end * (7.5625f * (p_normalizedTime) * p_normalizedTime + .75f) + p_start;
		}
		else if (p_normalizedTime < (2.5 / 2.75))
		{
			p_normalizedTime -= (2.25f / 2.75f);
			return p_end * (7.5625f * (p_normalizedTime) * p_normalizedTime + .9375f) + p_start;
		}
		else
		{
			p_normalizedTime -= (2.625f / 2.75f);
			return p_end * (7.5625f * (p_normalizedTime) * p_normalizedTime + .984375f) + p_start;
		}
	}
	#endregion
	
	#region EaseInElastic
	private static float EaseInElastic(float p_start, float p_end, float p_normalizedTime)
	{
		p_end -= p_start;
		
		float __temp1 = 1f;
		float __temp2 = __temp1 * .3f;
		float __temp3 = 0;
		float __temp4 = 0;
		
		if (p_normalizedTime == 0) 
			return p_start;
		
		if ((p_normalizedTime /= __temp1) == 1) 
			return p_start + p_end;
		
		if (__temp4 == 0f || __temp4 < Mathf.Abs(p_end))
		{
			__temp4 = p_end;
			__temp3 = __temp2 / 4;
		}
		else
		{
			__temp3 = __temp2 / (2 * Mathf.PI) * Mathf.Asin(p_end / __temp4);
		}
		
		return -(__temp4 * Mathf.Pow(2, 10 * (p_normalizedTime-=1)) * Mathf.Sin((p_normalizedTime * __temp1 - __temp3) * (2 * Mathf.PI) / __temp2)) + p_start;
	}		
	#endregion
	
	#region EaseOutElastic
	private static float EaseOutElastic(float p_start, float p_end, float p_normalizedTime)
	{
		p_end -= p_start;
		
		float __temp1 = 1f;
		float __temp2 = __temp1 * .3f;
		float __temp3 = 0;
		float __temp4 = 0;
		
		if (p_normalizedTime == 0) 
			return p_start;
		
		if ((p_normalizedTime /= __temp1) == 1) 
			return p_start + p_end;
		
		if (__temp4 == 0f || __temp4 < Mathf.Abs(p_end))
		{
			__temp4 = p_end;
			__temp3 = __temp2 / 4;
		}
		else
		{
			__temp3 = __temp2 / (2 * Mathf.PI) * Mathf.Asin(p_end / __temp4);
		}
		
		return (__temp4 * Mathf.Pow(2, -10 * p_normalizedTime) * Mathf.Sin((p_normalizedTime * __temp1 - __temp3) * (2 * Mathf.PI) / __temp2) + p_end + p_start);
	}		
	#endregion
	
	#region EaseInOutElastic
	private static float EaseInOutElastic(float p_start, float p_end, float p_normalizedTime)
	{
		p_end -= p_start;
		
		float __temp1 = 1f;
		float __temp2 = __temp1 * .3f;
		float __temp3 = 0;
		float __temp4 = 0;
		
		if (p_normalizedTime == 0) 
			return p_start;
		
		if ((p_normalizedTime /= __temp1/2) == 2) 
			return p_start + p_end;
		
		if (__temp4 == 0f || __temp4 < Mathf.Abs(p_end))
		{
			__temp4 = p_end;
			__temp3 = __temp2 / 4;
		}
		else
		{
			__temp3 = __temp2 / (2 * Mathf.PI) * Mathf.Asin(p_end / __temp4);
		}
		
		if (p_normalizedTime < 1) 
			return -0.5f * (__temp4 * Mathf.Pow(2, 10 * (p_normalizedTime-=1)) * Mathf.Sin((p_normalizedTime * __temp1 - __temp3) * (2 * Mathf.PI) / __temp2)) + p_start;
		
		return __temp4 * Mathf.Pow(2, -10 * (p_normalizedTime-=1)) * Mathf.Sin((p_normalizedTime * __temp1 - __temp3) * (2 * Mathf.PI) / __temp2) * 0.5f + p_end + p_start;
	}		
	#endregion
	
	#region EaseInQuart
	private static float EaseInQuart(float p_start, float p_end, float p_normalizedTime)
	{
		p_end -= p_start;
		return p_end * p_normalizedTime * p_normalizedTime * p_normalizedTime * p_normalizedTime + p_start;
	}
	#endregion
	
	#region EaseOutQuart
	private static float EaseOutQuart(float p_start, float p_end, float p_normalizedTime)
	{
		p_normalizedTime--;
		p_end -= p_start;
		return -p_end * (p_normalizedTime * p_normalizedTime * p_normalizedTime * p_normalizedTime - 1) + p_start;
	}
	#endregion
	
	#region EaseInOutQuart
	private static float EaseInOutQuart(float p_start, float p_end, float p_normalizedTime)
	{
		p_normalizedTime /= .5f;
		p_end -= p_start;
		if (p_normalizedTime < 1) 
			return p_end / 2 * p_normalizedTime * p_normalizedTime * p_normalizedTime * p_normalizedTime + p_start;
		p_normalizedTime -= 2;
		return -p_end / 2 * (p_normalizedTime * p_normalizedTime * p_normalizedTime * p_normalizedTime - 2) + p_start;
	}
	#endregion
	
	#region EaseInQuint
	private static float EaseInQuint(float p_start, float p_end, float p_normalizedTime)
	{
		p_end -= p_start;
		return p_end * p_normalizedTime * p_normalizedTime * p_normalizedTime * p_normalizedTime * p_normalizedTime + p_start;
	}
	#endregion
	
	#region EaseOutQuint
	private static float EaseOutQuint(float p_start, float p_end, float p_normalizedTime)
	{
		p_normalizedTime--;
		p_end -= p_start;
		return p_end * (p_normalizedTime * p_normalizedTime * p_normalizedTime * p_normalizedTime * p_normalizedTime + 1) + p_start;
	}
	#endregion
	
	#region EaseInOutQuint
	private static float EaseInOutQuint(float p_start, float p_end, float p_normalizedTime)
	{
		p_normalizedTime /= .5f;
		p_end -= p_start;
		if (p_normalizedTime < 1) 
			return p_end / 2 * p_normalizedTime * p_normalizedTime * p_normalizedTime * p_normalizedTime * p_normalizedTime + p_start;
		p_normalizedTime -= 2;
		return p_end / 2 * (p_normalizedTime * p_normalizedTime * p_normalizedTime * p_normalizedTime * p_normalizedTime + 2) + p_start;
	}
	#endregion
	
	#region EaseInSine
	private static float EaseInSine(float p_start, float p_end, float p_normalizedTime)
	{
		p_end -= p_start;
		return -p_end * Mathf.Cos(p_normalizedTime / 1 * (Mathf.PI / 2)) + p_end + p_start;
	}
	#endregion
	
	#region EaseOutSine
	private static float EaseOutSine(float p_start, float p_end, float p_normalizedTime)
	{
		p_end -= p_start;
		return p_end * Mathf.Sin(p_normalizedTime / 1 * (Mathf.PI / 2)) + p_start;
	}
	#endregion
	
	#region EaseInOutSine
	private static float EaseInOutSine(float p_start, float p_end, float p_normalizedTime)
	{
		p_end -= p_start;
		return -p_end / 2 * (Mathf.Cos(Mathf.PI * p_normalizedTime / 1) - 1) + p_start;
	}
	#endregion
	
	#region EaseInExpo
	private static float EaseInExpo(float p_start, float p_end, float p_normalizedTime)
	{
		p_end -= p_start;
		return p_end * Mathf.Pow(2, 10 * (p_normalizedTime / 1 - 1)) + p_start;
	}
	#endregion
	
	#region EaseOutExpo
	private static float EaseOutExpo(float p_start, float p_end, float p_normalizedTime)
	{
		p_end -= p_start;
		return p_end * (-Mathf.Pow(2, -10 * p_normalizedTime / 1) + 1) + p_start;
	}
	#endregion
	
	#region EaseInOutExpo
	private static float EaseInOutExpo(float p_start, float p_end, float p_normalizedTime)
	{
		p_normalizedTime /= .5f;
		p_end -= p_start;
		if (p_normalizedTime < 1) 
			return p_end / 2 * Mathf.Pow(2, 10 * (p_normalizedTime - 1)) + p_start;
		p_normalizedTime--;
		return p_end / 2 * (-Mathf.Pow(2, -10 * p_normalizedTime) + 2) + p_start;
	}
	#endregion
	
	#region EaseInCirc
	private static float EaseInCirc(float p_start, float p_end, float p_normalizedTime)
	{
		p_end -= p_start;
		return -p_end * (Mathf.Sqrt(1 - p_normalizedTime * p_normalizedTime) - 1) + p_start;
	}
	#endregion
	
	#region EaseOutCirc
	private static float EaseOutCirc(float p_start, float p_end, float p_normalizedTime)
	{
		p_normalizedTime--;
		p_end -= p_start;
		return p_end * Mathf.Sqrt(1 - p_normalizedTime * p_normalizedTime) + p_start;
	}
	#endregion
	
	#region EaseInOutCirc
	private static float EaseInOutCirc(float p_start, float p_end, float p_normalizedTime)
	{
		p_normalizedTime /= .5f;
		p_end -= p_start;
		if (p_normalizedTime < 1) 
			return -p_end / 2 * (Mathf.Sqrt(1 - p_normalizedTime * p_normalizedTime) - 1) + p_start;
		p_normalizedTime -= 2;
		return p_end / 2 * (Mathf.Sqrt(1 - p_normalizedTime * p_normalizedTime) + 1) + p_start;
	}
	#endregion
	
	#region EaseInBack
	private static float EaseInBack(float p_start, float p_end, float p_normalizedTime)
	{
		p_end -= p_start;
		p_normalizedTime /= 1;
		float s = 1.70158f;
		return p_end * (p_normalizedTime) * p_normalizedTime * ((s + 1) * p_normalizedTime - s) + p_start;
	}
	#endregion
	
	#region EaseOutBack
	private static float EaseOutBack(float p_start, float p_end, float p_normalizedTime)
	{
		float __temp = 1.70158f;
		p_end -= p_start;
		p_normalizedTime = (p_normalizedTime / 1) - 1;
		return p_end * ((p_normalizedTime) * p_normalizedTime * ((__temp + 1) * p_normalizedTime + __temp) + 1) + p_start;
	}
	#endregion
	
	#region EaseInOutBack
	private static float EaseInOutBack(float p_start, float p_end, float p_normalizedTime)
	{
		float __temp = 1.70158f;
		p_end -= p_start;
		p_normalizedTime /= .5f;
		if ((p_normalizedTime) < 1)
		{
			__temp *= (1.525f);
			return p_end / 2 * (p_normalizedTime * p_normalizedTime * (((__temp) + 1) * p_normalizedTime - __temp)) + p_start;
		}
		p_normalizedTime -= 2;
		__temp *= (1.525f);
		return p_end / 2 * ((p_normalizedTime) * p_normalizedTime * (((__temp) + 1) * p_normalizedTime + __temp) + 2) + p_start;
	}
	#endregion
}
