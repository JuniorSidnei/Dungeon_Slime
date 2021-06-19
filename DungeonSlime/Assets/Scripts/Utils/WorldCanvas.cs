using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using TMPro;
using UnityEngine;

namespace DungeonSlime.Utils {

    [RequireComponent(typeof(Canvas))]
    public class WorldCanvas : Singleton<WorldCanvas> {

        public TextMeshProUGUI TextPrefab;

        public TextMeshProUGUI CreateTextAt(Vector3 position) {
            return Instantiate(TextPrefab, new Vector3(position.x + 0.1f, position.y + 0.1f), Quaternion.identity, transform);
        }
    }
}