using System.Diagnostics;
using System.Text;

namespace DialogIteration;

/// <summary>
/// Iterates dialog character-by-character.
/// </summary>
public sealed class DialogIterator {
    /// <summary>
    /// The settings to use while iterating dialog.
    /// </summary>
    public DialogIteratorSettings Settings { get; set; }

    /// <summary>
    /// Constructs a <see cref="DialogIterator"/> instance with the given or default settings.
    /// </summary>
    public DialogIterator(DialogIteratorSettings? Settings = null) {
        this.Settings = Settings ?? new DialogIteratorSettings();
    }
    /// <summary>
    /// Iterates asynchronously through <paramref name="Text"/>, character-by-character.
    /// </summary>
    /// <param name="Text">
    /// The full text to be iterated through. It should not include any markdown or BBCode tags, unless these are intended to be displayed literally.
    /// </param>
    /// <param name="NextFrameAsync">
    /// A method that will asynchronously wait until the next frame, and return the number of seconds passed since the last frame.
    /// </param>
    /// <param name="OnRune">
    /// A method that is called when each rune is iterated.
    /// </param>
    /// <param name="OnSyllable">
    /// A method that is called when the first rune in each syllable is iterated.<br/>
    /// The method will be called immediately after <paramref name="OnRune"/>.
    /// </param>
    /// <param name="ShouldSkip">
    /// A method that is called repeatedly to check whether the iteration should be skipped.<br/>
    /// If this method returns true, the iteration will skip to after the last rune, and <paramref name="OnRune"/> will be called for the last rune.
    /// </param>
    public async Task IterateAsync(
        string Text,
        Func<Task<double>> NextFrameAsync,
        OnRuneDelegate? OnRune = null,
        OnSyllableDelegate? OnSyllable = null,
        ShouldSkipDelegate? ShouldSkip = null
    ) {
        ArgumentNullException.ThrowIfNull(Text);
        ArgumentNullException.ThrowIfNull(NextFrameAsync);

        Settings.ValidateProperties();

        // Convert text to runes
        List<(Rune Rune, int CharIndex)> Runes = GetRunesAndCharIndexesFromString(Text);

        // Get voice syllable indexes
        Dictionary<int, float> Syllables = [];
        for (int Index = 0; Index < Runes.Count;) {
            // Skip whitespace
            if (Rune.IsWhiteSpace(Runes[Index].Rune)) {
                Index++;
                continue;
            }

            // Find end of word
            int WordStart = Index;
            while (Index < Runes.Count && !Rune.IsWhiteSpace(Runes[Index].Rune)) {
                Index++;
            }

            // Find punctuation
            float WordPitch = Settings.BasePitch;
            bool FoundPunctuation = false;
            for (int SubIndex = WordStart; SubIndex < Runes.Count; SubIndex++) {
                if (Settings.PeriodRunes.Contains(Runes[SubIndex].Rune)) {
                    WordPitch *= Settings.NormalPitchMultiplier;
                    FoundPunctuation = true;
                    break;
                }
                else if (Settings.QuestionMarkRunes.Contains(Runes[SubIndex].Rune)) {
                    WordPitch *= Settings.QuestionPitchMultiplier;
                    FoundPunctuation = true;
                    break;
                }
                else if (Settings.ExclamationMarkRunes.Contains(Runes[SubIndex].Rune)) {
                    WordPitch *= Settings.ExclamationPitchMultiplier;
                    FoundPunctuation = true;
                    break;
                }
            }
            if (!FoundPunctuation) {
                WordPitch *= Settings.NormalPitchMultiplier;
            }

            // Add each syllable in word
            if (Settings.SyllableLength <= 0) {
                Syllables[WordStart] = WordPitch;
            }
            else {
                for (int SubIndex = WordStart; SubIndex < Index; SubIndex += Settings.SyllableLength) {
                    Syllables[SubIndex] = WordPitch;
                }
            }
        }

        // Show each rune after delay
        for (int Index = 0; Index < Runes.Count; Index++) {
            Rune CurrentRune = Runes[Index].Rune;

            // Check for dialog iteration skip
            if (ShouldSkip is not null && ShouldSkip()) {
                if (OnRune is not null) {
                    OnRune(Runes.Count, Text.Length, Runes[^1].Rune);
                }
                break;
            }

            // Show next rune
            if (OnRune is not null) {
                OnRune(Index, Runes[Index].CharIndex, CurrentRune);
            }

            // Play voice syllable
            if (OnSyllable is not null) {
                if (Syllables.TryGetValue(Index, out float SyllablePitch)) {
                    OnSyllable(Index, Runes[Index].CharIndex, SyllablePitch);
                }
            }

            // Combine rune pairs
            while (Index + 1 < Runes.Count) {
                Rune CurrentFirstRune = Runes[Index].Rune;
                Rune CurrentSecondRune = Runes[Index + 1].Rune;

                bool IsCombinedPair = false;
                if (Settings.NewlineRunes.Contains(CurrentFirstRune) && Settings.NewlineRunes.Contains(CurrentSecondRune)) {
                    IsCombinedPair = true;
                }
                else if (Settings.QuestionMarkRunes.Contains(CurrentFirstRune) && Settings.ExclamationMarkRunes.Contains(CurrentSecondRune)) {
                    IsCombinedPair = true;
                }
                else if (Settings.ExclamationMarkRunes.Contains(CurrentFirstRune) && Settings.QuestionMarkRunes.Contains(CurrentSecondRune)) {
                    IsCombinedPair = true;
                }
                else if (Settings.PeriodRunes.Contains(CurrentFirstRune) && (Settings.RightDelimiterRunes.Contains(CurrentSecondRune) || Settings.SymmetricalDelimiterRunes.Contains(CurrentSecondRune))) {
                    IsCombinedPair = true;
                }
                else if (Settings.QuestionMarkRunes.Contains(CurrentFirstRune) && (Settings.RightDelimiterRunes.Contains(CurrentSecondRune) || Settings.SymmetricalDelimiterRunes.Contains(CurrentSecondRune))) {
                    IsCombinedPair = true;
                }
                else if (Settings.ExclamationMarkRunes.Contains(CurrentFirstRune) && (Settings.RightDelimiterRunes.Contains(CurrentSecondRune) || Settings.SymmetricalDelimiterRunes.Contains(CurrentSecondRune))) {
                    IsCombinedPair = true;
                }

                if (IsCombinedPair) {
                    Index++;
                    if (OnRune is not null) {
                        OnRune(Index, Runes[Index].CharIndex, CurrentSecondRune);
                    }
                }
                else {
                    break;
                }
            }

            // Wait for rune delay (unless last rune)
            if (Index + 1 < Runes.Count) {
                double RuneDelay = Settings.GetDelay(CurrentRune);
                long StartTimestamp = Stopwatch.GetTimestamp();

                while (true) {
                    if (ShouldSkip is not null && ShouldSkip()) {
                        break;
                    }

                    if (Stopwatch.GetElapsedTime(StartTimestamp).TotalSeconds >= RuneDelay) {
                        break;
                    }

                    await NextFrameAsync();
                }
            }
        }
    }

