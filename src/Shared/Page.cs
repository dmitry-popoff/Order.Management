using System.Text.Json.Serialization;

namespace Shared.Abstractions;

public readonly struct Page
{
    [JsonConstructor]
    public Page(int number = Page.DefaultPageNumber, int size = Page.DefaultPageSize) =>
        (Number, Size) = (number, size);

    public int Number { get; }
    public int Size { get; }

    public Page Next() => GetPage(this.Number + 1, this.Size);
    public Page Prev() => GetPage(this.Number - 1, this.Size);

    public const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 20;
    public static Page Default => new Page(DefaultPageNumber, DefaultPageSize);
    public static Page GetPage(int Number = Page.DefaultPageNumber, int Size = Page.DefaultPageSize) => 
        new Page(Number > 0 ? Number : Page.DefaultPageNumber, Size > 1 ? Size : Page.DefaultPageSize);
}
