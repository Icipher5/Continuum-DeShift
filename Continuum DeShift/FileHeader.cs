using System;

namespace Continuum_DeShift
{
    struct FileHeader
    {
        public Int32 iPacIdentifier;
        public Int32 iDataStart;
        public Int32 iFileSize;
        public Int32 iNumFiles;
        public Int32 iLanguage;
        public Int32 iFileNameLength;
        public string[] sFileNames;
        public Int32[] iFileNumber;
        public Int32[] iDataOffset;
        public Int32[] iFileSizes;

        public FileHeader(int numFiles)
        {
            iPacIdentifier  = 0x43415046; //FPAC in hex
            iDataStart      = new Int32();
            iFileSize       = new Int32();
            iNumFiles       = numFiles;
            iLanguage       = 0x01; //this is almost always 0x1, except when containing language text files it's 0x11
            iFileNameLength = new Int32();
            sFileNames      = new string[numFiles];
            iFileNumber     = new Int32[numFiles];
            iDataOffset     = new Int32[numFiles];
            iFileSizes      = new Int32[numFiles];
        }
    }
}