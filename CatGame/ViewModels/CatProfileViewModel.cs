using CatGame.Helpers;
using CatGame.Models;
using CatGame.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace CatGame.ViewModels
{
    public class CatProfileViewModel : ViewModelBase
    {
        private readonly GameData _gameData;
        private readonly NavigationService _navigationService;
        private string _catName;
        private readonly Action _closeAction;
        private int _currentAvatarIndex;
        private readonly List<string> _avatarPaths;

        public string CatName
        {
            get => _catName;
            set => SetProperty(ref _catName, value);
        }

        public string CurrentAvatarImage => _avatarPaths[_currentAvatarIndex];
        public DateTime BirthDate { get; }

        public ICommand SaveCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand NextAvatarCommand { get; }
        public ICommand PreviousAvatarCommand { get; }

        public CatProfileViewModel(GameData gameData, Action closeAction)
        {
            _gameData = gameData;
            _closeAction = closeAction;
            _catName = gameData.CatProfile?.Name ?? "";
            BirthDate = gameData.CatProfile?.BirthDate ?? DateTime.Now;

            // Загружаем все пути к аватарам
            _avatarPaths = _gameData.Skins.Select(s => s.ImagePath).ToList();

            // Находим индекс текущего аватара
            if (_gameData.CatProfile?.AvatarPath != null)
            {
                var index = _avatarPaths.IndexOf(_gameData.CatProfile.AvatarPath);
                _currentAvatarIndex = index != -1 ? index : 0;
            }
            else
            {
                _currentAvatarIndex = 0;
            }

            SaveCommand = new RelayCommand(Save);
            CloseCommand = new RelayCommand(Close);
            NextAvatarCommand = new RelayCommand(NextAvatar);
            PreviousAvatarCommand = new RelayCommand(PreviousAvatar);
        }

        private void Save(object parameter)
        {
            _gameData.CatProfile = new CatProfile
            {
                Name = CatName,
                AvatarPath = CurrentAvatarImage,
                BirthDate = BirthDate
            };

            _gameData.SaveGame();
            Close(null);
        }

        private void Close(object parameter)
        {
            _closeAction?.Invoke();
        }

        private void NextAvatar(object parameter)
        {
            _currentAvatarIndex = (_currentAvatarIndex + 1) % _avatarPaths.Count;
            OnPropertyChanged(nameof(CurrentAvatarImage));
        }

        private void PreviousAvatar(object parameter)
        {
            _currentAvatarIndex = (_currentAvatarIndex - 1 + _avatarPaths.Count) % _avatarPaths.Count;
            OnPropertyChanged(nameof(CurrentAvatarImage));
        }
    }
}
