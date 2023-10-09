using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.ComponentModel;
using VideoPlayer.Models;
using System.Windows;
using System.IO;
using Newtonsoft.Json;
using System.Data.SqlTypes;

namespace VideoPlayer.Services
{
    // [JsonObject(IsReference = true)]
    public class FileIO
    {
        private string PATH { get; set; }
        

        public FileIO( string path)
        {
            PATH = path;
        }


        public List<FoldersModels> LoadData()
        {
            

            var fileExists = File.Exists(PATH);
            if (!fileExists)
            {
                File.CreateText(PATH).Dispose();
                return new List<FoldersModels>();
            }
            string json = File.ReadAllText(PATH);

            List<FoldersModels> currentButtons = JsonConvert.DeserializeObject<List<FoldersModels>>(json);
            return currentButtons;
        }
        public void SaveData(List<FoldersModels> btn)
        {
            string serializedButtons = JsonConvert.SerializeObject(btn);

            File.WriteAllText(PATH, serializedButtons);
        }
        public void SaveData(FoldersModels btn)
        {
            List<FoldersModels> allCurrentButtons = LoadData();
            allCurrentButtons.Add(btn);

            string serializedButtons = JsonConvert.SerializeObject(allCurrentButtons);

            File.WriteAllText(PATH, serializedButtons);
            //using (StreamWriter writer = File.CreateText(PATH))
            //{
            //    string output = JsonConvert.SerializeObject(_buttonList);
            //    writer.Write(output);
            //}
        }
    }
}
