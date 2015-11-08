using strange.extensions.command.impl;
using UnityEditor;

namespace Contexts.Base.Commands {
    public class QuitGameCommand : Command {
        public override void Execute() {
            if (UnityEngine.Application.isEditor) {
                EditorApplication.isPlaying = false;
            } else {
                UnityEngine.Application.Quit();
            }
        }
    }
}