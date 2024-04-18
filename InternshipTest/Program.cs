using InternshipTest;
using System.Text.RegularExpressions;

Dictionary<int, Stack<Container>> stacks = new Dictionary<int, Stack<Container>>();
string[] firstSection = new string[1];
string[] secondSection = new string[1];

// change input
string fileName = "input3.txt";
    GetFileInput(fileName);
    InitializeStacks();
    PopulateStacks(firstSection);
var instructions = GetInstructions(secondSection);


ContainerService containerService = new ContainerService(stacks);

// eliminate all white space lines from instructions
var cleanInput = secondSection.Where(line => !string.IsNullOrWhiteSpace(line));
var instructionsNumber = cleanInput.Count();
int currentIndex = 0;
for (int i = 0; i < instructionsNumber; i++)
{
    var containersToMove = instructions[currentIndex];
    var startingPoint = instructions[currentIndex + 1];
    var endingPoint = instructions[currentIndex + 2];

    currentIndex += 3;

    containerService.MoveContainer(containersToMove, startingPoint, endingPoint);
}


Console.WriteLine(containerService.GetTopContainersConcatenated());

foreach (var stack in stacks)
{
    Console.Write($"Stack {stack.Key + 1}:");
    Console.WriteLine();
    foreach (var container in stack.Value)
    {
        Console.WriteLine($"- {container.Letter}");
    }
}


void GetFileInput(string fileName)
{
    string solutionPath = @"..\\..\\..\\";
    string folderName = "Examples";
    string filePath = Path.Combine(solutionPath,folderName, fileName);

    string input = File.ReadAllText(filePath);

    string[] lines = input.Split('\n');

    // split input into 2 sections
    int emptyLineIndex = Array.FindIndex(lines, line => string.IsNullOrWhiteSpace(line));

    firstSection = lines.Take(emptyLineIndex).ToArray();
    secondSection = lines.Skip(emptyLineIndex + 1).ToArray();
}
void InitializeStacks()
{
    // -1 because array starts from 0
    int stackNumber = GetLastChar(firstSection[firstSection.Length - 1]);

    // create stacks
    for (int i = 0; i < stackNumber; i++)
    {
        stacks.Add(i, new Stack<Container>());
    }
}
void PopulateStacks(string[] firstSection)
{
    // -2 in order to start with first line of containers from bottom
    for (int i = firstSection.Length - 2; i >= 0; i--)
    {
        int spaceBetweenContainers = 4;
        int emptySpaces = 0;
        int stackNumber = 0;
        string line = firstSection[i];

        // check every char in lines
        for (int j = 0; j < line.Length; j++)
        {
            char c = line[j];

            if (c == '[')
            {
                // check if letter is between [ ]
                if (j + 2 < line.Length && char.IsLetter(line[j + 1]) && line[j + 2] == ']')
                {
                    try
                    {
                        stacks[stackNumber].Push(new Container(line[j + 1]));
                        stackNumber++;
                    } catch (KeyNotFoundException)
                    {
                        stackNumber = 0;
                    }
                }
            }
                // count the space between containers. If >= 4 it means empty space and move to the next stack
            else if (c == ' ')
            {
                emptySpaces++;
                if (emptySpaces >= spaceBetweenContainers)
                {
                    stackNumber++;
                    emptySpaces = 0;
                }
            }
        }
    }
}
List<int> GetInstructions(string[] secondSection)
{
    List<int> instructionsList = new List<int>();
    var instructionsNumber = secondSection.Length - 1;
    foreach (string line in secondSection)
    {
        MatchCollection matches = Regex.Matches(line, @"\d+");
        foreach (Match match in matches)
        {
            instructionsList.Add(int.Parse(match.Value));
        }
    }
    return instructionsList;
}
int GetLastChar(string line)
{
    string trimmedLine = line.Replace(" ", "");

    // -2 because last 2 chars are \r and \0 
    char lastChar = trimmedLine[trimmedLine.Length - 2];

    if (char.IsDigit(lastChar))
    {
        return int.Parse(lastChar.ToString());
    }
    else
    {
        throw new FormatException("Last char isn't a number.");
    }
}


