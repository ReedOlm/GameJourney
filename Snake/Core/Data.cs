using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Core
{
    public static class Data
    {
        // User (target) screen resolution, this will be what our assets are scaled to
        public static int TargetW { get; set; } = 852;
        public static int TargetH { get; set; } = 480;

        public static bool UpdateResolution = false;

        // These are the resolutions that the game needs to render to.
        public static int ScreenW { get; set; } = 852;
        public static int ScreenH { get; set; } = 480;

        // Game info
        public static bool Exit { get; set; } = false;
        public static float Scale { get; set; } = 0.44444f;

        // Scenes, add to enum to add scenes
        public enum Scenes { Menu, Game, Settings, Reset }
        public static Scenes CurrentState { get; set; } = Scenes.Menu; // Sets starting screen

        public const string PATH = "savedData.json";

        public static int SessionScore = 0;
    }
}
