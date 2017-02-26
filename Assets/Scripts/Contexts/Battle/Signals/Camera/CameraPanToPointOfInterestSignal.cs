using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contexts.Battle.Models;
using strange.extensions.signal.impl;

namespace Contexts.Battle.Signals.Camera {
    public class CameraPanToPointOfInterestSignal : Signal<IPointOfInterest> {
    }
}
