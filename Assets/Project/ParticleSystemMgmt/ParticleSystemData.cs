
using UnityEngine;

namespace Game.Project.ParticleSystemMgmt
{
    [System.Serializable]
    public class ParticleSystemData
    {
        public ParticleSystemType type;
        public ParticleSystemHandler particleSystem;
        [Min(10)]
        public int count = 10;
    }
}
