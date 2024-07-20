public class TextEditor
{
    public String Title { get; set; } = null!;
    public char[][] Content { get; set; } = null!;
    List<List<char>> Lines = new List<List<char>>();

    public TextEditor(String title, String content)
    {
        LoadContentToTextArray(content);
        PrintText();
    }

    private void LoadContentToTextArray(String content)
    {
        content = "This text is comprised of X characters, and the screen only accepts 15 per line, so you should see this printed on IDK lines.";
        int w = 15;

        //int w = Console.WindowWidth;
        int h = Console.WindowHeight - 2;

        for(int y = 0; true; y++)
        {
            
            for(int x = 0; ((y * w) + x) % w != 0; x++)
            {
                if((y * w) + x >= content.Length) return;

                Content[y][x] = content[(y * w) + x];
            }
        }
    }

    private void PrintText()
    {
        foreach (char[] line in Content)
        {
            foreach (char c in line)
            {
                System.Console.Write(c);
            }

            System.Console.WriteLine();
        }
    }
}