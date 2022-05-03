using System.Text;
using System.Collections;

namespace HuffmanCS {
  class BitReader : BinaryReader {
    readonly bool[] currentByte = new bool[8];
    byte currentIndex = 0;
    bool shouldRead = true;

    public BitReader(Stream input) : base(input) { }
    public BitReader(Stream input, Encoding encoding) : base(input, encoding) { }
    public BitReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen) { }

    public bool ReadBit() => ReadBoolean();

    public void DiscardByte() => shouldRead = true;

    public override bool ReadBoolean() {
      if (shouldRead || currentIndex == 8) {
        _ = ReadByte();
        shouldRead = false;
      }
      bool bit = currentByte[currentIndex];
      currentIndex++;
      return bit;
    }

    public override byte ReadByte() {
      byte b = base.ReadByte();
      BitArray bitArray = new(new byte[] { b });
      bitArray.CopyTo(currentByte, 0);
      currentIndex = 0;
      return b;
    }

    public override byte[] ReadBytes(int count) {
      byte[] bytes = new byte[count];
      for (int i = 0; i < count; i++) {
        bytes[i] = ReadByte();
      }
      return bytes;
    }
  }
}
