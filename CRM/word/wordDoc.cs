namespace SRMAgreement.word
{
    public static class wordDoc
    {
        public static async Task<string> CopyOriginalFile(string sourceFilePath, string newFileName, string destinationDirectoryPath)
        {
            if (File.Exists(sourceFilePath))
            {
                if (!Directory.Exists(destinationDirectoryPath))
                {
                    Directory.CreateDirectory(destinationDirectoryPath);
                }
                string destinationFilePath = Path.Combine(destinationDirectoryPath, $"{newFileName}.docx");
                File.Copy(sourceFilePath, destinationFilePath, overwrite: true);

                Console.WriteLine($"File copied to {destinationFilePath}");
                return destinationFilePath;
            }
            else
            {
                Console.WriteLine("Source file does not exist.");
            }
            return null;
        }
    }
}
