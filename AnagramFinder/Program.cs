
using System.Diagnostics;
using System.Text;

const int Buffer = 64*1024;

var sw = Stopwatch.StartNew();

using var sr = new StreamReader(File.OpenRead("documents/English.txt"), encoding: Encoding.UTF8, bufferSize: Buffer);
var fileContent = await sr.ReadToEndAsync();
var anagrams = fileContent
    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    .Select(w => w.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[0].ToLowerInvariant())
    .Select(w => new Tuple<string, string>(SortLetters(w), w))
    .Where(t => t.Item1.Length>1)
    .GroupBy(t => t.Item1, t => t.Item2)
    .Select(s => string.Join(',', s.Distinct()))
    .Where(s => s.IndexOf(',')>0)
    .ToArray();

sw.Stop();

foreach(var anagram in anagrams) Console.WriteLine(anagram);

Console.WriteLine($"took {sw.ElapsedMilliseconds} to find {anagrams.Length} anagram groups");

string SortLetters(string s) => string.Concat(s.Where(char.IsLetter).OrderBy(c => c));
