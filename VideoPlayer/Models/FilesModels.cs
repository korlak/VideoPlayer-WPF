using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoPlayer.Models
{
    public static class Globals
    {
        public static string pathVideo;
    }
    public class FilesModels
    {
        public string name { get; set; }
        public string dateCreation { get; set; }
        public string videoLenght { get; set; }
        public string videoSize { get; set; }
        public string videoType { get; set; }
        public string path { get; set; }

        public FilesModels(string name, string dateCreation, string videoLenght, string videoSize, string videoType, string path)
        {
            this.name = name;
            this.dateCreation = dateCreation;
            this.videoLenght = videoLenght;
            this.videoSize = videoSize;
            this.videoType = videoType;
            this.path = path;
        }
        
    }
}
