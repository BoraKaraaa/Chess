namespace Munkur
{
    public static class MunkurMath
    {
        public static int ClampTo1(int value)
        {
            if (value > 0)
            {
                return 1;
            }
            else if (value == 0)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        public static int Modulo(int value, int mod)
        {
            if (value >= 0)
            {
                return value % mod;
            }
            
            return value % mod + mod;
        }

        public static float Normalize(float current, float oldMin, float oldMax, float newMin, float newMax)
        {
            return (current - oldMin) / (oldMax - oldMin) * (newMax - newMin) + newMin;
        }
    }
}
