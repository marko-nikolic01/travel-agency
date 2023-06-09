using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.WPF.ViewModels.Guest1Demo
{
    public class DemoInstruction : INotifyPropertyChanged
    {
        private int _row;
        private int _column;
        private int _rowSpan;
        private int _columnSpan;
        private string _text;

        public int Row
        {
            get => _row;
            set
            {
                if (value != _row)
                {
                    _row = value;
                    OnPropertyChanged();
                }
            }
        }

        public int RowSpan
        {
            get => _rowSpan;
            set
            {
                if (value != _rowSpan)
                {
                    _rowSpan = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Column
        {
            get => _column;
            set
            {
                if (value != _column)
                {
                    _column = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ColumnSpan
        {
            get => _columnSpan;
            set
            {
                if (value != _columnSpan)
                {
                    _columnSpan = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                if (value != _text)
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }

        public DemoInstruction()
        {
            Row = 0;
            Column = 0;
            RowSpan = 0;
            ColumnSpan = 0;
            Text = "";
        }

        public void UpdateInstruction(int row, int column, int rowSpan, int columnSpan, string text)
        {
            Row = row;
            Column = column;
            RowSpan = rowSpan;
            ColumnSpan = columnSpan;
            Text = text;
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
