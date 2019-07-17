using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyFlow
{
    // Options automatically loaded from the "DependencyFlow" section of appsettings.json
    public class DependencyFlowOptions
    {
        public Thresholds Thresholds { get; set; }
    }
    
    public class Thresholds
    {
        public int RecentDays { get; set; }
        public int OutOfDateDays { get; set; }
        public int RecentCommits { get; set; }
    }
}
