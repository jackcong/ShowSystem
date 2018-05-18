using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ComLib.File.Csv
{
    public class CsvRowEnumerator : IEnumerator<CsvCell>
    {
        private int _position = -1;
        private CsvRow _cr;

        public CsvRowEnumerator(CsvRow cr)
        {
            _cr = cr;
        }

        public void Dispose()
        {
            // TODO:
        }

        public bool MoveNext()
        {
            if (_position < _cr.Length - 1)
            {
                ++_position;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            _position = -1;
        }

        public CsvCell Current
        {
            get { return _cr[_position]; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }

    public class CsvRow : IEnumerable<CsvCell>
    {
        //private bool _isDisposed = false;
        private List<CsvCell> _values;
        
        internal CsvRow()
        {
            _values=new List<CsvCell>();
        }

        internal CsvRow(int length)
        {
            _values=new List<CsvCell>(length);
        }

        public CsvCell this[int index]
        {
            get { return _values[index]; }
            set { _values[index] = value; }
        }

        public CsvCell this[string key]
        {
            get
            {
                foreach(var c in _values)
                {
                    if(c.Key==key)
                    {
                        return c;
                    }
                }
                throw new System.Exception("The key is not found in the file.");
            }
        }

        public bool ContainsKey(string key)
        {
            return _values.Any(c => c.Key == key);
        }

        public int Length
        {
            get { return _values.Count; }
        }

        public void Add(CsvCell cell)
        {
            _values.Add(cell);
        }

        public IEnumerator<CsvCell> GetEnumerator()
        {
            return new CsvRowEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // The classical Disposable Pattern, may be useful in some circumstances.
        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //protected void Dispose(bool disposing)
        //{
        //    if (!_isDisposed)
        //    {
        //        if (disposing)
        //        {
        //        }
        //    }
        //    _isDisposed = true;
        //}

        //~CsvRow()
        //{
        //    Dispose(false);
        //}
    }

    public class CsvCell
    {
        public string Key { get; private set; }
        public string Value { get; set; }

        public CsvCell(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public CsvCell()
        {
        }
    }

    public class CsvDataEnumerator : IEnumerator<CsvRow>
    {
        private int _position = -1;
        private CsvData _cd;

        public CsvDataEnumerator(CsvData cd)
        {
            _cd = cd;
        }

        public void Dispose()
        {
            // TODO:
        }

        public bool MoveNext()
        {
            if (_position < _cd.Length - 1)
            {
                ++_position;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            _position = -1;
        }

        public CsvRow Current
        {
            get { return _cd[_position]; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }

    public class CsvData : IEnumerable<CsvRow>
    {
        private readonly List<CsvRow> _data=new List<CsvRow>();

        private string[] _head;

        public CsvRow this[int index]
        {
            get { return _data[index]; }
        }

        public void BuildHead(string[] head)
        {
            _head = new string[head.Length];
            head.CopyTo(_head,0);
        }

        public void Add(CsvRow row)
        {
            _data.Add(row);
        }

        public int Length
        {
            get { return _data.Count; }
        }

        public string[] Head
        {
            get { return _head; }
        }

        public void Build(string[] head, string[,] body)
        {
            if (head.Length != body.GetLength(1))
                throw new System.Exception("The length of the head does not equal to the length of the body.");
            for (int i = 0; i < body.GetLength(0); ++i)
            {
                CsvRow cr = new CsvRow(body.GetLength(1));
                for (int j = 0; j < body.GetLength(1); ++j)
                {
                    cr.Add(new CsvCell(head[i], body[i, j]));
                }
                _data.Add(cr);
            }
        }

        public IEnumerator<CsvRow> GetEnumerator()
        {
            return new CsvDataEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
