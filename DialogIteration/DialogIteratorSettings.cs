using System.Text;

namespace DialogIteration;

/// <summary>
/// Settings for a <see cref="DialogIterator"/>.
/// </summary>
public sealed class DialogIteratorSettings {
    /// <summary>
    /// The base delay after any rune, in seconds.
    /// </summary>
    public double BaseDelay { get; set; } = 1.0;
    /// <summary>
    /// The delay multiplier after a non-special rune.
    /// </summary>
    public double NormalDelayMultiplier { get; set; } = 0.015;
    /// <summary>
    /// The delay multiplier after a period (full stop) or ellipsis.
    /// </summary>
    public double PeriodDelayMultiplier { get; set; } = 0.3;
    /// <summary>
    /// The delay multiplier after a question mark.
    /// </summary>
    public double QuestionMarkDelayMultiplier { get; set; } = 0.3;
    /// <summary>
    /// The delay multiplier after an exclamation mark.
    /// </summary>
    public double ExclamationMarkDelayMultiplier { get; set; } = 0.3;
    /// <summary>
    /// The delay multiplier after a comma, semicolon, or colon.
    /// </summary>
    public double SeparatorDelayMultiplier { get; set; } = 0.2;
    /// <summary>
    /// The delay multiplier after a newline.
    /// </summary>
    public double NewlineDelayMultiplier { get; set; } = 0.15;
    /// <summary>
    /// The delay multiplier after a symmetrical quotation mark or backtick.
    /// </summary>
    public double SymmetricalDelimiterDelayMultiplier { get; set; } = 0.0;
    /// <summary>
    /// The delay multiplier after a left bracket, square bracket, curly brace, or quotation mark.
    /// </summary>
    public double LeftDelimiterDelayMultiplier { get; set; } = 0.0;
    /// <summary>
    /// The delay multiplier after a right bracket, square bracket, curly brace, or quotation mark.
    /// </summary>
    public double RightDelimiterDelayMultiplier { get; set; } = 0.0;

    /// <summary>
    /// The maximum number of runes in a voice syllable.
    /// </summary>
    public int SyllableLength { get; set; } = 3;

    /// <summary>
    /// The pitch multiplier of a rune in any sentence.
    /// </summary>
    public float BasePitch { get; set; } = 1.0f;
    /// <summary>
    /// The pitch multiplier of a rune in a non-special sentence.
    /// </summary>
    public float NormalPitchMultiplier { get; set; } = 1.0f;
    /// <summary>
    /// The pitch multiplier of a rune in a question sentence.
    /// </summary>
    public float QuestionPitchMultiplier { get; set; } = 1.4f;
    /// <summary>
    /// The pitch multiplier of a rune in an exclamation sentence.
    /// </summary>
    public float ExclamationPitchMultiplier { get; set; } = 1.4f;

