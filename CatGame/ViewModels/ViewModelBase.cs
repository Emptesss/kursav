using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CatGame.ViewModels
{
    public class ViewModelBase : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Реализация INotifyPropertyChanged
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Метод для установки обычных свойств
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        // Методы для работы с DependencyProperty
        protected static DependencyProperty Register<T>(
            string name,
            PropertyMetadata metadata = null)
        {
            return DependencyProperty.Register(
                name,
                typeof(T),
                typeof(ViewModelBase),
                metadata ?? new PropertyMetadata(default(T)));
        }

        protected T GetValue<T>(DependencyProperty property)
            => (T)GetValue(property);

        protected void SetValue<T>(DependencyProperty property, T value)
            => SetValue(property, value);
    }
}