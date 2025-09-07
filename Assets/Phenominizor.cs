using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class Phenominizor
{
    // Common word mappings for more natural phonetics
    private static readonly Dictionary<string, string> WordMap = new Dictionary<string, string>()
    {
        { "you", "yu" }, { "are", "ar" }, { "is", "iz" }, { "am", "am" },
        { "the", "thuh" }, { "a", "uh" }, { "an", "an" }, { "and", "and" },
        { "be", "bee" }, { "been", "bin" }, { "was", "wuz" }, { "were", "wur" },
        { "do", "doo" }, { "does", "duz" }, { "did", "did" },
        { "have", "hav" }, { "has", "haz" }, { "had", "had" },
        { "will", "wil" }, { "would", "wud" }, { "can", "kan" },
        { "could", "kud" }, { "shall", "shal" }, { "should", "shud" },
        { "may", "mei" }, { "might", "mait" }, { "must", "must" },
        { "phone", "fone" }, { "through", "thru" }, { "enough", "enuf" },
        { "night", "nite" }, { "light", "lite" }, { "know", "no" },
        { "write", "rite" }, { "right", "rite" }
    };

    // General replacement rules
    private static readonly (string, string)[] Rules = new (string, string)[]
    {
        ("ph", "f"), ("ght", "t"), ("kn", "n"), ("wr", "r"),
        ("oo", "oo"), ("ee", "ee"), ("qu", "kw"), ("x", "ks"), ("c", "k")
    };

    // Map phoneme letters to mouth shape codes
    private static readonly Dictionary<char, int> MouthShapeMap = new Dictionary<char, int>()
    {
        { 'a', 1 }, { 'e', 2 }, { 'i', 3 }, { 'o', 4 }, { 'u', 5 }, { 'y', 6 },
        { 'f', 7 }, { 'm', 8 }, { 'p', 8 }, { 'b', 8 }, { 't', 9 }, { 'd', 9 },
        { 's', 10 }, { 'z', 10 }, { 'l', 11 }, { 'r', 12 }, { 'k', 13 }, { 'g', 13 },
        { 'h', 14 }, { 'n', 15 }, { 'w', 16 }
    };

    // Struct for each phoneme frame
    public struct MouthFrame
    {
        public string letter;    // The actual letter/phoneme
        public int mouthCode;    // Code for mouth shape
        public int pauseAfter;   // Pause duration after this frame (ms)
    }

    // Convert input sentence into mouth frames
    public static List<MouthFrame> ToMouthFrames(string sentence)
    {
        string[] tokens = Regex.Split(sentence, @"(\W)"); // split into words & punctuation
        List<MouthFrame> frames = new List<MouthFrame>();

        foreach (string token in tokens)
        {
            string trimmed = token.Trim();
            if (string.IsNullOrEmpty(trimmed)) continue;

            // Handle punctuation as pauses
            if (Regex.IsMatch(trimmed, @"[.,!?;:]"))
            {
                if (frames.Count > 0)
                {
                    var last = frames[frames.Count - 1];
                    last.pauseAfter += GetPauseForPunctuation(trimmed);
                    frames[frames.Count - 1] = last;
                }
                continue;
            }

            // Word-level phonetic conversion
            string phonetic;
            if (WordMap.ContainsKey(trimmed.ToLower()))
                phonetic = WordMap[trimmed.ToLower()];
            else
            {
                phonetic = trimmed.ToLower();
                foreach (var (oldVal, newVal) in Rules)
                    phonetic = phonetic.Replace(oldVal, newVal);
            }

            // Per-letter mouth codes
            foreach (char c in phonetic)
            {
                if (!char.IsLetter(c)) continue;

                int code = MouthShapeMap.ContainsKey(c) ? MouthShapeMap[c] : 0;

                frames.Add(new MouthFrame
                {
                    letter = c.ToString(),
                    mouthCode = code,
                    pauseAfter = 80 // default per-letter pause
                });
            }
        }

        return frames;
    }

    // Helper: pause values based on punctuation
    private static int GetPauseForPunctuation(string punctuation)
    {
        switch (punctuation)
        {
            case ".": return 500;
            case ",": return 250;
            case ";": return 300;
            case ":": return 300;
            case "!": return 500;
            case "?": return 500;
            default: return 100;
        }
    }
}
