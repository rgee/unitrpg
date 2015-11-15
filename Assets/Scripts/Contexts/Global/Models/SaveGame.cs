using System;
using System.Runtime.Serialization;

namespace Contexts.Global.Models {
    [Serializable]
    public class SaveGame : ISerializable {
        public DateTime LastSaveTime { get; private set; }
        public int? LastChapterCompleted { get; private set; }
        public SavedBattleState CurrentBattleState { get; private set; }
        public SavedCharacters SavedCharacters { get; private set; }


        public SaveGame(int? lastChapterCompleted, SavedBattleState currentBattleState, SavedCharacters savedCharacters) {
            LastChapterCompleted = lastChapterCompleted;
            CurrentBattleState = currentBattleState;
            SavedCharacters = savedCharacters;
        }

        protected SaveGame(SerializationInfo info, StreamingContext ctx) {
            foreach (var entry in info) {
                switch (entry.Name) {
                    case "LastChapterCompleted":
                        LastChapterCompleted = (int) entry.Value;
                        break;
                    case "CurrentBattleState":
                        CurrentBattleState = (SavedBattleState) entry.Value;
                        break;
                    case "SavedCharacters":
                        SavedCharacters = (SavedCharacters) entry.Value;
                        break;
                    case "LastSaveTime":
                        LastSaveTime = (DateTime) entry.Value;
                        break;
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("CurrentBattleState", CurrentBattleState);
            info.AddValue("LastChapterCompleted", LastChapterCompleted);
            info.AddValue("SavedCharacters", SavedCharacters);
            info.AddValue("LastSaveTime", new DateTime());
        }
    }
}