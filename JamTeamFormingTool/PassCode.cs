namespace JamTeamFormingTool
{
    public static class PassCode
    {
        private static Random rand = new Random();
        public static string Generate()
        {
            return new string(new char[] {
                (char)(65 + rand.Next(26)), // letter
                (char)(48 + rand.Next(10)), // number
                (char)(65 + rand.Next(26)), // letter
                (char)(48 + rand.Next(10)) // number
            });
        }
    }
}