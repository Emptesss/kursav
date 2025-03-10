using System.Windows.Media;

namespace CatGame.Services
{
    public class SoundService
    {
        private readonly MediaPlayer _mediaPlayer = new MediaPlayer();

        public void PlayClickSound()
        {
            _mediaPlayer.Open(new Uri("Views/zvukknopki.mp3", UriKind.Relative));
            _mediaPlayer.Play();
            _mediaPlayer.Volume = 0.5; // Настройка громкости
        }
    }
}