    /// <summary>
    /// Creates a list of tuples from the given string, where each tuple contains a UTF-32 rune and a UTF-16 char index.
    /// </summary>
    private static List<(Rune Rune, int CharIndex)> GetRunesAndCharIndexesFromString(string String) {
        // See https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Text/StringRuneEnumerator.cs

        List<(Rune Rune, int CharIndex)> Runes = [with(capacity: String.Length)];

        int CurrentCharIndex = 0;

        while ((uint)CurrentCharIndex < String.Length) {
            if (!Rune.TryGetRuneAt(String, CurrentCharIndex, out Rune CurrentRune)) {
                CurrentRune = Rune.ReplacementChar;
            }

            Runes.Add((CurrentRune, CurrentCharIndex));

            CurrentCharIndex += CurrentRune.Utf16SequenceLength;
        }

        return Runes;
    }

    /// <summary>
    /// A delegate that is called by <see cref="IterateAsync(string, Func{Task{double}}, OnRuneDelegate?, OnSyllableDelegate?, ShouldSkipDelegate?)"/>
    /// when each rune is iterated.
    /// </summary>
    /// <param name="RuneIndex">
    /// The index of the current rune, which should now be displayed.<br/>
    /// If the text contains any UTF-16 surrogate pairs, this value may not correspond to the UTF-16 char index.<br/>
    /// To get the total number of runes to display, add 1 to this value.
    /// </param>
    /// <param name="CharIndex">
    /// The index of the current char, which should now be displayed.<br/>
    /// If the text contains any UTF-16 surrogate pairs, this value may not correspond to the UTF-32 rune index.<br/>
    /// To get the total number of chars to display, add 1 to this value.
    /// </param>
    /// <param name="Rune">
    /// The current rune.<br/>
    /// If you need the full list of runes, call <see cref="string.EnumerateRunes()"/> on the full text.
    /// </param>
    public delegate void OnRuneDelegate(int RuneIndex, int CharIndex, Rune Rune);
    /// <summary>
    /// A delegate that is called by <see cref="IterateAsync(string, Func{Task{double}}, OnRuneDelegate?, OnSyllableDelegate?, ShouldSkipDelegate?)"/>
    /// when the first rune in each syllable is iterated.
    /// </summary>
    /// <param name="FirstRuneIndex">
    /// The index of the current rune, which is the first rune in the syllable.<br/>
    /// If the text contains any UTF-16 surrogate pairs, this value may not correspond to the UTF-16 char index.
    /// </param>
    /// <param name="FirstCharIndex">
    /// The index of the current char, which is the first char in the syllable.<br/>
    /// If the text contains any UTF-16 surrogate pairs, this value may not correspond to the UTF-32 rune index.
    /// </param>
    /// <param name="SyllablePitch">
    /// The pitch multiplier to use when playing a voice for this syllable.
    /// </param>
    public delegate void OnSyllableDelegate(int FirstRuneIndex, int FirstCharIndex, float SyllablePitch);
    /// <summary>
    /// A delegate that is called by <see cref="IterateAsync(string, Func{Task{double}}, OnRuneDelegate?, OnSyllableDelegate?, ShouldSkipDelegate?)"/>
    /// repeatedly to check whether the iteration should be skipped.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the iteration should be skipped; otherwise, <see langword="false"/>.
    /// </returns>
    public delegate bool ShouldSkipDelegate();
}