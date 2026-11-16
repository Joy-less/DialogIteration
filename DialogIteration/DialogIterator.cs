using System.Text;

namespace DialogIteration;

public sealed class DialogIterator {
    public DialogIteratorSettings Settings { get; set; }

    public DialogIterator(DialogIteratorSettings? Settings = null) {
        this.Settings = Settings ?? new DialogIteratorSettings();
    }
    public double GetDelay(Rune Rune) {
        return Settings.BaseDelay * (
            Settings.PeriodRunes.Contains(Rune) ? Settings.PeriodDelayMultiplier :
            Settings.QuestionRunes.Contains(Rune) ? Settings.QuestionDelayMultiplier :
            Settings.ExclamationRunes.Contains(Rune) ? Settings.ExclamationDelayMultiplier :
            Settings.CommaRunes.Contains(Rune) ? Settings.CommaDelayMultiplier :
            Settings.NewlineRunes.Contains(Rune) ? Settings.NewlineDelayMultiplier :
            Settings.OpenBracketRunes.Contains(Rune) ? Settings.OpenBracketDelayMultiplier :
            Settings.CloseBracketRunes.Contains(Rune) ? Settings.CloseBracketDelayMultiplier :
            Settings.CharacterDelayMultiplier
        );
    }
    public bool IsPunctuation(Rune Rune) {
        return Settings.PeriodRunes.Contains(Rune)
            || Settings.QuestionRunes.Contains(Rune)
            || Settings.ExclamationRunes.Contains(Rune)
            || Settings.CommaRunes.Contains(Rune);
    }
    public async Task IterateAsync(string ParsedText, Func<Task<double>> YieldFrame, Action<int, Rune>? OnCharacter, Action<int, Rune, float>? OnSyllable, Func<bool>? ShouldSkip) {
        ArgumentNullException.ThrowIfNull(ParsedText);
        ArgumentNullException.ThrowIfNull(YieldFrame);

        // Convert text to runes
        Rune[] ParsedRunes = [.. ParsedText.EnumerateRunes()];

        // Get voice syllable indexes
        Dictionary<int, float> Syllables = [];
        for (int Index = 0; Index < ParsedRunes.Length;) {
            // Skip whitespace
            if (Rune.IsWhiteSpace(ParsedRunes[Index])) {
                Index++;
                continue;
            }

            // Find end of word
            int WordStart = Index;
            while (Index < ParsedRunes.Length && !Rune.IsWhiteSpace(ParsedRunes[Index])) {
                Index++;
            }

            // Find punctuation
            float WordPitch = Settings.BasePitch;
            bool FoundPunctuation = false;
            for (int SubIndex = WordStart; SubIndex < ParsedRunes.Length; SubIndex++) {
                if (Settings.PeriodRunes.Contains(ParsedRunes[SubIndex])) {
                    WordPitch *= Settings.StatementPitchMultiplier;
                    FoundPunctuation = true;
                    break;
                }
                else if (Settings.QuestionRunes.Contains(ParsedRunes[SubIndex])) {
                    WordPitch *= Settings.QuestionPitchMultiplier;
                    FoundPunctuation = true;
                    break;
                }
                else if (Settings.ExclamationRunes.Contains(ParsedRunes[SubIndex])) {
                    WordPitch *= Settings.ExclamationPitchMultiplier;
                    FoundPunctuation = true;
                    break;
                }
            }
            if (!FoundPunctuation) {
                WordPitch *= Settings.StatementPitchMultiplier;
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
        for (int Index = 0; Index < ParsedRunes.Length; Index++) {
            Rune CurrentRune = ParsedRunes[Index];

            // Cancel dialog iteration on skip
            if (ShouldSkip is not null && ShouldSkip()) {
                break;
            }

            // Show next rune
            if (OnCharacter is not null) {
                OnCharacter(Index + 1, CurrentRune);
            }

            // Play voice syllable
            if (OnSyllable is not null) {
                if (Syllables.TryGetValue(Index, out float SyllablePitch)) {
                    OnSyllable(Index + 1, CurrentRune, SyllablePitch);
                }
            }

            // Combine rune pairs
            while (Index + 1 < ParsedRunes.Length) {
                Rune CurrentFirstRune = ParsedRunes[Index];
                Rune CurrentSecondRune = ParsedRunes[Index + 1];

                bool IsCombinedPair = false;
                foreach ((Rune FirstInPair, Rune SecondInPair) in Settings.CombinedRunePairs) {
                    if (CurrentFirstRune == FirstInPair && CurrentSecondRune == SecondInPair) {
                        IsCombinedPair = true;
                        break;
                    }
                }

                if (IsCombinedPair) {
                    Index++;
                    if (OnCharacter is not null) {
                        OnCharacter(Index + 1, CurrentSecondRune);
                    }
                }
                else {
                    break;
                }
            }

            // Wait for rune delay (unless last rune)
            if (Index + 1 < ParsedRunes.Length) {
                double ElapsedSeconds = 0.0;
                double TargetSeconds = GetDelay(CurrentRune);
                while (ElapsedSeconds < TargetSeconds && !(ShouldSkip is not null && ShouldSkip())) {
                    ElapsedSeconds += await YieldFrame();
                }
            }
        }
        // Ensure all runes are shown
        if (OnCharacter is not null) {
            OnCharacter(-1, default);
        }
    }
}