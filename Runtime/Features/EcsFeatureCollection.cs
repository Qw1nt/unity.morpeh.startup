using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Qw1nt.MorpehStartup.Aspects;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace Qw1nt.MorpehStartup.Features
{
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class EcsFeatureCollection
    {
        private readonly FastList<IEcsFeature> _features = new();
        private readonly AspectsCollection _aspectsCollection = new();
        private readonly FastList<IPrepareSystem> _preparedSystems = new(32);

        public bool IsBuild { get; private set; } = false;

        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _features.length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsFeatureCollection Add<T>() where T : class, IEcsFeature, new()
        {
            _features.Add(new T());
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsFeatureCollection Add<T>(T instance) where T : class, IEcsFeature
        {
            _features.Add(instance);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async UniTaskVoid Build(World world, IObjectResolver resolver)
        {
            var initializationQueue = new Queue<InitializationQueueItem>(_features.capacity * 2);
            var systemIndex = 0;

            foreach (var feature in _features)
            {
                _aspectsCollection.Reset();
                feature.Build(_aspectsCollection);
                
                foreach (var aspect in _aspectsCollection)
                {
                    aspect.SystemsGroup = world.CreateSystemsGroup();
                    aspect.Resolver = resolver;
                    aspect.PreparedSystems = _preparedSystems;
                    
                    aspect.Init();
                    await aspect.Prepare();

                    initializationQueue.Enqueue(new InitializationQueueItem(systemIndex, aspect.SystemsGroup));
                    systemIndex++;
                }
            }

            _aspectsCollection.Dispose();

            _features.Clear();
            _features.capacity = 0;
            
            _preparedSystems.Clear();
            _preparedSystems.capacity = 0;

            while (initializationQueue.Count > 0)
            {
                var item = initializationQueue.Dequeue();
                world.AddSystemsGroup(item.Index, item.SystemsGroup);
            }
            
            IsBuild = true;
        }
        
        // TODO Rename
        private struct InitializationQueueItem
        {
            public InitializationQueueItem(int index, SystemsGroup systemsGroup)
            {
                Index = index;
                SystemsGroup = systemsGroup;
            }

            public int Index { get; }
            
            public SystemsGroup SystemsGroup { get; }
        }
    }
}