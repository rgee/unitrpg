using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Utilities;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class ContextMenuViewMediator : Mediator {
        [Inject]
        public ContextMenuView View { get; set; }

        [Inject]
        public ContextRequestedSignal ContextRequestedSignal { get; set; }

        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public PlayerTurnCompleteSignal PlayerTurnCompleteSignal { get; set; }

        public override void OnRegister() {
            View.DismissSignal.AddListener(View.Hide);
            View.ItemSelectedSignal.AddListener(OnItemSelected);
            ContextRequestedSignal.AddListener(ShowMenu);
        }

        public void OnItemSelected(string item) {
            if (item == "End Turn") {
               PlayerTurnCompleteSignal.Dispatch(); 
            }
        }

        public void ShowMenu(Vector2 position) {
            var dimensions = Model.Dimensions;
            var worldPosition = dimensions.GetWorldPositionForGridPosition(position);
            var config = BubbleMenuTemplates.GetContextMenuTemplate();
            View.transform.position = worldPosition;

            View.Show(config);
        }
    }
}