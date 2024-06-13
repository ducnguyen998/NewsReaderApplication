using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsReaderSystem.Models
{
    public class FaceMousingResult
    {
        public int x { get; set; }
        public int y { get; set; }
        public int state { get; set; }

        //

        public override string ToString()
        {
            return $"x={x}, y={y}, state={state}";
        }
    }
}
