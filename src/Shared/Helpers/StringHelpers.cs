namespace Shared.Helpers;

public static class StringHelpers
{
    public static IEnumerable<ReadOnlyMemory<char>> Split(ReadOnlyMemory<char> input)
    {
        int wordStart = 0;
        for (int i = 0; i <= input.Length; i++)
            if (i == input.Length || char.IsWhiteSpace(input.Span[i]))
            {
                yield return input[wordStart..i];
                wordStart = i + 1;
            }
    }

    public static bool ToUpper(ref ReadOnlyMemory<char> original)
    {
        Span<char> span = stackalloc char[original.Length];
        original.Span.CopyTo(span);

        bool hasChanges = false;

        if (Char.IsUpper(span[0]) is false)
        {
            span[0] = Char.ToUpper(span[0]);
            hasChanges = true;
        }

        for (int i = 1; i < span.Length; i++)
        {
            if (Char.IsUpper(span[i]))
            {
                span[i] = Char.ToLower(span[i]);
                hasChanges = true;
            }
        }
        original = hasChanges ? span.ToString().AsMemory() : original;

        return hasChanges;
    }
}
