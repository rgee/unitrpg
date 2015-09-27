using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Dialogue {
    public class DialogueUtils {
        public static Cutscene ParseFromJson(string rawCutsceneJson) {
            var json = new JSONObject(rawCutsceneJson);
            var result = new Cutscene();
            foreach (var deck in json["decks"].list) {
                var parsedDeck = new Deck();
                parsedDeck.Speaker = deck["speaker"].str;

                var cards = deck["cards"];
                foreach (var card in cards.list) {
                    var parsedCard = new Card();
                    foreach (var line in card["lines"].list) {
                        parsedCard.Lines.Add(line.str);
                    }
                    parsedDeck.Cards.Add(parsedCard);
                }

                result.Decks.Add(parsedDeck);
            }

            return result;
        }
    }
}
