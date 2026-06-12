using UnityEngine;

public static class Mathfx
{
	// ================ Ease in and out ================
	
	public static float Hermite(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));
	}

	public static Vector2 Hermite(Vector2 start, Vector2 end, float value)
	{
		return new Vector2(Hermite(start.x, end.x, value), Hermite(start.y, end.y, value));
	}

	public static Vector3 Hermite(Vector3 start, Vector3 end, float value)
	{
		return new Vector3(Hermite(start.x, end.x, value), Hermite(start.y, end.y, value), Hermite(start.z, end.z, value));
	}

	public static Quaternion Hermite(Quaternion start, Quaternion end, float value)
	{
		return Quaternion.Slerp(start, end, Hermite(0f, 1f, value));
	}
	
	public static float SmoothStep(float x, float min, float max)
	{
		x = Mathf.Clamp(x, min, max);
		float v1 = (x - min) / (max - min);
		float v2 = (x - min) / (max - min);
		return -2 * v1 * v1 * v1 + 3 * v2 * v2;
	}

	public static Vector2 SmoothStep(Vector2 vec, float min, float max)
	{
		return new Vector2(SmoothStep(vec.x, min, max), SmoothStep(vec.y, min, max));
	}

	public static Vector3 SmoothStep(Vector3 vec, float min, float max)
	{
		return new Vector3(SmoothStep(vec.x, min, max), SmoothStep(vec.y, min, max), SmoothStep(vec.z, min, max));
	}
	
	// ================ Ease out ================
	
	public static float Sinerp(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
	}

	public static Vector2 Sinerp(Vector2 start, Vector2 end, float value)
	{
		return new Vector2(Mathf.Lerp(start.x, end.x, Mathf.Sin(value * Mathf.PI * 0.5f)),
			Mathf.Lerp(start.y, end.y, Mathf.Sin(value * Mathf.PI * 0.5f)));
	}

	public static Vector3 Sinerp(Vector3 start, Vector3 end, float value)
	{
		return new Vector3(Mathf.Lerp(start.x, end.x, Mathf.Sin(value * Mathf.PI * 0.5f)),
			Mathf.Lerp(start.y, end.y, Mathf.Sin(value * Mathf.PI * 0.5f)),
			Mathf.Lerp(start.z, end.z, Mathf.Sin(value * Mathf.PI * 0.5f)));
	}
	
	public static Quaternion Sinerp(Quaternion start, Quaternion end, float value)
	{
		return Quaternion.Slerp(start, end, Sinerp(0f, 1f, value));
	}

	// ================ Ease in ================

	public static float Coserp(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));
	}

	public static Vector2 Coserp(Vector2 start, Vector2 end, float value)
	{
		return new Vector2(Coserp(start.x, end.x, value), Coserp(start.y, end.y, value));
	}

	public static Vector3 Coserp(Vector3 start, Vector3 end, float value)
	{
		return new Vector3(Coserp(start.x, end.x, value), Coserp(start.y, end.y, value),
			Coserp(start.z, end.z, value));
	}
	
	public static Quaternion Coserp(Quaternion start, Quaternion end, float value)
	{
		return Quaternion.Slerp(start, end, Coserp(0f, 1f, value));
	}

	// ================ Boing ================
	
	public static float Berp(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) +
		         value) * (1f + (1.2f * (1f - value)));
		return start + (end - start) * value;
	}

	public static Vector2 Berp(Vector2 start, Vector2 end, float value)
	{
		return new Vector2(Berp(start.x, end.x, value), Berp(start.y, end.y, value));
	}

	public static Vector3 Berp(Vector3 start, Vector3 end, float value)
	{
		return new Vector3(Berp(start.x, end.x, value), Berp(start.y, end.y, value), Berp(start.z, end.z, value));
	}
	
	public static Quaternion Berp(Quaternion start, Quaternion end, float value)
	{
		return Quaternion.Slerp(start, end, Berp(0f, 1f, value));
	}

	// ================ Bounce ================
	
	public static float Bounce(float x)
	{
		return Mathf.Abs(Mathf.Sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));
	}

	public static Vector2 Bounce(Vector2 vec)
	{
		return new Vector2(Bounce(vec.x), Bounce(vec.y));
	}

	public static Vector3 Bounce(Vector3 vec)
	{
		return new Vector3(Bounce(vec.x), Bounce(vec.y), Bounce(vec.z));
	}
	
	// ================ Angle ================
	
	/*
	 * Angular Lerp - is like lerp but handles the wraparound from 0 to 360.
	 * This is useful when interpolating eulerAngles and the object
	 * crosses the 0/360 boundary.  The standard Lerp function causes the object
	 * to rotate in the wrong direction and looks stupid. Clerp fixes that.
	 */
	public static float AngularLerp(float start, float end, float value)
	{
		const float min = 0.0f;
		const float max = 360.0f;
		float half = Mathf.Abs((max - min) / 2.0f); //half the distance between min and max

		if (end - start < - half)
		{
			return start + (max - start + end) * value;
		}
		else if (end - start > half)
		{
			return start - (max - end + start) * value;
		}
		else
		{
			return start + (end - start) * value;
		}
	}

	// ================ Custom ================
	
	public static float CustomCurve(AnimationCurve anim, float value)
	{
		return anim.Evaluate(value);
	}
	
	// ================ Matching ================

	public static bool LinearMatch(float target, ref float current, float fullRange, float duration, float deltaTime)
	{
		if (Mathf.Approximately(current, target))
		{
			current = target;
			return true;
		}

		float speed = duration == 0f ? float.MaxValue : (fullRange / duration);
		if (current < target)
		{
			current = Mathf.Min(current + speed * deltaTime, target);
		}
		else if (current > target)
		{
			current = Mathf.Max(current - speed * deltaTime, target);
		}
		return false;
	}
}