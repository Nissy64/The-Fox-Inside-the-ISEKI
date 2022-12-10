namespace Managers
{
    public static class FolderManager
    {
        public enum GizmosFiles
        {
            Position0,
            Position1,
            Position2,
            Position3,
            Position4,
            Position5,
            Position6,
            Position7,
            Position8,
            Position9,
            Position10
        }

        public static string GetGizmosFiles(GizmosFiles gizmosFiles)
        {
            string[] planetNames =
            {
                "Position 0",
                "Position 1",
                "Position 2",
                "Position 3",
                "Position 4",
                "Position 5",
                "Position 6",
                "Position 7",
                "Position 8",
                "Position 9",
                "Position 10"
            };

            return planetNames[(int)gizmosFiles];
        }
    }
}