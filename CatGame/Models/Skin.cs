﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CatGame.Models
{
    public class Skin : INotifyPropertyChanged
    {
        private string _name;
        private string _imagePath;
        private int _price;
        private bool _isPurchased;
        private bool _isActive;
        private bool _showInsufficientFunds;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool ShowInsufficientFunds
        {
            get => _showInsufficientFunds;
            set
            {
                _showInsufficientFunds = value;
                OnPropertyChanged(nameof(ShowInsufficientFunds));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public int Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        public bool IsPurchased
        {
            get => _isPurchased;
            set
            {
                _isPurchased = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}