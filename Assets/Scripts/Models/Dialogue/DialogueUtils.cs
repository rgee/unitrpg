using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Dialogue {
    public class DialogueUtils
    {
        public static Cutscene ParseFromJson(string rawCutsceneJson) {
            var json = new JSONObject(rawCutsceneJson, strict:true);
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

                    var responses = card["emotionalResponses"];
                    var responseActors = responses.keys;
                    foreach (var actor in responseActors) {
                        var response = responses[actor];

                        var emotion = GetEmotionForString(response["emotion"].str);
                        var facing = response["facing"].str == "left" ? Facing.Left : Facing.Right;
                        parsedCard.EmotionalResponses.Add(actor, new EmotionalResponse(emotion, facing));
                    }

                    parsedDeck.Cards.Add(parsedCard);
                }

                result.Decks.Add(parsedDeck);
            }

            var speakers = json["actors"].list;
            result.Speakers = speakers.Select(obj => obj.str).ToList();

            return result;
        }
        
        private static EmotionType GetEmotionForString(string emotion) {
            if (emotion == "default") {
                return EmotionType.DEFAULT;
            }

            if (emotion == "worried") {
                return EmotionType.WORRIED;
            }

            if (emotion == "happy") {
                return EmotionType.HAPPY;
            }

            throw new ArgumentException("Invalid emotion type " + emotion);
        }
    }
}
