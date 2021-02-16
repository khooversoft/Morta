using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.Tools
{
    /// <summary>
    /// Cursor list, implements a minimum required for list with a cursor as an index
    ///   Used to process a set of data in a list where a pointer to the current position is required.
    ///   Cursor can be saved and restored, or abandon, used for nested processing
    /// </summary>
    /// <typeparam name="T">list type</typeparam>
    public class CursorList<T> : IEnumerable<T>
    {
        private readonly List<T> _list;
        private readonly Stack<int> _cursorStack = new Stack<int>();
        private int _cursor = 0;

        public CursorList()
        {
            _list = new List<T>();
        }

        public CursorList(IEnumerable<T> list)
        {
            _list = new List<T>(list);
        }

        public T this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }

        public int Count => _list.Count;

        public bool EndOfList => _list.Count == 0 || _cursor >= _list.Count;

        public int Cursor
        {
            get { return _cursor; }
            set { _cursor = Math.Min(_list.Count, value); }
        }

        public T Current
        {
            get
            {
                if (EndOfList)
                {
                    throw new InvalidOperationException("End of list has been reached");
                }

                return _list[_cursor];
            }
        }

        public IReadOnlyList<T> FromCursor { get { return _list.Skip(Cursor).ToList(); } }

        public void Clear() => _list.Clear();

        public void Add(T value) => _list.Add(value);

        public void RemoveAt(int index) => _list.RemoveAt(index);

        public IEnumerable<T> Next(int count)
        {
            while (!EndOfList)
            {
                yield return Next();
            }
        }

        public T Next()
        {
            if (_cursor >= _list.Count)
            {
                throw new InvalidOperationException("End of list has been reached");
            }

            return _list[_cursor++];
        }

        public bool TryNext(out T value)
        {
            value = default!;

            if (_cursor >= _list.Count)
            {
                return false;
            }

            value = _list[_cursor++];
            return true;
        }

        public void SaveCursor() => _cursorStack.Push(_cursor);

        public void RestoreCursor()
        {
            if (_cursorStack.Count == 0)
            {
                throw new InvalidOperationException("No cursor(s) to restore");
            }

            _cursor = _cursorStack.Pop();
        }

        public void AbandonSavedCursor()
        {
            if (_cursorStack.Count == 0)
            {
                throw new InvalidOperationException("No cursor(s) to restore");
            }

            _cursorStack.Pop();
        }

        public override string ToString()
        {
            var list = new List<string>()
            {
                $"{nameof(Cursor)}={Cursor}",
                $"{nameof(Current)}={Current}",
                $"{nameof(Count)}={Count}",
                $"{nameof(EndOfList)}={EndOfList}",
                $"FromCursor(5): ({string.Join(", ", FromCursor.Take(5).Select(x => x?.ToString() ?? "<none>"))})",
            };

            return string.Join(", ", list);
        }

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
    }
}
