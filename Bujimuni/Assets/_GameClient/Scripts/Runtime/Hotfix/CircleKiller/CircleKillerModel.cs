using System.Collections;
using System.Collections.Generic;
using Lighten;
using UnityEngine;

namespace CircleKiller
{
    public class CircleKillerModel : AbstractModel
    {
        public RxProperty<int> Score { get; private set; } = new RxProperty<int>();
        public bool IsRunning { get; set; }
    }
}

