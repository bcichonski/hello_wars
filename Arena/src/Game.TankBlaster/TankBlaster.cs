﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using Common.Interfaces;
using Common.Models;
using Common.Serialization;
using Game.TankBlaster.Models;
using Game.TankBlaster.Properties;
using Game.TankBlaster.Services;
using Game.TankBlaster.UserControls;

namespace Game.TankBlaster
{
    public class TankBlaster : IGame
    {
        private Battlefield _field;
        private BotService _botService;
        private int _delayTime;
        private ExplosionService _explosionService;
        private TankBlasterConfig _gameConfig;
        private LocationService _locationService;
        private int _roundNumber;
        private int _moveTimeout;

        public TankBlaster()
        {
            var xmlStream = new StreamReader("TankBlaster.config.xml");
            var configurationXml = xmlStream.ReadToEnd();
            ApplyConfiguration(configurationXml);
        }

        public async Task<RoundResult> PerformNextRoundAsync()
        {
            _roundNumber++;

            await _explosionService.HandleExplodablesAsync(_delayTime);

            if (!_botService.AreMultipleBotsLeft)
            {
                return new RoundResult
                {
                    FinalResult = _botService.GetBotResults(),
                    IsFinished = true,
                    History = new List<RoundPartialHistory>()
                };
            }

            return await _botService.PlayBotMovesAsync(_delayTime, _roundNumber, _moveTimeout);
        }

        public UserControl GetVisualisationUserControl(IConfigurable configurable)
        {
            _delayTime = configurable.NextMoveDelay;
            _moveTimeout = configurable.MoveTimeout;
            return new TankBlasterUserControl(_field);
        }

        public void SetupNewGame(IEnumerable<ICompetitor> competitors)
        {
            do
            {
                Reset();

                _field.GenerateRandomBoard();

                _botService.SetUpBots(competitors);

                _field.OnArenaChanged();

            } while (!_locationService.CanBotsMeet());
        }

        public void Reset()
        {
            _field.Reset();
            _roundNumber = 0;
        }

        public void SetPreview(object boardState)
        {
            _field.ImportState((Battlefield)boardState);
            _field.OnArenaChanged();
        }

        public string GetGameRules()
        {
            Resources.indestructibleTile.Save("tank_blaster_indestructible_tile.png");
            Resources.regularTile.Save("tank_blaster_regular_tile.png");
            Resources.fortifiedTile.Save("tank_blaster_fortified_tile.png");
            Resources.bomb.Save("tank_blaster_bomb.png");
            Resources.missile.Save("tank_blaster_missile.png");
            Resources.hello_wars_example.Save("tank_blaster_example.png");
            Resources.tank1.Save("tank_blaster_tank.png");
            Resources.blast_example.Save("tank_blaster_blast_example.png");
            Resources.tank_blaster_chained_explosion.Save("tank_blaster_chained_explosion.png");
            Resources.board_explained.Save("tank_blaster_board_explained.png");
            return Resources.GameRules;
        }

        public void ApplyConfiguration(string configurationXml)
        {
            _gameConfig = new XmlSerializer<TankBlasterConfig>().Deserialize(configurationXml);
            _field = new Battlefield(_gameConfig.MapWidth, _gameConfig.MapHeight);
            _locationService = new LocationService(_field);
            _explosionService = new ExplosionService(_field, _gameConfig, _locationService);
            _botService = new BotService(_field, _gameConfig, _locationService);
        }

        public void ChangeDelayTime(int delayTime)
        {
            _delayTime = delayTime;
        }
    }
}