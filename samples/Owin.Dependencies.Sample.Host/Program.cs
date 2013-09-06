using Microsoft.Owin.Hosting;
using System;

namespace Owin.Dependencies.Sample.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:5000"))
            {
                Console.WriteLine("Started on http://localhost:5000");
                Console.ReadLine();
                Console.WriteLine("Stopping...");
            }
        }
    }
}