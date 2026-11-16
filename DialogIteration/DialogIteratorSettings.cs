using System.Text;

namespace DialogIteration;

/// <summary>
/// Settings for a <see cref="DialogIterator"/>.
/// </summary>
public sealed class DialogIteratorSettings {
    /// <summary>
    /// The base delay after any character.
    /// </summary>
    public double BaseDelay { get; set; } = 1.0;
    /// <summary>
    /// The delay multiplier after a non-special character.
    /// </summary>
    public double CharacterDelayMultiplier { get; set; } = 0.015;
    /// <summary>
    /// The delay multiplier after a period (full stop).
    /// </summary>
    public double PeriodDelayMultiplier { get; set; } = 0.3;
    /// <summary>
    /// The delay multiplier after a question mark.
    /// </summary>
    public double QuestionDelayMultiplier { get; set; } = 0.3;
    /// <summary>
    /// The delay multiplier after an exclamation mark.
    /// </summary>
    public double ExclamationDelayMultiplier { get; set; } = 0.3;
    /// <summary>
    /// The delay multiplier after a comma, semicolon, or colon.
    /// </summary>
    public double CommaDelayMultiplier { get; set; } = 0.2;
    /// <summary>
    /// The delay multiplier after a newline.
    /// </summary>
    public double NewlineDelayMultiplier { get; set; } = 0.15;
    /// <summary>
    /// The delay multiplier after an open bracket, open square bracket, or open curly brace.
    /// </summary>
    public double OpenBracketDelayMultiplier { get; set; } = 0.0;
    /// <summary>
    /// The delay multiplier after a close bracket, close square bracket, or close curly brace.
    /// </summary>
    public double CloseBracketDelayMultiplier { get; set; } = 0.0;

    /// <summary>
    /// The maximum number of characters in a voice syllable.
    /// </summary>
    public int SyllableLength { get; set; } = 3;

    /// <summary>
    /// The pitch multiplier of a character in any sentence.
    /// </summary>
    public float BasePitch { get; set; } = 1.0f;
    /// <summary>
    /// The pitch multiplier of a character in a non-special sentence.
    /// </summary>
    public float StatementPitchMultiplier { get; set; } = 1.0f;
    /// <summary>
    /// The pitch multiplier of a character in a question sentence.
    /// </summary>
    public float QuestionPitchMultiplier { get; set; } = 1.4f;
    /// <summary>
    /// The pitch multiplier of a character in an exclamation sentence.
    /// </summary>
    public float ExclamationPitchMultiplier { get; set; } = 1.4f;

