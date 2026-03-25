using UnityEngine;

public static class GPSUtils
{
    public static float GetDistance(double lat1, double lon1, double lat2, double lon2)
    {
        float R = 6371000f; // Earth radius (meters)

        float dLat = Mathf.Deg2Rad * (float)(lat2 - lat1);
        float dLon = Mathf.Deg2Rad * (float)(lon2 - lon1);

        float a =
            Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
            Mathf.Cos(Mathf.Deg2Rad * (float)lat1) *
            Mathf.Cos(Mathf.Deg2Rad * (float)lat2) *
            Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);

        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));

        return R * c;
    }
}