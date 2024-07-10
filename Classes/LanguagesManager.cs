using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Game.Classes
{
    internal class LanguagesManager
    {
        private readonly JObject Subtitles;
        public bool Chose { get; private set; }

        public LanguagesManager(byte language)
        {
            string path = "C:\\Users\\aprendiz.informatica\\Desktop\\Terminal-Game---Varisten\\Languages\\"; // Caminho para os arquivos
            string[] languageOptions = new string[]
            {
                $"{path}en-usa.json",
                $"{path}pt-br.json",
            };
            
            try
            {
                // Carrega o conteúdo do arquivo JSON
                string jsonText = File.ReadAllText(languageOptions[language]);
                this.Subtitles = JObject.Parse(jsonText);
                this.Chose = true; // Para verificar se o processo foi bem sucedido
            }
            catch (Exception)
            {
                this.Chose = false;
                Console.WriteLine("Error! The file could not be obtained.\n");
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
    }
}
