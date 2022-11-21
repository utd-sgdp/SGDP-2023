using UnityEngine;

namespace Game
{
    public static class QuaternionExtensions
    {
        public static Quaternion Randomize(this Quaternion rotation, Vector3 amount)
        {
            Vector3 random = amount;
            
            if (random.x > 0) random.x = Random.Range(-random.x, random.x);
            if (random.y > 0) random.y = Random.Range(-random.y, random.y);
            if (random.z > 0) random.z = Random.Range(-random.z, random.z);
            
            return rotation * Quaternion.Euler(random);
        }
    }
}