using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHOTOTINDER
{
    public class FileService
    {
        private readonly string _likeDirectoryPath;
        private readonly string _dislikeDirectoryPath;
        public DirectoryInfo SourceDirectoryInfo { get; private set; }
        private DirectoryInfo LikeSubDirectory { get; set; }
        private DirectoryInfo DislikeSubDirectory { get; set; }

        public FileService(string likeDirectoryPath, string dislikeDirectoryPath)
        {
            _likeDirectoryPath = likeDirectoryPath;
            _dislikeDirectoryPath = dislikeDirectoryPath;
        }

        public void SetSourceDirectory(string sourceDirectoryPath)
        {
            SourceDirectoryInfo = new DirectoryInfo(sourceDirectoryPath);
        }
        public bool SourceDirectoryExists()
        {
            return SourceDirectoryInfo.Exists;
        }
        public FileInfo[] PrintDirectoryContent()
        {
            FileInfo[] files = SourceDirectoryInfo.GetFiles("*.JPG");
            return files;
        }
        public void CreateWorkDirectories()
        {
            LikeSubDirectory = CreateDirectory(_likeDirectoryPath);
            DislikeSubDirectory = CreateDirectory(_dislikeDirectoryPath);
        }

        public void MoveFileToLikeSubDirectory(string fileNameForMoving)
        {
            MoveFile(LikeSubDirectory, fileNameForMoving);

        }
        public void MoveFileToDislikeSubDirectory(string fileNameForMoving)
        {
            MoveFile(DislikeSubDirectory, fileNameForMoving);
        }
        private DirectoryInfo CreateDirectory(string newDirectoryName)
        {
            string subDirectoryName = string.Concat(SourceDirectoryInfo, "\\", newDirectoryName);
            DirectoryInfo subDirectory = new DirectoryInfo(subDirectoryName);
            if (!subDirectory.Exists)
            {
                subDirectory.Create();
            }
            return subDirectory;
        }
        private void MoveFile(DirectoryInfo directoryNameForMoving, string fileNameForMoving)
        {
            string oldPath = string.Concat(SourceDirectoryInfo.FullName, "\\", fileNameForMoving);
            string newPath = string.Concat(directoryNameForMoving.FullName, "\\", fileNameForMoving);
            FileInfo evaluatedFile = new FileInfo(oldPath);
            if (evaluatedFile.Exists)
            {
                evaluatedFile.MoveTo(newPath);
            }
            else
            {
                Console.WriteLine("Файл не найден");
            }
        }
    }
}