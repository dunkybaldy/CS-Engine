using System;

namespace ExampleGame.Options
{
    public class GeneralSettings
    {
        public int DesiredFramerate { get; set; }
        public bool FpsCounterOn { get; set; }
        public int MasterVolume { get; set; }
        public TimeSpan AutoSaveInterval { get; set; }
    }
}
