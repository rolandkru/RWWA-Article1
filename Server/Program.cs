// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace RWWA_Article1.Server
{
    using System;

    using Microsoft.Owin.Hosting;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main.
        /// </summary>
        private static void Main()
        {
            // Start OWIN host
            WebApp.Start<Startup>(url: "http://localhost:10281/");
            
            Console.WriteLine("Hello, I'm ready when you are.");
            Console.ReadLine();
        }
    }
}