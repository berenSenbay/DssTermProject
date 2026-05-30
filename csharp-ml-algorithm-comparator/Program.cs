using System;
using System.Windows;

namespace csharp_ml_algorithm_comparator
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var app = new Application();
            app.Run(new MainWindow());
        }
    }
}
