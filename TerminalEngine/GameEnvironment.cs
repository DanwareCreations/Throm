using System;
using System.Reflection;

namespace TerminalEngine {

    public class GameEnvironment {
        public Assembly Assembly { get; set; } = Assembly.GetExecutingAssembly();
        public string Environment { get; set; } = "Development";

        public bool IsDevelopment() => Environment.Equals("Development", StringComparison.InvariantCultureIgnoreCase);
        public bool IsStaging() => Environment.Equals("Staging", StringComparison.InvariantCultureIgnoreCase);
        public bool IsProduction() => Environment.Equals("Production", StringComparison.InvariantCultureIgnoreCase);
    }

}
