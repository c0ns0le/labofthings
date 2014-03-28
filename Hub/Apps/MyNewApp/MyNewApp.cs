using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeOS.Hub.Common;
using HomeOS.Hub.Platform.Views;

namespace MyNewApp
{
   [System.AddIn.AddIn("HomeOS.Hub.Apps.My.New.App")]
    public class MyNewApp : ModuleBase
    {
        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public override void PortRegistered(VPort port)
        {
            throw new NotImplementedException();
        }

        public override void PortDeregistered(VPort port)
        {
            throw new NotImplementedException();
        }
    }
}
