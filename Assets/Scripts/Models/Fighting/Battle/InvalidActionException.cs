using System;

namespace Models.Fighting.Battle {
    public class InvalidActionException : Exception {
        public InvalidActionException(string message) : base("Invalid Action: " + message) {
        }
    }
}