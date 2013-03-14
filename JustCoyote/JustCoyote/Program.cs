using System;

namespace JustCoyote
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (JustCoyote game = new JustCoyote())
            {
                game.Run();
            }
        }
    }
#endif
}

