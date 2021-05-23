using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;

namespace DungeonSlime.Utils {
    public class GameManager : Singleton<GameManager> {

        public QueuedEventDispatcher GlobalDispatcher = new QueuedEventDispatcher();

        private void Update() {
            GlobalDispatcher.DispatchAll();
        }
    }
}
