namespace HuffmanCS {
  class HuffmanNode : IEquatable<HuffmanNode>, IComparable<HuffmanNode> {
    readonly char? character;
    int frequency;
    HuffmanNode? left;
    HuffmanNode? right;

    public HuffmanNode(char? character = null, int frequency = 0, HuffmanNode? left = null, HuffmanNode? right = null) {
      this.character = character;
      this.frequency = frequency;
      this.left = left;
      this.right = right;
    }

    public void IncrementFrequency(int count = 1) => frequency += count;

    public bool IsLeaf => character.HasValue;

    public override string ToString() {
      if (character == null) {
        return string.Empty;
      }
      return $"Character: {character}, frequency: {frequency}";
    }

    public override int GetHashCode() => character.GetHashCode();

    public override bool Equals(object? obj) {
      if (obj == null || obj is not HuffmanNode other) {
        return false;
      }
      return Equals(other);
    }

    public bool Equals(HuffmanNode? other) {
      if (other == null) {
        return false;
      }
      return other.character == character;
    }

    public int CompareTo(HuffmanNode? other) {
      if (other == null) {
        return 1;
      }
      if (other.character == character) {
        return 0;
      }
      if (other.frequency == frequency && character is not null) {
        return character.Value.CompareTo(other.character);
      }
      return -frequency.CompareTo(other.frequency);
    }

    public char? Character => character;
    public int Frequency {
      get => frequency;
      set => frequency = value;
    }
    public HuffmanNode? Left {
      get => left;
      set => left = value;
    }
    public HuffmanNode? Right {
      get => right;
      set => right = value;
    }
  }
}
