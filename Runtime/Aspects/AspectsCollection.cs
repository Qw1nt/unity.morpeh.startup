using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Qw1nt.MorpehStartup.Aspects
{
    public class AspectsCollection : IAspectsCollection, IEnumerator<EcsAspectBase>, IEnumerable<EcsAspectBase>
    {
        private readonly List<EcsAspectBase> _aspects = new(32);
        private int _pointer = -1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            _aspects.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public AspectsCollection Add(EcsAspectBase aspectBase)
        {
            _aspects.Add(aspectBase);
            return this;
        }

        public AspectsCollection Add<T>() where T : EcsAspectBase, new()
        {
            _aspects.Add(new T());
            return this;
        }

        public bool MoveNext()
        {
            if (_pointer >= _aspects.Count - 1)
                return false;

            _pointer++;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            _pointer = -1;
        }

        public EcsAspectBase Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
#if UNITY_EDITOR
                if (_pointer == -1 || _pointer >= _aspects.Count)
                    throw new ArgumentException();
#endif

                return _aspects[_pointer];
            }
        }

        object IEnumerator.Current => Current;

        public IEnumerator<EcsAspectBase> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            _aspects.Clear();
            _aspects.Capacity = 0;
        }
    }
}