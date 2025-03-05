using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatGame.Models
{
    namespace CatGame.Models
    {
        public class BadFood : Food
        {
            public int Penalty { get; set; } = 1; // Штраф за сбор плохой еды
        }
    }
}
