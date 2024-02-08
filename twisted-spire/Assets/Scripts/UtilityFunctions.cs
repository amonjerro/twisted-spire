using UnityEngine;

public static class UtilityFunctions
{
    public static Vector3 RotateClockwise(Vector3 position, float theta)
    {
        float rotationAngleInRadians = Mathf.Deg2Rad*theta;
        float newPositionX = position.x * Mathf.Cos(rotationAngleInRadians) + position.z * Mathf.Sin(rotationAngleInRadians);
        float newPositionZ = -position.x * Mathf.Sin(rotationAngleInRadians) + position.z * Mathf.Cos(rotationAngleInRadians);
        return new Vector3(newPositionX, position.y, newPositionZ);
    }

    public static Vector3 RotateCounterClockwise(Vector3 position, float theta)
    {
        float rotationAngleInRadians = Mathf.Deg2Rad * theta;
        float newPositionX = position.x * Mathf.Cos(rotationAngleInRadians) - position.z * Mathf.Sin(rotationAngleInRadians);
        float newPositionZ = position.x * Mathf.Sin(rotationAngleInRadians) + position.z * Mathf.Cos(rotationAngleInRadians);
        return new Vector3(newPositionX, position.y, newPositionZ);
    }
}