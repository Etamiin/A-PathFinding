using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inertia.Tools
{
    public sealed class PathFinderResult<T> where T : Cell
    {
        public bool IsEndOfPath => _currentIndex < 0;

        private T[] _path;
        private int _currentIndex;

        public PathFinderResult(T[] path)
        {
            _path = path;
            Reset();
        }

        public PathFinderResult<T> Reset()
        {
            _currentIndex = _path.Length - 1;
            return this;
        }
        public T GetNextCell()
        {
            if (IsEndOfPath) return null;

            return _path[_currentIndex--];
        }
    }
}
