using HuffmanCS;
using System.Text;
using System.Collections;

Console.Write("Compress or uncompress? (press c or u) ");
ConsoleKeyInfo keyInfo = Console.ReadKey();
Console.WriteLine();

if (keyInfo.Key == ConsoleKey.U) {
  Console.Write("Enter input file path: ");
  string? inputFilePath = Console.ReadLine();
  if (string.IsNullOrEmpty(inputFilePath)) {
    Console.WriteLine("Invalid input file path.");
    Environment.Exit(1);
  }
  Console.Write("Enter output file path: ");
  string? outputFilePath = Console.ReadLine();
  if (string.IsNullOrEmpty(outputFilePath)) {
    Console.WriteLine("Invalid output file path.");
    Environment.Exit(1);
  }
  string fileContents = HuffmanTree.ReadFromFile(inputFilePath);
  File.WriteAllText(outputFilePath, fileContents, Encoding.UTF8);
  Console.WriteLine($"Successfully written original contents to {outputFilePath}.");
  Environment.Exit(0);
} else if (keyInfo.Key == ConsoleKey.C) {
  Console.Write("Enter input file path: ");
  string? inputFilePath = Console.ReadLine();
  if (string.IsNullOrEmpty(inputFilePath)) {
    Console.WriteLine("Invalid file path.");
    Environment.Exit(1);
  }
  Console.Write("Enter output file path: ");
  string? outputFilePath = Console.ReadLine();
  if (string.IsNullOrEmpty(outputFilePath)) {
    Console.WriteLine("Invalid output file path.");
    Environment.Exit(1);
  }
  HuffmanTree tree = HuffmanTree.CreateFromFile(inputFilePath);
  tree.WriteToFile(outputFilePath);
  Console.WriteLine($"Successfully written compressed file to {outputFilePath}.");
  Environment.Exit(0);
} else if (keyInfo.Key == ConsoleKey.D) {
  Console.Write("Enter input file path: ");
  string? inputFilePath = Console.ReadLine();
  if (string.IsNullOrEmpty(inputFilePath)) {
    Console.WriteLine("Invalid file path.");
    Environment.Exit(1);
  }
  FileStream stream = File.OpenRead(inputFilePath);
  BinaryReader reader = new(stream, Encoding.UTF8, false);
  string content = "";
  bool[] currentByte = new bool[8];
  while (stream.Length != stream.Position) {
    BitArray bitArray = new(new byte[] { reader.ReadByte() });
    bitArray.CopyTo(currentByte, 0);
    for (int i = 0; i < 8; ++i) {
      content += (currentByte[i] ? "1" : "0");
    }
    content += " ";
  }
  Console.WriteLine(content);
}

Console.WriteLine("Invalid key pressed.");
Environment.Exit(1);
