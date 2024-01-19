using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    private HeroAcademy heros = new HeroAcademy();

    private void Start() {
        foreach (string heroName in heros) {
            print(heroName);
        }
    }

    //This class can be enumerated
    class HeroAcademy : IEnumerable {

        private List<string> olympianNames = new List<string>() {
            "Zeus", "Poisedon", "Hades"
        };

        public IEnumerator GetEnumerator() {
            return olympianNames.GetEnumerator();
        }
    }
}