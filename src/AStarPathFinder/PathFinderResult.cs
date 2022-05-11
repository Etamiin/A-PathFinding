using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inertia.Tools
{
    public sealed class PathFinderResult
    {
        public bool IsEndOfPath => _currentIndex < 0;

        private Cell[] _path;
        private int _currentIndex;

        public PathFinderResult(Cell[] path)
        {
            _path = path;
            Reset();
        }

        public PathFinderResult Reset()
        {
            _currentIndex = _path.Length - 1;
            return this;
        }
        public Cell GetNextCell()
        {
            if (IsEndOfPath) return null;

            return _path[_currentIndex--];
        }
    }
}
