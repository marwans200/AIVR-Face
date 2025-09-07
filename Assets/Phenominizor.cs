using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class Phenominizor
{
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

    private static readonly (string, string)[] Rules = new (string, string)[]
    {
        ("ph", "f"), ("ght", "t"), ("kn", "n"), ("wr", "r"),
        ("oo", "oo"), ("ee", "ee"), ("qu", "kw"), ("x", "ks"), ("c", "k")
    };

    private static readonly Dictionary<char, int> MouthShapeMap = new Dictionary<char, int>()
    {
        { 'a', 1 }, { 'e', 2 }, { 'i', 3 }, { 'o', 4 }, { 'u', 5 }, { 'y', 6 },
        { 'f', 7 }, { 'm', 8 }, { 'p', 8 }, { 'b', 8 }, { 't', 9 }, { 'd', 9 },
        { 's', 10 }, { 'z', 10 }, { 'l', 11 }, { 'r', 12 }, { 'k', 13 }, { 'g', 13 },
        { 'h', 14 }, { 'n', 15 }, { 'w', 16 }
    };

    public struct MouthFrame
    {
        public string letter;
        public int mouthCode;
        public int pauseAfter;
    }

    public static List<MouthFrame> ToMouthFrames(string sentence)
    {
        string[] tokens = Regex.Split(sentence, @"(\s+|[.,!?;:])");
        List<MouthFrame> frames = new List<MouthFrame>();

        foreach (string token in tokens)
        {
            if (string.IsNullOrEmpty(token)) continue;

            // Spaces = word gap
            if (Regex.IsMatch(token, @"\s+"))
            {
                if (frames.Count > 0)
                {
                    var last = frames[frames.Count - 1];
                    last.pauseAfter += 300; // slower word pause
                    frames[frames.Count - 1] = last;
                }
                continue;
            }

            // Punctuation = long pauses
            if (Regex.IsMatch(token, @"[.,!?;:]"))
            {
                if (frames.Count > 0)
                {
                    var last = frames[frames.Count - 1];
                    last.pauseAfter += GetPauseForPunctuation(token);
                    frames[frames.Count - 1] = last;
                }
                continue;
            }

            // Word-level phonetic conversion
            string phonetic;
            if (WordMap.ContainsKey(token.ToLower()))
                phonetic = WordMap[token.ToLower()];
            else
            {
                phonetic = token.ToLower();
                foreach (var (oldVal, newVal) in Rules)
                    phonetic = phonetic.Replace(oldVal, newVal);
            }

            // Letter → frames
            foreach (char c in phonetic)
            {
                if (!char.IsLetter(c)) continue;

                int code = MouthShapeMap.ContainsKey(c) ? MouthShapeMap[c] : 0;

                frames.Add(new MouthFrame
                {
                    letter = c.ToString(),
                    mouthCode = code,
                    pauseAfter = 120 // slower per-letter duration
                });
            }
        }

        return frames;
    }

    private static int GetPauseForPunctuation(string punctuation)
    {
        switch (punctuation)
        {
            case ".": return 700;
            case ",": return 400;
            case ";": return 450;
            case ":": return 450;
            case "!": return 700;
            case "?": return 700;
            default: return 200;
        }
    }
}
