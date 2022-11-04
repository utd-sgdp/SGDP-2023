using UnityEngine;

namespace Game.Utility
{
    public static class Math
    {
        public static float LambertW(float x)
        {
            if (x < -Mathf.Exp(-1))
            {
                throw new System.Exception("The LambertW-function is not defined for " + x + ".");
            }

            int amountOfIterations = Mathf.Max(4, (int)Mathf.Ceil(Mathf.Log10(x) / 3));
            float w = 3 * Mathf.Log(x + 1) / 4;

            for (int i = 0; i < amountOfIterations; i++)
            {
                w -= (w * Mathf.Exp(w) - x) / (Mathf.Exp(w) * (w + 1) - (w + 2) * (w * Mathf.Exp(w) - x) / (2 * w + 2));
            }

            return w;
        }
    }
}