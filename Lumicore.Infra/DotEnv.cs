using System.Text;

namespace Lumicore.Infra;

public static class DotEnv
{
       public static void Load(string path, bool overwrite = false)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Path to .env must be provided", nameof(path));

        var fullPath = Path.GetFullPath(path);
        if (!File.Exists(fullPath))
            throw new FileNotFoundException($".env file not found at '{fullPath}'", fullPath);

        foreach (var kv in Parse(File.ReadAllLines(fullPath)))
        {
            var key = kv.Key;
            var value = kv.Value;

            var existing = Environment.GetEnvironmentVariable(key);
            if (existing == null || overwrite)
            {
                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }

    public static string Get(string key) => Environment.GetEnvironmentVariable(key)!;

    private static IEnumerable<KeyValuePair<string, string>> Parse(IEnumerable<string> lines)
    {
        foreach (var raw in lines)
        {
            var line = raw?.Trim() ?? string.Empty;
            if (line.Length == 0) continue; // skip empty
            if (line.StartsWith("#")) continue; // skip comments

            // allow inline comments when not in quotes
            string key;
            string value;

            var idx = line.IndexOf('=');
            if (idx <= 0)
                continue; // malformed line, ignore silently

            key = line[..idx].Trim();
            var valuePart = line[(idx + 1)..].Trim();

            if (valuePart.Length == 0)
            {
                value = string.Empty;
            }
            else if (valuePart.StartsWith("\""))
            {
                // quoted value
                value = ParseQuoted(valuePart);
            }
            else
            {
                // unquoted: stop at first unescaped # (comment)
                value = StripInlineComment(valuePart);
                value = value.Trim();
            }

            if (key.Length > 0)
                yield return new KeyValuePair<string, string>(key, value);
        }
    }

    private static string ParseQuoted(string input)
    {
        // expects starting with '"'
        var sb = new StringBuilder();
        bool inQuotes = false;
        bool escaped = false;
        for (int i = 0; i < input.Length; i++)
        {
            var c = input[i];
            if (!inQuotes)
            {
                if (c == '"')
                {
                    inQuotes = true;
                }
                continue; // ignore anything before the first quote
            }

            if (escaped)
            {
                sb.Append(c switch
                {
                    'n' => '\n',
                    'r' => '\r',
                    't' => '\t',
                    '"' => '"',
                    '\\' => '\\',
                    _ => c
                });
                escaped = false;
                continue;
            }

            if (c == '\\')
            {
                escaped = true;
                continue;
            }

            if (c == '"')
            {
                // end quote, rest can contain comment
                // find first # outside quotes -> we're outside already
                var rest = i + 1 < input.Length ? input[(i + 1)..] : string.Empty;
                // allow comment starting with # after spaces
                var commentIdx = IndexOfUnescapedHash(rest);
                if (commentIdx >= 0)
                    rest = rest[..commentIdx];
                rest = rest.Trim();
                // ignore rest content; value is sb.ToString()
                return sb.ToString();
            }

            sb.Append(c);
        }

        // if we reach here, closing quote missing; return what we have
        return sb.ToString();
    }

    private static string StripInlineComment(string input)
    {
        var idx = IndexOfUnescapedHash(input);
        var slice = idx >= 0 ? input[..idx] : input;
        // unescape common sequences in unquoted values too
        return Unescape(slice);
    }

    private static int IndexOfUnescapedHash(string input)
    {
        bool escaped = false;
        for (int i = 0; i < input.Length; i++)
        {
            var c = input[i];
            if (escaped)
            {
                escaped = false;
                continue;
            }
            if (c == '\\')
            {
                escaped = true;
                continue;
            }
            if (c == '#') return i;
        }
        return -1;
    }

    private static string Unescape(string s)
    {
        var sb = new StringBuilder(s.Length);
        bool escaped = false;
        foreach (var c in s)
        {
            if (escaped)
            {
                sb.Append(c switch
                {
                    'n' => '\n',
                    'r' => '\r',
                    't' => '\t',
                    '\\' => '\\',
                    _ => c
                });
                escaped = false;
            }
            else if (c == '\\')
            {
                escaped = true;
            }
            else
            {
                sb.Append(c);
            }
        }
        return sb.ToString().TrimEnd();
    }
}