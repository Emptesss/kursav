using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using CatGame.Models;

namespace CatGame.Services
{
    public class GameDataService
    {
        private readonly string _saveFilePath;

        public GameDataService()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string savesDir = Path.Combine(basePath, "Saves");

            // Создаем директорию если её нет
            Directory.CreateDirectory(savesDir);

            _saveFilePath = Path.Combine(savesDir, "gamedata.json");
            Debug.WriteLine($"Путь к файлу сохранения: {_saveFilePath}");
        }

        public void SaveGame(GameData gameData)
        {
            try
            {
                var saveData = new GameSaveData
                {
                    Balance = gameData.Balance,
                    CurrentGameBalance = gameData.CurrentGameBalance,
                    SelectedSkinName = gameData.SelectedSkin?.Name,
                    SelectedWallpaperName = gameData.SelectedWallpaper?.Name,
                    SelectedLockerName = gameData.SelectedLocker?.Name,
                    PurchasedSkins = gameData.Skins
                        .Where(s => s.IsPurchased)
                        .Select(s => s.Name)
                        .ToList(),
                    PurchasedWallpapers = gameData.Wallpapers
                        .Where(w => w.IsPurchased)
                        .Select(w => w.Name)
                        .ToList(),
                    PurchasedLockers = gameData.Lockers
                        .Where(l => l.IsPurchased)
                        .Select(l => l.Name)
                        .ToList(),
                    CatProfile = gameData.CatProfile  // Добавляем это
                };

                string jsonString = JsonSerializer.Serialize(saveData, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(_saveFilePath, jsonString);
                Debug.WriteLine("Данные успешно сохранены:");
                Debug.WriteLine(jsonString);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при сохранении данных: {ex.Message}");
                throw;
            }
        }

        public void LoadGame(GameData gameData)
        {
            if (!File.Exists(_saveFilePath))
            {
                Debug.WriteLine("Файл сохранения не найден");
                return;
            }

            try
            {
                string jsonString = File.ReadAllText(_saveFilePath);
                Debug.WriteLine("Загружены данные:");
                Debug.WriteLine(jsonString);

                var saveData = JsonSerializer.Deserialize<GameSaveData>(jsonString);

                if (saveData != null)
                {
                    // Устанавливаем баланс
                    gameData.Balance = saveData.Balance;
                    Debug.WriteLine($"Загружен баланс из файла: {saveData.Balance}");

                    gameData.CurrentGameBalance = saveData.CurrentGameBalance;

                    // Восстанавливаем купленные скины
                    foreach (var skinName in saveData.PurchasedSkins)
                    {
                        var skin = gameData.Skins.FirstOrDefault(s => s.Name == skinName);
                        if (skin != null)
                        {
                            skin.IsPurchased = true;
                            Debug.WriteLine($"Восстановлен купленный скин: {skinName}");
                        }
                    }

                    if (saveData.CatProfile != null)
                    {
                        gameData.CatProfile = saveData.CatProfile;
                        Debug.WriteLine($"Восстановлен профиль кота: {saveData.CatProfile.Name}");
                    }


                    // Восстанавливаем купленные обои
                    foreach (var wallpaperName in saveData.PurchasedWallpapers)
                    {
                        var wallpaper = gameData.Wallpapers.FirstOrDefault(w => w.Name == wallpaperName);
                        if (wallpaper != null)
                        {
                            wallpaper.IsPurchased = true;
                            Debug.WriteLine($"Восстановлены купленные обои: {wallpaperName}");
                        }
                    }
                    foreach (var lockerName in saveData.PurchasedLockers) // Добавляем это
                    {
                        var locker = gameData.Lockers.FirstOrDefault(l => l.Name == lockerName);
                        if (locker != null) locker.IsPurchased = true;
                    }
                    // Восстанавливаем выбранный скин
                    if (!string.IsNullOrEmpty(saveData.SelectedSkinName))
                    {
                        var selectedSkin = gameData.Skins.FirstOrDefault(s => s.Name == saveData.SelectedSkinName);
                        if (selectedSkin != null)
                        {
                            gameData.SelectedSkin = selectedSkin;
                            Debug.WriteLine($"Восстановлен выбранный скин: {saveData.SelectedSkinName}");
                        }
                    }

                    // Восстанавливаем выбранные обои
                    if (!string.IsNullOrEmpty(saveData.SelectedWallpaperName))
                    {
                        var selectedWallpaper = gameData.Wallpapers.FirstOrDefault(w => w.Name == saveData.SelectedWallpaperName);
                        if (selectedWallpaper != null)
                        {
                            gameData.SelectedWallpaper = selectedWallpaper;
                            Debug.WriteLine($"Восстановлены выбранные обои: {saveData.SelectedWallpaperName}");
                        }
                    }
                    if (!string.IsNullOrEmpty(saveData.SelectedLockerName))
                    {
                        var selectedLocker = gameData.Lockers.FirstOrDefault(l => l.Name == saveData.SelectedLockerName);
                        if (selectedLocker != null) gameData.SelectedLocker = selectedLocker;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
                throw;
            }
        }
    }
}