    /// <summary>
    /// A list of recognized period (full stop) characters.
    /// </summary>
    public HashSet<Rune> PeriodRunes { get; set; } = [
        new Rune('.'), // Half-width full stop
        new Rune('．'), // Full-width full stop

        new Rune('｡'), // Half-width ideographic full stop
        new Rune('。'), // Full-width ideographic full stop

        new Rune('…'), // Ellipsis
    ];
    /// <summary>
    /// A list of recognized question mark characters.
    /// </summary>
    public HashSet<Rune> QuestionRunes { get; set; } = [
        new Rune('?'), // Half-width question mark
        new Rune('？'), // Full-width question mark

        new Rune('⁇'), // Double question mark
    ];
    /// <summary>
    /// A list of recognized exclamation mark characters.
    /// </summary>
    public HashSet<Rune> ExclamationRunes { get; set; } = [
        new Rune('!'), // Half-width exclamation mark
        new Rune('！'), // Full-width exclamation mark

        new Rune('‼'), // Double exclamation mark
    ];
    /// <summary>
    /// A list of recognized comma, semicolon, and colon characters.
    /// </summary>
    public HashSet<Rune> CommaRunes { get; set; } = [
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
    /// A list of recognized newline characters.
    /// </summary>
    public HashSet<Rune> NewlineRunes { get; set; } = [
        new Rune('\n'), // Line feed
        new Rune('\r'), // Carriage return
        new Rune('\u2028'), // Line separator
        new Rune('\u2029'), // Paragraph separator
        new Rune('\u0085'), // Next line
    ];
    /// <summary>
    /// A list of recognized open bracket, open square bracket, and open curly brace characters.
    /// </summary>
    public HashSet<Rune> OpenBracketRunes { get; set; } = [
        new Rune('('), // Half-width open bracket
        new Rune('（'), // Full-width open bracket
        
        new Rune('['), // Half-width open square bracket
        new Rune('［'), // Full-width open square bracket

        new Rune('{'), // Half-width open curly brace
        new Rune('｛'), // Full-width open curly brace

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
    /// A list of recognized close bracket, close square bracket, and close curly brace characters.
    /// </summary>
    public HashSet<Rune> CloseBracketRunes { get; set; } = [
        new Rune(')'), // Half-width close bracket
        new Rune('）'), // Full-width close bracket

        new Rune(']'), // Half-width close square bracket
        new Rune('］'), // Full-width close square bracket

        new Rune('}'), // Half-width close curly brace
        new Rune('｝'), // Full-width close curly brace

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
    /// A list of character pairs that should be iterated as one unit,
    /// for example a question mark followed by an exclamation mark.
    /// </summary>
    public HashSet<(Rune First, Rune Second)> CombinedRunePairs { get; set; } = [
        (new Rune('\r'), new Rune('\n')), // Carriage return + Line feed

        (new Rune('?'), new Rune('!')), // Half-width question mark + Half-width exclamation mark
        (new Rune('?'), new Rune('！')), // Half-width question mark + Full-width exclamation mark
        (new Rune('?'), new Rune('‼')), // Half-width question mark + Double exclamation mark

        (new Rune('？'), new Rune('!')), // Full-width question mark + Half-width exclamation mark
        (new Rune('？'), new Rune('！')), // Full-width question mark + Full-width exclamation mark
        (new Rune('？'), new Rune('‼')), // Full-width question mark + Double exclamation mark

        (new Rune('⁇'), new Rune('!')), // Double question mark + Half-width exclamation mark
        (new Rune('⁇'), new Rune('！')), // Double question mark + Full-width exclamation mark
        (new Rune('⁇'), new Rune('‼')), // Double question mark + Double exclamation mark

        (new Rune('!'), new Rune('?')), // Half-width exclamation mark + Half-width question mark
        (new Rune('!'), new Rune('？')), // Half-width exclamation mark + Full-width question mark
        (new Rune('!'), new Rune('⁇')), // Half-width exclamation mark + Double question mark

        (new Rune('！'), new Rune('?')), // Full-width exclamation mark + Half-width question mark
        (new Rune('！'), new Rune('？')), // Full-width exclamation mark + Full-width question mark
        (new Rune('！'), new Rune('⁇')), // Full-width exclamation mark + Double question mark

        (new Rune('‼'), new Rune('?')), // Double exclamation mark + Half-width question mark
        (new Rune('‼'), new Rune('？')), // Double exclamation mark + Full-width question mark
        (new Rune('‼'), new Rune('⁇')), // Double exclamation mark + Double question mark

        (new Rune('.'), new Rune(')')), // Half-width full stop + Half-width close bracket
        (new Rune('.'), new Rune('）')), // Half-width full stop + Full-width close bracket
        
        (new Rune('．'), new Rune(')')), // Full-width full stop + Half-width close bracket
        (new Rune('．'), new Rune('）')), // Full-width full stop + Full-width close bracket

        (new Rune('｡'), new Rune(')')), // Half-width ideographic full stop + Half-width close bracket
        (new Rune('｡'), new Rune('）')), // Half-width ideographic full stop + Full-width close bracket

        (new Rune('。'), new Rune(')')), // Full-width ideographic full stop + Half-width close bracket
        (new Rune('。'), new Rune('）')), // Full-width ideographic full stop + Full-width close bracket

        (new Rune('…'), new Rune(')')), // Ellipsis + Half-width close bracket
        (new Rune('…'), new Rune('）')), // Ellipsis + Full-width close bracket

        (new Rune('?'), new Rune(')')), // Half-width question mark + Half-width close bracket
        (new Rune('?'), new Rune('）')), // Half-width question mark + Full-width close bracket

        (new Rune('？'), new Rune(')')), // Full-width question mark + Half-width close bracket
        (new Rune('？'), new Rune('）')), // Full-width question mark + Full-width close bracket

        (new Rune('⁇'), new Rune(')')), // Double question mark + Half-width close bracket
        (new Rune('⁇'), new Rune('）')), // Double question mark + Full-width close bracket

        (new Rune('!'), new Rune(')')), // Half-width exclamation mark + Half-width close bracket
        (new Rune('!'), new Rune('）')), // Half-width exclamation mark + Full-width close bracket

        (new Rune('！'), new Rune(')')), // Full-width exclamation mark + Half-width close bracket
        (new Rune('！'), new Rune('）')), // Full-width exclamation mark + Full-width close bracket

        (new Rune('‼'), new Rune(')')), // Double exclamation mark + Half-width close bracket
        (new Rune('‼'), new Rune('）')), // Double exclamation mark + Full-width close bracket
    ];

    /// <summary>
    /// Constructs a <see cref="DialogIteratorSettings"/> instance with default values.
    /// </summary>
    public DialogIteratorSettings() {
    }
}