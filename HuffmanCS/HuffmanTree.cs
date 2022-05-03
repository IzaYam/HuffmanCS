using System.Text;

namespace HuffmanCS {

  class HuffmanTree {
    readonly HuffmanNode root;
    readonly Dictionary<char, string> codes;
    readonly string filePath;
    readonly string fileContents;

    private HuffmanTree(HuffmanNode root, string filePath, string fileContents) {
      this.root = root;
      codes = new();
      InitCharCodes(root);
      this.filePath = filePath;
      this.fileContents = fileContents;
    }

    public HuffmanNode Root => root;
    public string FilePath => filePath;
    public string FileContents => fileContents;

    public void WriteToFile(string path) {
      FileStream stream = File.Open(path, FileMode.Create);
      BitWriter writer = new(stream, Encoding.UTF8, false);
      // Header: count of codes, then each char, code length and the code
      writer.Write(codes.Count);
      foreach (KeyValuePair<char, string> code in codes) {
        writer.Write(code.Key);
        writer.Write(code.Value.Length);
        foreach (char bit in code.Value) {
          writer.Write(bit == '1');
        }
        writer.FlushBits();
      }
      // Body: File contents
      foreach (char character in fileContents) {
        foreach (char bit in codes[character]) {
          writer.Write(bit == '1');
        }
      }
      writer.Flush();
    }

    void InitCharCodes(HuffmanNode? node, string code = "") {
      if (node == null) {
        return;
      }
      if (node.IsLeaf && node.Character != null && code.Length > 0) {
        codes.Add((char)node.Character, code);
        return;
      }
      InitCharCodes(node.Left, code + "0");
      InitCharCodes(node.Right, code + "1");
    }

    public static HuffmanTree CreateFromFile(string path) {
      string contents = File.ReadAllText(path, Encoding.UTF8);
      List<HuffmanNode> nodes = GetCharFrequencies(contents).Values.ToList();
      nodes.Sort();

      while (nodes.Count > 1) {
        HuffmanNode firstLastNode = nodes[nodes.Count - 1];
        HuffmanNode secondLastNode = nodes[nodes.Count - 2];

        int totalFreq = firstLastNode.Frequency + secondLastNode.Frequency;
        HuffmanNode node = new(left: secondLastNode, right: firstLastNode, frequency: totalFreq);
        nodes.RemoveRange(nodes.Count - 2, 2);
        nodes.Add(node);
        nodes.Sort();
      }

      return new HuffmanTree(nodes[0], path, contents);
    }

    public static string ReadFromFile(string path) {
      FileStream stream = File.OpenRead(path);
      BitReader reader = new(stream, Encoding.UTF8, false);
      // Header
      int count = reader.ReadInt32();
      Dictionary<string, char> codeLookup = new();
      for (int i = 0; i < count; ++i) {
        char key = reader.ReadChar();
        int codeLength = reader.ReadInt32();
        string value = "";
        for (int j = 0; j < codeLength; ++j) {
          value += (reader.ReadBit() ? "1" : "0");
        }
        reader.DiscardByte();
        codeLookup.Add(value, key);
      }
      string fileContents = "";
      string currentCode = "";
      // Body
      while (stream.Position != stream.Length) {
        currentCode += (reader.ReadBit() ? "1" : "0");
        if (codeLookup.ContainsKey(currentCode)) {
          fileContents += codeLookup[currentCode];
          currentCode = "";
        }
      }
      return fileContents;
    }

    static Dictionary<char, HuffmanNode> GetCharFrequencies(string contents) {
      Dictionary<char, HuffmanNode> nodes = new();
      foreach (char character in contents) {
        if (nodes.ContainsKey(character)) {
          nodes[character].IncrementFrequency();
          continue;
        }
        nodes.Add(character, new HuffmanNode(character, frequency: 1));
      }
      return nodes;
    }
  }
}
