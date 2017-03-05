

using Assets.Contexts.Base;
using Contexts.Base.Signals;
using Contexts.Battle.Commands;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Signals.Camera;
using Contexts.Battle.Signals.Public;
using Contexts.Battle.Utilities;
using Contexts.Battle.Views;
using Contexts.Battle.Views.UniqueCombatants;
using Contexts.Global.Signals;
using Models.Fighting.Maps.Configuration;
using Models.SaveGames;
using strange.extensions.command.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Contexts.Battle {
    public class BattleContext : BaseContext {
        public BattleContext(MonoBehaviour view) : base(view) {

        }

        protected override void mapBindings() {
            base.mapBindings();

            ICommandBinding startBinding;
            if (this == Context.firstContext) {
                startBinding = commandBinder.GetBinding<StartSignal>();
            } else {
                startBinding = commandBinder.Bind<ScreenRevealedSignal>();
            }

            if (this == Context.firstContext) {
                injectionBinder.Unbind<ISaveGameRepository>();
                injectionBinder.Bind<ISaveGameRepository>().ToValue(new TestingSaveGameRepository());
            }

            startBinding.To<PlayIntroCutsceneCommand>().InSequence();

            injectionBinder.Bind<BattleViewState>().ToSingleton().CrossContext();
            injectionBinder.Bind<BattleEventRegistry>().ToSingleton().CrossContext();

            injectionBinder.Bind<BattleStartSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<SpawnCombatantSignal>().ToSingleton().CrossContext();

            injectionBinder.Bind<HoverTileEnableSignal>().ToSingleton();
            injectionBinder.Bind<HoveredTileChangeSignal>().ToSingleton();
            injectionBinder.Bind<GatherBattleFromEditorSignal>().ToSingleton();
            injectionBinder.Bind<InitializeMapSignal>().ToSingleton();
            injectionBinder.Bind<MapPositionClickedSignal>().ToSingleton();
            injectionBinder.Bind<UnitSelectedSignal>().ToSingleton();
            injectionBinder.Bind<UnitDeselectedSignal>().ToSingleton();
            injectionBinder.Bind<HoverTileDisableSignal>().ToSingleton();
            injectionBinder.Bind<MoveCombatantSignal>().ToSingleton();
            injectionBinder.Bind<BackSignal>().ToSingleton();
            injectionBinder.Bind<CombatActionSelectedSignal>().ToSingleton();
            injectionBinder.Bind<NewMapHighlightSignal>().ToSingleton();
            injectionBinder.Bind<ClearHighlightSignal>().ToSingleton();
            injectionBinder.Bind<MovementPathReadySignal>().ToSingleton();
            injectionBinder.Bind<ActionCompleteSignal>().ToSingleton();
            injectionBinder.Bind<MovementPathUnavailableSignal>().ToSingleton();
            injectionBinder.Bind<StateTransitionSignal>().ToSingleton();
            injectionBinder.Bind<ActionAnimationCompleteSignal>().ToSingleton();
            injectionBinder.Bind<NewFightForecastSignal>().ToSingleton();
            injectionBinder.Bind<FightForecastDisableSignal>().ToSingleton();
            injectionBinder.Bind<FightConfirmedSignal>().ToSingleton();
            injectionBinder.Bind<AttackConnectedSignal>().ToSingleton();
            injectionBinder.Bind<IntroCutsceneStartSignal>().ToSingleton();
            injectionBinder.Bind<IntroCutsceneCompleteSignal>().ToSingleton();
            injectionBinder.Bind<EnemyTurnCompleteSignal>().ToSingleton();
            injectionBinder.Bind<PhaseChangeCompleteSignal>().ToSingleton();
            injectionBinder.Bind<PhaseChangeStartSignal>().ToSingleton();
            injectionBinder.Bind<NextBattleSignal>().ToSingleton();
            injectionBinder.Bind<ContextRequestedSignal>().ToSingleton();
            injectionBinder.Bind<EndTurnSignal>().ToSingleton();
            injectionBinder.Bind<ShowContextMenuSignal>().ToSingleton();
            injectionBinder.Bind<ExitContextMenuSignal>().ToSingleton();
            injectionBinder.Bind<CameraLockSignal>().ToSingleton();
            injectionBinder.Bind<CameraUnlockSignal>().ToSingleton();
            injectionBinder.Bind<IncrememtTurnSignal>().ToSingleton();
            injectionBinder.Bind<ProcessTileEventsSignal>().ToSingleton();
            injectionBinder.Bind<BeginSurveyingSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<AnimateActionSignal>().ToSingleton();
            injectionBinder.Bind<CameraPanSignal>().ToSingleton();
            injectionBinder.Bind<CameraPanCompleteSignal>().ToSingleton();
            injectionBinder.Bind<CameraPanToPointOfInterestSignal>().ToSingleton();

            commandBinder.Bind<SpawnCombatantSignal>().To<SpawnCombatantCommand>();
            commandBinder.Bind<BeginSurveyingSignal>().To<BeginSurveyingCommand>();
            commandBinder.Bind<ExitContextMenuSignal>().To<ExitContextMenuCommand>();
            commandBinder.Bind<EndTurnSignal>().To<EndTurnCommand>();
            commandBinder.Bind<ContextRequestedSignal>().To<ShowContextMenuCommand>();
            commandBinder.Bind<NextBattleSignal>().To<NextBattleCommand>();

            // When the intro cutscene completes, load the BattlePrep context into the scene
            // and then trigger it.
            commandBinder.Bind<IntroCutsceneCompleteSignal>()
                .To<LoadPreparationsCommand>();

            commandBinder.Bind<BattleStartSignal>().To<StartBattleCommand>();
            commandBinder.Bind<InitializeMapSignal>().To<InitializeMapCommand>();
            commandBinder.Bind<MapPositionClickedSignal>().To<SelectMapPositionCommand>();
            commandBinder.Bind<HoverPositionSignal>().To<MapHoveredCommand>();
            commandBinder.Bind<BackSignal>().To<BackCommand>();
            commandBinder.Bind<CombatActionSelectedSignal>().To<CombatActionSelectedCommand>();
            commandBinder.Bind<StateTransitionSignal>().To<StateTransitionCommand>();
            commandBinder.Bind<ActionAnimationCompleteSignal>().To<ActionCompleteCommand>();
            commandBinder.Bind<FightConfirmedSignal>().To<FightConfirmedCommand>();
            commandBinder.Bind<PhaseChangeStartSignal>().To<PhaseChangeStartCommand>();
            commandBinder.Bind<PhaseChangeCompleteSignal>().To<PhaseChangeCompleteCommand>();
            commandBinder.Bind<PlayerTurnCompleteSignal>().To<NextPhaseCommand>();
            commandBinder.Bind<EnemyTurnCompleteSignal>().To<NextPhaseCommand>();
            commandBinder.Bind<IncrememtTurnSignal>().To<IncrementTurnCommand>();
            commandBinder.Bind<EnemyTurnStartSignal>().To<RunEnemyActionsCommand>();

            mediationBinder.Bind<MapView>().To<MapViewMediator>();
            mediationBinder.Bind<MapHighlightView>().To<MapHighlightViewMediator>();
            mediationBinder.Bind<ActionMenuView>().To<ActionMenuViewMediator>();
            mediationBinder.Bind<BattleView>().To<BattleViewMediator>();
            mediationBinder.Bind<MovementPathView>().To<MovementPathViewMediator>();
            mediationBinder.Bind<CombatForecastView>().To<CombatForecastViewMediator>();
            mediationBinder.Bind<CombatantView>().To<CombatantViewMediator>();
            mediationBinder.Bind<LiatView>().To<LiatViewMediator>();
            mediationBinder.Bind<JanekView>().To<JanekViewMediator>();
            mediationBinder.Bind<CombatEffectsView>().To<CombatEffectsViewMediator>();
            mediationBinder.Bind<PhaseChangeView>().To<PhaseChangeViewMediator>();
            mediationBinder.Bind<IntroCutsceneView>().To<IntroCutsceneViewMediator>();
            mediationBinder.Bind<ContextMenuView>().To<ContextMenuViewMediator>();
            mediationBinder.Bind<CameraView>().To<CameraViewMediator>();
        }
    }
}