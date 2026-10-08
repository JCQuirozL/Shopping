namespace Shopping.Helpers
{
    /// <summary>
    /// Lightweight translation service used instead of resx-based localization.
    /// Looks up the current UI culture (set by the Region selector / RequestLocalization
    /// middleware) and returns the matching text for the given key.
    /// </summary>
    public interface ITranslator
    {
        string this[string key] { get; }
    }
}
