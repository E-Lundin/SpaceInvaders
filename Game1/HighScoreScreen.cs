﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    class HighScoreScreen
    {
        public static HighScoreScreen self;
        public static List<HighScore> HighScores = new List<HighScore>();

        public HighScoreScreen()
        {
            self = this;
            LoadHighScores();
        }
        public static int? BiggestHighScore
        {
            get
            {
                if (!HighScores.Any())
                {
                    return null;
                }
                else
                {
                    int maxScore = HighScores.Max(s => s.Score);
                    return maxScore;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            Vector2 titleSize = Images.MenuFont.MeasureString("Leaderboard");
            Vector2 pos = new Vector2(Game.gameSize.Width / 2 - titleSize.X / 2, 5);
            spriteBatch.DrawString(Images.MenuFont, "Leaderboard", pos, Color.Lime);

            spriteBatch.DrawString(Images.MenuFont, "#", new Vector2(300, 100), Color.White);
            spriteBatch.DrawString(Images.MenuFont, "Score", new Vector2(450, 100), Color.White);

            if (!HighScores.Any())
            {
                return;
            }
            int count = 0;

            var entries = HighScores.OrderByDescending(s => s.Score).Take(5);
            foreach (HighScore entry in entries)
            {
                Vector2 textPosition = new Vector2(300, 150 + 50 * count);
                spriteBatch.DrawString(Images.MenuFont, entry.Score.ToString(), textPosition + new Vector2(150, 0), Color.White);
                spriteBatch.DrawString(Images.MenuFont, (count + 1).ToString(), textPosition, Color.White);
                count++;
            }
        }

        public void LoadHighScores()
        {
            var serializer = new XmlSerializer(HighScores.GetType());
            object obj;
            try
            {
                using (var reader = new StreamReader("highscores.xml"))
                {
                    obj = serializer.Deserialize(reader.BaseStream);
                }
                HighScores = (List<HighScore>)obj;
            }
            catch
            {
                return;
            }
        }

        public void SaveHighScores()
        {
            using (XmlWriter writer = XmlWriter.Create("highscores.xml"))
            {
                XmlSerializer ser = new XmlSerializer(HighScores.GetType());
                ser.Serialize(writer, HighScores);
                writer.Flush();
                writer.Close();
            }
        }
        public void LogScore(int score)
        {
            int minScore;

            if (HighScores.Any() && HighScores.Count >= 5)
            {
                var minEntry = HighScores.OrderByDescending(s => s.Score).Last();
                minScore = minEntry.Score;
                if (score > minScore)
                {
                    HighScores.Remove(minEntry);
                    HighScores.Add(new HighScore() { Score = score });
                }
            }
            else
            {
                HighScores.Add(new HighScore() { Score = score });
            }
            SaveHighScores();
        }
    }
}
