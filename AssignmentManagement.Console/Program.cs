using AssignmentManagement.Core.Services;
using AssignmentManagement.Core.Interfaces;
using AssignmentManagement.UI;
using AssignmentManagement.Core;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace AssignmentManagement.Console
{
    internal class Program
    {
        public static void Main(string[] args) // 🔥 Static Main method — this is required
        {
            var services = new ServiceCollection();
                       
            services.AddSingleton<IAssignmentService, AssignmentService>();
            services.AddSingleton<ConsoleUI>();
            services.AddSingleton<IAssignmentFormatter, AssignmentFormatter>();
            services.AddSingleton<IAppLogger, ConsoleAppLogger>();

            var serviceProvider = services.BuildServiceProvider();
            var consoleUI = serviceProvider.GetRequiredService<ConsoleUI>();

            consoleUI.Run();
        }
    }
}
