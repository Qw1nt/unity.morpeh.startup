using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Qw1nt.MorpehStartup.Systems
{
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class CleanupDelHereSystem<T> : ICleanupSystem where T : struct, IComponent
    {
        public World World { get; set; }

        private Stash<T> _stash;
        
        public void OnAwake()
        {
            _stash = World.GetStash<T>();
        }

        public void OnUpdate(float deltaTime)
        {
            _stash.RemoveAll();
        }

        public void Dispose()
        {
        }
    }
}