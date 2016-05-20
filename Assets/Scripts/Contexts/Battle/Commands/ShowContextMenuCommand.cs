using Contexts.Battle.Models;
using Contexts.Battle.Utilities;
using Contexts.Battle.Views;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Battle.Commands {
    public class ShowContextMenuCommand : Command {
        [Inject]
        public Vector2 Position { get; set; }

        [Inject]
        public BattleViewState ViewState { get; set; }

        public override void Execute() {
            var dimensions = ViewState.Dimensions;
            var worldPosition = dimensions.GetWorldPositionForGridPosition(Position);

            var contextMenuView = new GameObject("context_menu");
            contextMenuView.transform.position = worldPosition;

            var view = contextMenuView.AddComponent<BubbleMenuView>();
            view.Show(BubbleMenuTemplates.GetContextMenuTemplate());
        }
    }
}