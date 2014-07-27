namespace SharpMTProto.Services
{
    public interface IGZipService
    {
        byte[] Unpack(byte[] packed);
    }
}