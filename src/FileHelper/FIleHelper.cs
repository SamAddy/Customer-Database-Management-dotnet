namespace Component.CustomersDatabase;

public class FileHelper
{
    private string _filePath;
    private FileInfo? _fileInfo;

    public FileHelper(string filepath)
    {
        if (string.IsNullOrWhiteSpace(filepath))
            throw new ArgumentException("File path cannot be empty or null.");

        _filePath = filepath;
        _fileInfo = new FileInfo(_filePath);

        if (!_fileInfo.Exists)
            throw new FileNotFoundException("File not found.", _filePath);
    }
    public string[]? GetAll()
    {
        try
        {
            var data = File.ReadAllLines(_filePath);
            return data;
        }
        catch (IOException e)
        {
            throw new FileNotFoundException("An error occurred while reading the file.", e);
        }
    }

    public void WriteToFile(string content)
    {
        File.AppendAllText(_filePath, content);
    }

    public void ReplaceLine(string lineToReplace, string newLine)
    {
        string [] lines = File.ReadAllLines(_filePath);
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] == lineToReplace)
            {
                lines[i] = newLine;
                break;
            }
        }
        File.WriteAllLines(_filePath, lines);
    }
    public class FileHelperException : Exception
    {
        public FileHelperException(string message) : base(message)
        {
        }

        public FileHelperException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}