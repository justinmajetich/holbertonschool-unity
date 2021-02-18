using UnityEngine;

/// <summary>
/// Provides a number of miscellaneous utility methods.
/// </summary>
public class Utility
{
    /// <summary>
    /// Performs the inverse of Mathf.Clamp. If the given value falls between the min and max thresholds,
    /// it's equated to the nearest threshold; otherwise, it's returned unchanged.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The threshold to which the value must be lesser.</param>
    /// <param name="max">The threshold to which the value must be greater.</param>
    /// <returns>
    /// The value of the nearest threshold to given value,
    /// or the given value unchanged if outside thresholds.
    /// </returns>
    static public float InverseClamp(float value, float min, float max)
    {
        // If value falls between the two outward facing thresholds.
        if (value > min && value < max)
        {
            // Get mean of min and max.
            float thresholdMean = (min + max) / 2;
            
            // Determine if value falls closer to min or max, and equate accordingly.
            if (value > thresholdMean)
                value = max;
            else
                value = min;
        }

        return value;
    }

    /// <summary>
    /// Calculates a random point within a circle set on the X and Z axes.
    /// </summary>
    /// <param name="diameter">A scalar to define the size of the circle being generated.</param>
    /// <returns>Returns a Vector3 with randomized X and Z values and a Y value of 0.</returns>
    static public Vector3 randomInsideXZCircle(float diameter)
    {
        // Generate random point in sphere scaled by given diameter.
        Vector3 randomPos = Random.insideUnitSphere * diameter;

        // Zero out random positions Y value.
        randomPos = new Vector3(randomPos.x, 0f, randomPos.z);

        return randomPos;
    }
}
