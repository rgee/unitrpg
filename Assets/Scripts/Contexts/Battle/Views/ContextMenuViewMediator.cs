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
        public ShowContextMenuSignal ShowContextMenuSignal { get; set; }


        [Inject]
        public EndTurnSignal EndTurnSignal { get; set; }
        
        [Inject]
        public ExitContextMenuSignal ExitContextMenuSignal { get; set; }

        [Inject]
        public BackSignal BackSignal { get; set; }

        public override void OnRegister() {
            BackSignal.AddListener(OnDismiss);
            View.DismissSignal.AddListener(OnDismiss);
            View.ItemSelectedSignal.AddListener(OnItemSelected);
            ShowContextMenuSignal.AddListener(ShowMenu);
        }

        public void OnDismiss() {
            View.Hide();
            ExitContextMenuSignal.Dispatch();
        }

        public void OnItemSelected(string item) {
            if (item == "End Turn") {
               EndTurnSignal.Dispatch(); 
            }
            View.Hide();
        }

        public void ShowMenu(Vector3 position) {
            var config = BubbleMenuTemplates.GetContextMenuTemplate();
            View.transform.position = position;

            View.Show(config);
        }
    }
}