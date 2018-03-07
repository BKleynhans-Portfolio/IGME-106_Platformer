using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    abstract class Environment : GameObject
    {
        public override void Move()
        {
        }
        public override void Die()//doesn't do anything, just here so there's no error message
        {
        }
    }
}
