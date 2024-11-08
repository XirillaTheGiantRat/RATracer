namespace INFOGR2024Template
{
    class MyApplication
    {
        // member variables
        public Scene scene;
        public Surface screen;
        // constructor
        public MyApplication(Surface screen, Scene scene)
        {
            this.screen = screen;
            this.scene = scene;
        }
        // initialize
        public void Init()
        {

        }
        // tick: renders one frame
        public void Tick()
        {
            screen.Clear(0);

            screen.Render(scene);
        }
    }
}