// ------------------------------------------------------------------------------
// File: CutscenePlayer.cs
// Description: Plays Timeline cutscenes by name.
// Author: (Your Name)
// Date: 2025-08-10
// Unity Version: 2022.3 LTS
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace MemorySketch
{
    public class CutscenePlayer : MonoBehaviour
    {
        [System.Serializable]
        public class NamedCutscene
        {
            public string name;
            public PlayableDirector director;
        }

        [SerializeField] private List<NamedCutscene> cutscenes = new List<NamedCutscene>();

        public void PlayCutscene(string cutsceneName)
        {
            var cs = cutscenes.Find(c => c.name == cutsceneName);
            if (cs != null && cs.director != null)
            {
                cs.director.time = 0;
                cs.director.Play();
            }
            else
            {
                Debug.LogWarning("Cutscene not found: " + cutsceneName);
            }
        }
    }
}
