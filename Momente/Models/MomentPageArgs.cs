using Momente.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Momente.Models
{
    public class MomentPageArgs
    {
        public MomentPageArgs() { }

        public MomentPageArgs(Moment moment, MomentAction action = MomentAction.None)
        {
            Moment = moment;
            Action = action;
        }

        public Moment Moment { get; set; } = new();

        public MomentAction Action { get; set; } = MomentAction.None;

    }
}
