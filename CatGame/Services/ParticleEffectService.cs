using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using CatGame.Models;

namespace CatGame.Services
{
    public class ParticleEffectService
    {
        private readonly Random _random = new Random();
        private readonly DispatcherTimer _updateTimer;
        public ObservableCollection<BubbleParticle> Particles { get; } = new ObservableCollection<BubbleParticle>();

        public ParticleEffectService()
        {
            _updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16)
            };
            _updateTimer.Tick += UpdateParticles;
            _updateTimer.Start();
        }

        public async Task CreateBubblePopEffect(Point position, Color color, double bubbleSize)
        {
            // Создаем эффект трещин (маленькие частицы, летящие внутрь)
            for (int i = 0; i < 8; i++)
            {
                double angle = (Math.PI * 2 * i) / 8;
                var velocity = new Vector(
                    Math.Cos(angle) * -100, // Отрицательная скорость - движение к центру
                    Math.Sin(angle) * -100
                );

                var crack = new BubbleParticle
                {
                    Position = new Point(
                        position.X + Math.Cos(angle) * bubbleSize / 3,
                        position.Y + Math.Sin(angle) * bubbleSize / 3
                    ),
                    Velocity = velocity,
                    Size = bubbleSize * 0.15,
                    Opacity = 1,
                    Color = color,
                    LifeTime = 0.1,
                    IsShard = false
                };

                Particles.Add(crack);
            }

            // Небольшая задержка перед разлетом осколков
            await Task.Delay(50);

            // Создаем осколки, разлетающиеся наружу
            for (int i = 0; i < 8; i++)
            {
                double angle = (Math.PI * 2 * i) / 8 + _random.NextDouble() * 0.2;
                double speed = 300 + _random.NextDouble() * 100;

                var velocity = new Vector(
                    Math.Cos(angle) * speed,
                    Math.Sin(angle) * speed
                );

                var shard = new BubbleParticle
                {
                    Position = position,
                    Velocity = velocity,
                    Size = bubbleSize * 0.3,
                    Opacity = 1,
                    Color = color,
                    LifeTime = 0.3,
                    Rotation = angle * (180 / Math.PI),
                    IsShard = true
                };

                Particles.Add(shard);
            }
        }

        private void UpdateParticles(object sender, EventArgs e)
        {
            const double deltaTime = 0.016;

            for (int i = Particles.Count - 1; i >= 0; i--)
            {
                var particle = Particles[i];

                particle.Position = new Point(
                    particle.Position.X + particle.Velocity.X * deltaTime,
                    particle.Position.Y + particle.Velocity.Y * deltaTime
                );

                if (particle.IsShard)
                {
                    // Для осколков добавляем немного гравитации
                    particle.Velocity = new Vector(
                        particle.Velocity.X * 0.95,
                        particle.Velocity.Y * 0.95 + 400 * deltaTime
                    );
                }

                particle.LifeTime -= deltaTime;
                particle.Opacity = particle.LifeTime * 3; // Быстрое затухание

                if (particle.LifeTime <= 0)
                {
                    Particles.RemoveAt(i);
                }
            }
        }
    }
}