using Contexts.Battle.Models;
using Contexts.Battle.Utilities;
using Contexts.Battle.Views;
using strange.extensions.command.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Contexts.Battle.Commands {
    public class ShowContextMenuCommand : Command {
        [Inject]
        public Vector2 Position { get; set; }

        [Inject]
        public BattleViewState ViewState { get; set; }

        public override void Execute() {
        }
    }
}