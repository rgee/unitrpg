using System;
using Assets.Contexts.Chapters.Chapter2.Models;
using Assets.Contexts.Chapters.Chapter2.Signals;
using strange.extensions.command.impl;

namespace Assets.Contexts.Chapters.Chapter2.Commands {
    public class MarkHouseVisitedCommand : Command {
        [Inject]
        public House House { get; set; }

        [Inject]
        public HouseLightDisableSignal HouseLightDisableSignal { get; set; }

        [Inject]
        public EastmerePlazaState ChapterModel { get; set; }

        public override void Execute() {
            switch (House) {
                case House.Inn:
                    ChapterModel.InnInspected = true;
                    HouseLightDisableSignal.Dispatch(House);
                    break;
                case House.Generic:
                    ChapterModel.HouseInspected = true;
                    HouseLightDisableSignal.Dispatch(House);
                    break;
                case House.Clinic:
                    ChapterModel.ClinicInspected = true;
                    // Spawn Maelle
                    // Spawn reinforcements
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
