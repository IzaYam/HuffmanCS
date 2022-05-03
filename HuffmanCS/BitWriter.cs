using System.Text;

namespace HuffmanCS {
  class BitWriter : BinaryWriter {
    bool[] currentByte = new bool[8];
    byte currentIndex = 0;

    public BitWriter() : base() { }
    public BitWriter(Stream output) : base(output) { }
    public BitWriter(Stream output, Encoding encoding) : base(output, encoding) { }
    public BitWriter(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen) { }

    public void FlushBits() {
      base.Write(ConvertToByte(currentByte));
      currentByte = new bool[8];
      currentIndex = 0;
    }

    public override void Flush() {
      FlushBits();
      base.Flush();
    }

    public override void Write(bool value) {
      currentByte[currentIndex] = value;
      currentIndex++;

      if (currentIndex == 8) {
        FlushBits();
      }
    }

    static byte ConvertToByte(bool[] currentByte) {
      byte b = 0;
      byte index = 0;
      for (int i = 0; i < 8; i++) {
        if (currentByte[i]) {
          b |= (byte)(1 << index);
        }
        index++;
      }
      return b;
    }
  }
}
