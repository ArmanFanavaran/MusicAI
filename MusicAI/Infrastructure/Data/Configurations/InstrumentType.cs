namespace MusicAI.Infrastructure.Data.Configurations
{
    public class InstrumentType
    {
        // MIDI program number as key
        public static Dictionary<byte, (string Instrument, byte MidiProgram)> typesWithMidiProgram = new Dictionary<byte, (string Instrument, byte MidiProgram)>
        {
            {1, ("Piano", 1)},                  // Acoustic Grand Piano (MIDI program 1)
            {2, ("Guitar", 25)},                // Acoustic Guitar (Nylon) (MIDI program 25)
            {3, ("Bass Guitar", 34)},           // Electric Bass (Finger) (MIDI program 34)
            {4, ("Recorder (Soprano)", 75)},    // Recorder (MIDI program 75)
            {5, ("Violin", 41)},                // Violin (MIDI program 41)
            {6, ("Cello", 43)},                 // Cello (MIDI program 43)
            {7, ("Double Bass", 44)},           // Contrabass (Double Bass) (MIDI program 44)
            {8, ("Harp", 46)},                  // Harp (MIDI program 46)
            {9, ("Soprano Saxophone", 65)},     // Soprano Sax (MIDI program 65)
            {10, ("Alto Saxophone", 66)},       // Alto Sax (MIDI program 66)
            {11, ("Tenor Saxophone", 67)},      // Tenor Sax (MIDI program 67)
            {12, ("Baritone Saxophone", 68)},   // Baritone Sax (MIDI program 68)
            {13, ("Flute", 73)},                // Flute (MIDI program 73)
            {14, ("Alto Flute", 73)},           // Flute (Alto Flute uses same MIDI program as regular Flute)
            {15, ("Piccolo", 72)},              // Piccolo (MIDI program 72)
            {16, ("Clarinet", 71)},             // Clarinet (MIDI program 71)
            {17, ("A Clarinet", 71)},           // Clarinet (Same MIDI program as standard clarinet)
            {18, ("Alto Clarinet", 71)},        // Clarinet (Same MIDI program as standard clarinet)
            {19, ("Bass Clarinet", 72)},        // Bass Clarinet (MIDI program 72)
            {20, ("Oboe", 69)},                 // Oboe (MIDI program 69)
            {21, ("Oboe d'Amore", 69)},         // Oboe d'Amore (Same MIDI program as regular Oboe)
            {22, ("English Horn", 70)},         // English Horn (MIDI program 70)
            {23, ("Bassoon", 71)},              // Bassoon (MIDI program 71)
            {24, ("Trumpet", 57)},              // Trumpet (MIDI program 57)
            {25, ("French Horn", 60)},          // French Horn (MIDI program 60)
            {26, ("Tenor Trombone", 58)},       // Trombone (MIDI program 58)
            {27, ("Soprano Ukulele", 24)},      // Acoustic Guitar (Steel) (Closest equivalent for Soprano Ukulele, MIDI program 24)
            {28, ("Concert Ukulele", 24)},      // Acoustic Guitar (Steel) (Closest equivalent for Concert Ukulele, MIDI program 24)
        };
    }
}
