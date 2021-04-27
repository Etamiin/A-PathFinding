using System;

namespace Inertia.PathFinding
{
    public class HeapCollection<T> where T : IHeapItem<T>
    {
        public int Count { get; private set; }

        private T[] _items;
        
        public HeapCollection(int maxSize)
        {
            _items = new T[maxSize];
        }

        public bool Contains(T item)
        {
            return Equals(_items[item.HeapIndex], item);
        }
        public void Add(T item)
        {
            item.HeapIndex = Count;
            _items[item.HeapIndex] = item;

            SortUp(item);
            Count++;
        }
        public T RemoveFirstItem()
        {
            var first = _items[0];
            Count--;

            _items[0] = _items[Count];
            _items[0].HeapIndex = 0;

            SortDown(_items[0]);
            return first;
        }

        public void UpdateItem(T item)
        {
            SortUp(item);
        }

        private void SortUp(T item)
        {
            var parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                var parent = _items[parentIndex];
                if (item.CompareTo(parent) > 0)
                    SwapItems(item, parent);
                else
                    break;

                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }
        private void SortDown(T item)
        {
            while (true)
            {
                var indexLeft = item.HeapIndex * 2 + 1;
                var indexRight = item.HeapIndex * 2 + 2;
                if (indexLeft < Count)
                {
                    var swapIndex = indexLeft;
                    if (indexRight < Count)
                    {
                        if (_items[indexLeft].CompareTo(_items[indexRight]) < 0)
                            swapIndex = indexRight;
                    }

                    if (item.CompareTo(_items[swapIndex]) < 0)
                        SwapItems(item, _items[swapIndex]);
                    else
                        break;
                }
                else
                {
                    break;
                }
            }
        }
        private void SwapItems(T item0, T item1)
        {
            _items[item0.HeapIndex] = item1;
            _items[item1.HeapIndex] = item0;

            var mIndex = item0.HeapIndex;
            item0.HeapIndex = item1.HeapIndex;
            item1.HeapIndex = mIndex;
        }
    }

    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex { get; set; }
    }
}
