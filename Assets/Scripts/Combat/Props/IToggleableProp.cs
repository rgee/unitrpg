using System.Collections;

namespace Combat.Props {
    public interface IToggleableProp {
        IEnumerator Enable();
        IEnumerator Disable();
    }
}