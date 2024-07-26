﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Game
{
    public class LanguagesManager
    {
        public bool Chose { get; private set; }
        public string[] LanguageOptions { get; private set; } = new string[]
        {
            $"#en-usa.json",
            $"#pt-br.json",
        };

        private JObject Subtitles;

        public LanguagesManager() 
        {
            LanguageChoose();
        }

        public void LanguageChoose(int language = 0)
        {
            string path = "..\\..\\Languages\\"; // Caminho para os arquivos
            try
            {
                // Carrega o conteúdo do arquivo JSON
                string jsonText = File.ReadAllText(this.LanguageOptions[language].Replace("#", path));
                this.Subtitles = JObject.Parse(jsonText);
                this.Chose = true; // Para verificar se o processo foi bem sucedido
            }
            catch (Exception ex)
            {
                this.Chose = false;
                Console.WriteLine(ex.Message);
            }
        }

        public string GetSubtitle(string group, string chave)
        {
            try
            {
                return this.Subtitles[group][chave].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter legenda: {ex.Message}");
                return null;
            }
        }

        public string ShowSubtitle(string subtitle)
        {
            #region Show subtitles
            Console.WriteLine(subtitle);
            #endregion

            return subtitle;
        }
    }
}
