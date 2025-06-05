using System;
using System.IO;

namespace EducationalEventGenerator
{
    public static class HighScoreManager
    {
        private const string FilePath = "highscore.txt";

        public static int LoadHighScore()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string content = File.ReadAllText(FilePath);
                    if (int.TryParse(content, out int highScore))
                    {
                        return highScore;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Ошибка загрузки рекорда: {ex.Message}");
            }
            return 0;
        }

        public static void SaveHighScore(int score)
        {
            try
            {
                File.WriteAllText(FilePath, score.ToString());
            }
            catch (Exception ex)
            {
                Logger.Log($"Ошибка сохранения рекорда: {ex.Message}");
            }
        }
    }
}