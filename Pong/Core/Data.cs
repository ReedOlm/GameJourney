using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Core
{
    public static class Data
    {
        // Running screen resolution and game state info
        public static int ScreenW { get; set; } = 1280;
        public static int ScreenH { get; set; } = 720;
        public static bool Exit { get; set; } = false;
        public static bool HasStarted { get; set; } = false;
        public static float Scale { get; set; } = 0.44444f;

        // Scenes, add to enum to add scenes
        public enum Scenes { Menu, Game, Settings, PauseMenu }
        public static Scenes CurrentState { get; set; } = Scenes.Menu; // Sets starting screen

        public const string PATH = "savedData.json";
    }
}
