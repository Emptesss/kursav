using System.Windows.Media;

namespace CatGame.Services
{
    public class MusicPlayer
    {
        private readonly MediaPlayer _mediaPlayer = new MediaPlayer();

        public MusicPlayer()
        {
            _mediaPlayer.MediaEnded += (s, e) => _mediaPlayer.Position = TimeSpan.Zero;
        }

        public void Play(string filePath)
        {
            _mediaPlayer.Open(new Uri(filePath, UriKind.RelativeOrAbsolute));
            _mediaPlayer.Play();
            _mediaPlayer.Volume = 0.2; // Начальная громкость
        }

        public void SetVolume(double volume)
        {
            _mediaPlayer.Volume = volume;
        }

        public void TogglePause()
        {
            if (_mediaPlayer.CanPause)
                _mediaPlayer.Pause();
            else
                _mediaPlayer.Play();
        }
    }
}