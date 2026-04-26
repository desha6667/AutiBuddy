namespace AutiBuddy.Helpers
{
    public class LevenshteinHelper
    {
        public static int CalculateDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s))
                return t.Length;
            if (string.IsNullOrEmpty(t))
                return s.Length;

            int[,] d = new int[s.Length + 1, t.Length + 1];

            for (int i = 0; i <= s.Length; i++)
                d[i, 0] = i;
            for (int j = 0; j <= t.Length; j++)
                d[0, j] = j;

            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = s[i - 1] == t[j - 1] ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost
                    );
                }
            }

            return d[s.Length, t.Length];
        }

        public static double CalculateSimilarity(string input, string target)
        {
            input = input.ToLower().Trim();
            target = target.ToLower().Trim();

            int distance = CalculateDistance(input, target);
            int maxLength = Math.Max(input.Length, target.Length);

            if (maxLength == 0) return 100;

            return (1.0 - (double)distance / maxLength) * 100;
        }
    }
}
