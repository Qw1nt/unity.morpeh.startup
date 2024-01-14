using System.Runtime.CompilerServices;
using Qw1nt.MorpehStartup.Features;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace Qw1nt.Runtime.MorpehStartup
{
    public abstract class EcsStartupBase : MonoBehaviour
    {
        [Inject] private readonly IObjectResolver _objectResolver;

        private readonly EcsFeatureCollection _features = new();

        private static World World => World.Default;

        private void Awake()
        {
            Setup();
            _features.Build(World, _objectResolver).Forget();
        }

        protected virtual bool IsFreeze()
        {
            return false;
        }
        
        private void FixedUpdate()
        {
            if (IsFreeze() == true)
                return;

            World.FixedUpdate(Time.fixedDeltaTime);
        }

        private void Update()
        {
            if (IsFreeze() == true)
                return;

            World.Update(Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (IsFreeze() == true)
                return;

            World.LateUpdate(Time.deltaTime);
            World.CleanupUpdate(Time.deltaTime);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Add<T>() where T : class, IEcsFeature, new()
        {
            var feature = new T();
            _features.Add(feature);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Add<T>(T instance) where T : class, IEcsFeature
        {
            _features.Add(instance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract void Setup();

        private void OnDestroy()
        {
            World.Default?.Dispose();
        }
    }
}