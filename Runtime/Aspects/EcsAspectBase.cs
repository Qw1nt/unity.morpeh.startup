using Cysharp.Threading.Tasks;
using Qw1nt.MorpehStartup.Systems;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace Qw1nt.MorpehStartup.Aspects
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public abstract class EcsAspectBase
    {
        public SystemsGroup SystemsGroup { get; set; }

        public IObjectResolver Resolver { get; set; }
        
        public FastList<IPrepareSystem> PreparedSystems { get; set; }

        public abstract void Init();

        public async UniTask Prepare()
        {
            foreach (var preparedSystem in PreparedSystems)
                await preparedSystem.Prepare();

            PreparedSystems.Clear();
            PreparedSystems = null;
        }

        protected void Add<T>() where T : class, IInitializer, new()
        {
            DeterminateSystem(new T());
        }

        protected void Add<T>(T instance) where T : class, IInitializer
        {
            DeterminateSystem(instance);
        }

        protected void Cleanup<T>() where T : struct, IComponent
        {
            SystemsGroup.AddSystem(new CleanupDelHereSystem<T>());
        }

        protected void DelHere<T>() where T : struct, IComponent
        {
            SystemsGroup.AddSystem(new DelHereSystem<T>());
        }

        private void DeterminateSystem(object instance)
        {
            Resolver.Inject(instance);

            if (instance is IPrepareSystem prepareSystem)
                PreparedSystems.Add(prepareSystem);
            
            switch (instance)
            {
                case ICleanupSystem cleanupSystem:
                    SystemsGroup.AddSystem(cleanupSystem);
                    break;
                case ISystem system:
                    SystemsGroup.AddSystem(system);
                    break;
                case IInitializer initializer:
                    SystemsGroup.AddInitializer(initializer);
                    break;
            }
        }
    }
}