    /// <summary>
    /// A list of recognized period (full stop) and ellipsis runes.
    /// </summary>
    public HashSet<Rune> PeriodRunes { get; set; } = [
        new Rune('.'), // Half-width full stop
        new Rune('．'), // Full-width full stop

        new Rune('｡'), // Half-width ideographic full stop
        new Rune('。'), // Full-width ideographic full stop

        new Rune('…'), // Ellipsis
    ];
    /// <summary>
    /// A list of recognized question mark runes.
    /// </summary>
    public HashSet<Rune> QuestionMarkRunes { get; set; } = [
        new Rune('?'), // Half-width question mark
        new Rune('？'), // Full-width question mark

        new Rune('⁇'), // Double question mark
    ];
    /// <summary>
    /// A list of recognized exclamation mark runes.
    /// </summary>
    public HashSet<Rune> ExclamationMarkRunes { get; set; } = [
        new Rune('!'), // Half-width exclamation mark
        new Rune('！'), // Full-width exclamation mark

        new Rune('‼'), // Double exclamation mark
    ];
    /// <summary>
    /// A list of recognized comma, semicolon, and colon runes.
    /// </summary>
    public HashSet<Rune> SeparatorRunes { get; set; } = [
        new Rune(','), // Half-width comma
        new Rune('，'), // Full-width comma

        new Rune('､'), // Half-width ideographic comma
        new Rune('、'), // Full-width ideographic comma

        new Rune(';'), // Half-width semicolon
        new Rune('；'), // Full-width semicolon

        new Rune(':'), // Half-width colon
        new Rune('：'), // Full-width colon
    ];
    /// <summary>
    /// A list of recognized newline runes.
    /// </summary>
    public HashSet<Rune> NewlineRunes { get; set; } = [
        new Rune('\n'), // Line feed
        new Rune('\r'), // Carriage return
        new Rune('\u2028'), // Line separator
        new Rune('\u2029'), // Paragraph separator
        new Rune('\u0085'), // Next line
    ];
    /// <summary>
    /// A list of recognized symmetrical quotation mark and backtick runes.
    /// </summary>
    public HashSet<Rune> SymmetricalDelimiterRunes { get; set; } = [
        new Rune('"'), // Straight double quotation mark
        new Rune('\''), // Straight single quotation mark
        new Rune('`'), // Backtick
    ];
    /// <summary>
    /// A list of recognized left bracket, square bracket, curly brace, and quotation mark runes.
    /// </summary>
    public HashSet<Rune> LeftDelimiterRunes { get; set; } = [
        new Rune('('), // Half-width left bracket
        new Rune('（'), // Full-width left bracket
        
        new Rune('['), // Half-width left square bracket
        new Rune('［'), // Full-width left square bracket

        new Rune('{'), // Half-width left curly brace
        new Rune('｛'), // Full-width left curly brace

        new Rune('“'), // Left curly double quotation mark
        new Rune('‘'), // Left curly single quotation mark

        new Rune('〈'), // Left angle bracket
        new Rune('《'), // Left double angle bracket
        new Rune('「'), // Left corner bracket
        new Rune('『'), // Left white corner bracket
        new Rune('【'), // Left black lenticular bracket
        new Rune('〔'), // Left tortoise shell bracket
        new Rune('〖'), // Left white lenticular bracket
        new Rune('〘'), // Left white tortoise shell bracket
        new Rune('〚'), // Left white square bracket
    ];
    /// <summary>
    /// A list of recognized right bracket, square bracket, and curly brace, and quotation mark runes.
    /// </summary>
    public HashSet<Rune> RightDelimiterRunes { get; set; } = [
        new Rune(')'), // Half-width right bracket
        new Rune('）'), // Full-width right bracket

        new Rune(']'), // Half-width right square bracket
        new Rune('］'), // Full-width right square bracket

        new Rune('}'), // Half-width right curly brace
        new Rune('｝'), // Full-width right curly brace
        
        new Rune('”'), // Right curly double quotation mark
        new Rune('’'), // Right curly single quotation mark

        new Rune('〉'), // Right angle bracket
        new Rune('》'), // Right double angle bracket
        new Rune('」'), // Right corner bracket
        new Rune('』'), // Right white corner bracket
        new Rune('】'), // Right black lenticular bracket
        new Rune('〕'), // Right tortoise shell bracket
        new Rune('〗'), // Right white lenticular bracket
        new Rune('〙'), // Right white tortoise shell bracket
        new Rune('〛'), // Right white square bracket
    ];

    /// <summary>
    /// Constructs a <see cref="DialogIteratorSettings"/> instance with default values.
    /// </summary>
    public DialogIteratorSettings() {
    }
    /// <summary>
    /// Calculates the delay after the given rune, in seconds.
    /// </summary>
    public double GetDelay(Rune Rune) {
        return BaseDelay * (
            PeriodRunes.Contains(Rune) ? PeriodDelayMultiplier :
            QuestionMarkRunes.Contains(Rune) ? QuestionMarkDelayMultiplier :
            ExclamationMarkRunes.Contains(Rune) ? ExclamationMarkDelayMultiplier :
            SeparatorRunes.Contains(Rune) ? SeparatorDelayMultiplier :
            NewlineRunes.Contains(Rune) ? NewlineDelayMultiplier :
            SymmetricalDelimiterRunes.Contains(Rune) ? SymmetricalDelimiterDelayMultiplier :
            LeftDelimiterRunes.Contains(Rune) ? LeftDelimiterDelayMultiplier :
            RightDelimiterRunes.Contains(Rune) ? RightDelimiterDelayMultiplier :
            NormalDelayMultiplier
        );
    }
    /// <summary>
    /// Returns true if the given rune is considered to be a punctuation pause.<br/>
    /// This includes periods, question marks, exclamation marks, and separators.
    /// </summary>
    public bool IsPunctuationPause(Rune Rune) {
        return PeriodRunes.Contains(Rune)
            || QuestionMarkRunes.Contains(Rune)
            || ExclamationMarkRunes.Contains(Rune)
            || SeparatorRunes.Contains(Rune);
    }
}