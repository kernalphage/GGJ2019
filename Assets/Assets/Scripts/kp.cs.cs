using UnityEngine;
using System.Collections.Generic;


public class kp
{
	public static float RangeMap(float t, float start, float stop, 
		float outStart, float outStop, bool doClamp = false)  {
		t = t - start;
		t = t / (stop - start);
		if (doClamp) {
			t = Mathf.Clamp01(t);
		}
		return Mathf.Lerp (outStart, outStop, t);
	}

	public static Vector2 RangeMap(Vector2 t, Vector2 start, Vector2 stop, 
		Vector2 outStart, Vector2 outStop, bool doClamp = false) {
		t = t - start;
		t = t / (stop - start);
		if (doClamp) {
			t.x = Mathf.Clamp01(t.x);
			t.y = Mathf.Clamp01(t.y);
		}
		float x =  Mathf.Lerp (outStart.x, outStop.x, t.x);
		float y =  Mathf.Lerp (outStart.y, outStop.y, t.y);
		return new Vector2(x,y);
	}


	/* Screen to world utils */
	public static Vector2 RandomScreenPoint( Rect inset = new Rect() )
	{
		Vector2 topRight = ScreenTopRight();
			topRight.x += inset.x;
			topRight.y += inset.y;
		Vector2 bottomLeft = ScreenBottomLeft();
			bottomLeft.x -= inset.width;
			bottomLeft.y -= inset.height;

		float ex = Random.Range(topRight.x, bottomLeft.x);
		float wy = Random.Range(topRight.y, bottomLeft.y);
		return new Vector2(ex,wy);
	}
	public static Vector2 MapToScreen(Vector2 input)
	{
		Vector2 topRight = ScreenTopRight();
		Vector2 bottomLeft = ScreenBottomLeft();
		input = RangeMap(input, new Vector2(0,0), new Vector2(1,1), bottomLeft, topRight);
		return input;
	}
	public static Vector2 ClampToScreen(Vector2 input)
	{
		Vector2 topRight = ScreenTopRight();
		Vector2 bottomLeft = ScreenBottomLeft();
		input.x = Mathf.Clamp(input.x, bottomLeft.x, topRight.x);
		input.y = Mathf.Clamp(input.y, bottomLeft.y, topRight.y);
		return input;
	}
	public static Vector2 ScreenBottomLeft()
	{
		Vector2 dim = new Vector3(0,0,Camera.main.farClipPlane);
		return Camera.main.ViewportToWorldPoint(dim);
		return dim;
	}
	public static Vector2 ScreenTopRight()
	{
		Vector2 dim = new Vector3(1,1,Camera.main.farClipPlane);
		return Camera.main.ViewportToWorldPoint(dim);
		return dim;
	}


// Light debugging utils
	public static void Assert( bool condition, object throwable = null)
	{
		if(!condition)
		{
			if(throwable == null) throwable = "err";
			Debug.LogError(throwable.ToString());
		}
	}
	public static void Suggest( bool condition, object throwable = null)
	{
		if(!condition)
		{
			if(throwable == null) throwable = "warn";
			Debug.LogWarning(throwable.ToString());
		}
	}
}

public static class IListExtensions {
	/// <summary>
	/// Shuffles the element order of the specified list.
	/// </summary>
	public static void Shuffle<T>(this IList<T> ts) {
		var count = ts.Count;
		var last = count - 1;
		for (var i = 0; i < last; ++i) {
			var r = UnityEngine.Random.Range(i, count);
			var tmp = ts[i];
			ts[i] = ts[r];
			ts[r] = tmp;
		}
	}
    public static T RandomSample<T>(this IList<T> ts)
    {
        var count = ts.Count;
        int sample = Mathf.FloorToInt(UnityEngine.Random.value * count);
        return ts[sample % count]; // bugs ech 
    }

    public static T RandomSample<T>(this IList<T> ts, System.Func<T, float> del ) {

		var count = ts.Count;
		var last = count - 1;
		float sum = 0;
		for (var i = 0; i < count; ++i) {
			sum += del (ts [i]);
		}
		var sample = UnityEngine.Random.value * sum;
		for (var i = 0; i < last; ++i) {
			sample -= del (ts [i]);
			if (sample <= 0) {
				return ts [i];
			}
		}
		return ts [last % count];
	}
}
