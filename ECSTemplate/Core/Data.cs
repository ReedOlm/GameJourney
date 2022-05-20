using System;
using System.Collections.Generic;
using System.Text;

namespace ECSTemplate.Core
{
    public static class Data
    {
        // These are the resolutions that the game needs to render to.
        public static int ScreenW { get; set; } = 1920; // Change these to acommodate our assets
        public static int ScreenH { get; set; } = 1080;
        public static bool Exit { get; set; } = false;
        public static float Scale { get; set; } = 0.44444f;

        // User (target) screen resolution, this will be what our assets are scaled to
        public static int TargetW { get; set; } = 1920; // Change these to change window size
        public static int TargetH { get; set; } = 1080; // Change these to change window size

        // Scenes, add to enum to add scenes
        public enum Scenes { Menu, Game, Settings}
        public static Scenes CurrentState { get; set; } = Scenes.Menu; // Sets starting screen

        public const string PATH = "savedData.json";
    }
}
