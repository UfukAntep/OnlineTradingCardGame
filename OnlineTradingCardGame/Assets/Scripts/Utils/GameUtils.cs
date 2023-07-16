using UnityEngine;

public static class GameUtils
{
    #region Game Stop and Resume Functions

    public static void GameResume() =>
        Time.timeScale = 1f;

    public static void GameStop() =>
        Time.timeScale = 0f;

    public static bool IsGameStillPlay() => Time.timeScale != 0f;

    #endregion

    public static void LookAtY(this Transform transform, Transform target)
    {
        transform.rotation = LookRotationY(transform.position, target.position);
    }

    public static Quaternion LookRotationY(Vector3 pos, Vector3 target)
    {
        Vector3 rotVector = target - pos;
        rotVector.y = 0f;
        return Quaternion.LookRotation(rotVector);
    }
}